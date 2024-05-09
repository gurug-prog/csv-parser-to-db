using CsvParserToDb.DataAccess.Factories;
using CsvParserToDb.DataAccess.Repositories;


namespace CsvParserToDb.Domain.Services;

public class TripDomainService
{
    private readonly ITripsRepository _tripsRepository;

    public TripDomainService(IRepositoryFactory repositoryFactory)
    {
        _tripsRepository = repositoryFactory.CreateTripsRepository();
    }


}
