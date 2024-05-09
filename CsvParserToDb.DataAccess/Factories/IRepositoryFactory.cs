using CsvParserToDb.DataAccess.Repositories;

namespace CsvParserToDb.DataAccess.Factories;

public interface IRepositoryFactory
{
    ITripsRepository CreateTripsRepository();
}
