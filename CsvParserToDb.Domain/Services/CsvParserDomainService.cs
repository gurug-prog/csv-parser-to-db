using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using CsvParserToDb.Domain.Shared.Entities;

namespace CsvParserToDb.Domain.Services;

public class CsvParserDomainService
{
    private readonly IEqualityComparer<TripEntity> _comparer;
    private readonly HashSet<TripEntity> _uniqueRecords;
    private readonly List<TripEntity> _duplicateRecords;
    private readonly string _datasetFilePath;

    public CsvParserDomainService(
        IEqualityComparer<TripEntity> comparer,
        string datasetFilePath)
    {
        _comparer = comparer;
        _uniqueRecords = new(_comparer);
        _duplicateRecords = new();
        _datasetFilePath = datasetFilePath;
    }

    public async Task<int> RemoveDuplicates(string outputDuplicatesPath)
    {
        using (var reader = new StreamReader(_datasetFilePath))
        using (var originalCsv = new CsvReader(reader,
            new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            }))
        {
            originalCsv.Context.RegisterClassMap<TripEntityCsvMap>();

            TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            IAsyncEnumerable<TripEntity> records = originalCsv.GetRecordsAsync<TripEntity>();
            await foreach (var record in records)
            {
                record.TpepDropoffDatetime = TimeZoneInfo.ConvertTimeToUtc(record.TpepDropoffDatetime, est);
                record.TpepPickupDatetime = TimeZoneInfo.ConvertTimeToUtc(record.TpepPickupDatetime, est);
                record.StoreAndFwdFlag = record.StoreAndFwdFlag?.Trim() switch
                {
                    "Y" => "Yes",
                    "N" => "No",
                    _ => null,
                };

                if (!_uniqueRecords.Add(record))
                {
                    _duplicateRecords.Add(record);
                }
            }
        }

        Task.WaitAll(
            WriteToCsv(_duplicateRecords, outputDuplicatesPath),
            WriteToCsv(_uniqueRecords, _datasetFilePath));

        return _duplicateRecords.Count;
    }

    private static async Task WriteToCsv<T>(IEnumerable<T> records, string filePath)
    {
        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
        csv.Context.RegisterClassMap<TripEntityCsvMap>();
        await csv.WriteRecordsAsync(records);
    }
}
