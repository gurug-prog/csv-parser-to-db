using CsvParserToDb.Domain.Services;

namespace CsvParserToDb.Domain.Factories;

public interface IDomainServiceFactory
{
    CsvParserDomainService CreateCsvParser();
}
