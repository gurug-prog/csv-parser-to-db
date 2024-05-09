using CsvParserToDb.DataAccess.Repositories;

namespace CsvParserToDb.DataAccess.Factories;

public class DefaultRepositoryFactory : IRepositoryFactory
{
    private readonly string _connectionString;
    public DefaultRepositoryFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public ITripsRepository CreateTripsRepository()
    {
        return new TripsRepository(_connectionString);
    }
}
