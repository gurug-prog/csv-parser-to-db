using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using CsvParserToDb.Domain.Shared.Entities;

namespace CsvParserToDb.Domain.Services;

public class CsvParserService
{
    private readonly HashSet<TripEntity> _uniqueRecords;
    private readonly List<TripEntity> _duplicateRecords;
    private readonly IEqualityComparer<TripEntity> _comparer;
    private readonly string _datasetFilePath;
    private readonly string _duplicatesFilePath;

    public CsvParserService(
        IEqualityComparer<TripEntity> comparer,
        string datasetFilePath,
        string duplicatesFilePath)
    {
        _comparer = new TripDuplicateEqualityComparer();
        //_comparer = comparer;
        _uniqueRecords = new(_comparer);
        _duplicateRecords = new();
        _datasetFilePath = datasetFilePath;
        _duplicatesFilePath = duplicatesFilePath;
    }

    public void RemoveDuplicates()
    {
        using var reader = new StreamReader(_datasetFilePath);
        using var originalCsv = new CsvReader(reader,
            new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            });
        var records = originalCsv.GetRecords<TripEntity>();

        foreach (var record in records)
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

        WriteToCsv(_duplicateRecords, _duplicatesFilePath);
        WriteToCsv(_uniqueRecords, _datasetFilePath);
    }

    private static void WriteToCsv<T>(IEnumerable<T> records, string filePath)
    {
        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
        csv.WriteRecords(records);
    }
}
