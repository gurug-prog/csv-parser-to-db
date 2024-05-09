using CsvParserToDb.DataAccess.Repositories;

namespace CsvParserToDb.DataAccess.Factories;

public class RepositoryFactory : IRepositoryFactory
{
    public ITripsRepository CreateTripsRepository()
    {
        return new TripsRepository();
    }

    //public ITripsRepository CreateTripsRepository()
    //{
    //    return new TripsRepository();
    //}
}
