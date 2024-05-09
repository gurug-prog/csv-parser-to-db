using CsvParserToDb.DataAccess.Repositories;

namespace CsvParserToDb.DataAccess.Factories;

public class DefaultRepositoryFactory : IRepositoryFactory
{
    public ITripsRepository CreateTripsRepository()
    {
        string connectionString = @"Server=(LocalDB)\MSSQLLocalDB;Database=TestTaskDb;Trusted_Connection=True;";
        return new TripsRepository(connectionString);
    }
}
