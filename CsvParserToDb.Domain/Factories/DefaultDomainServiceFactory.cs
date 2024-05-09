using CsvParserToDb.Domain.Services;
using CsvParserToDb.Domain.Shared.Entities;

namespace CsvParserToDb.Domain.Factories;

public class DefaultDomainServiceFactory : IDomainServiceFactory
{
    private readonly string _datasetFilePath;

    public DefaultDomainServiceFactory(string datasetFilePath)
    {
        _datasetFilePath = datasetFilePath;
    }

    public CsvParserDomainService CreateCsvParser()
    {
        var comparer = new TripDuplicateEqualityComparer();
        return new CsvParserDomainService(comparer, _datasetFilePath);
    }
}
