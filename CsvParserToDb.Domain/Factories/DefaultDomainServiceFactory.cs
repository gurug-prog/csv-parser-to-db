using CsvParserToDb.Domain.Services;
using CsvParserToDb.Domain.Shared.Entities;

namespace CsvParserToDb.Domain.Factories;

public class DefaultDomainServiceFactory : IDomainServiceFactory
{
    private readonly string _datasetFilePath;
    private readonly string _duplicatesFilePath;

    public DefaultDomainServiceFactory(
        string datasetFilePath,
        string duplicatesFilePath)
    {
        _datasetFilePath = datasetFilePath;
        _duplicatesFilePath = duplicatesFilePath;
    }

    public CsvParserDomainService CreateCsvParser()
    {
        var comparer = new TripDuplicateEqualityComparer();
        return new CsvParserDomainService(comparer, _datasetFilePath, _duplicatesFilePath);
    }
}
