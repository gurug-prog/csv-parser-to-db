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
    private readonly string _duplicatesFilePath;

    public CsvParserDomainService(
        IEqualityComparer<TripEntity> comparer,
        string datasetFilePath,
        string duplicatesFilePath)
    {
        _comparer = comparer;
        _uniqueRecords = new(_comparer);
        _duplicateRecords = new();
        _datasetFilePath = datasetFilePath;
        _duplicatesFilePath = duplicatesFilePath;
    }

    public async Task<int> RemoveDuplicates()
    {
        using var reader = new StreamReader(_datasetFilePath);
        using var originalCsv = new CsvReader(reader,
            new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            });

        IAsyncEnumerable<TripEntity> records = originalCsv.GetRecordsAsync<TripEntity>();
        await foreach (var record in records)
        {
            record.TpepDropoffDatetime = record.TpepDropoffDatetime.ToUniversalTime();
            record.TpepPickupDatetime = record.TpepPickupDatetime.ToUniversalTime();
            record.StoreAndFwdFlag = record.StoreAndFwdFlag.Trim() switch
            {
                "Y" => "Yes",
                "N" => "No",
                _ => throw new Exception("Invalid data was found in store_and_fwd_flag field...")
            };

            if (!_uniqueRecords.Add(record))
            {
                _duplicateRecords.Add(record);
            }
        }

        await WriteToCsv(_duplicateRecords, _duplicatesFilePath);
        await WriteToCsv(_uniqueRecords, _datasetFilePath);
        return _duplicateRecords.Count;
    }

    private static async Task WriteToCsv<T>(IEnumerable<T> records, string filePath)
    {
        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
        await csv.WriteRecordsAsync(records);
    }
}
