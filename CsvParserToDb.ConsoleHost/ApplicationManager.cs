using CsvParserToDb.DataAccess.Factories;
using CsvParserToDb.DataAccess.Repositories;
using CsvParserToDb.Domain.Factories;
using CsvParserToDb.Domain.Services;

namespace CsvParserToDb.ConsoleHost;

public class ApplicationManager
{
    private readonly ITripsRepository _tripsRepository;
    private readonly CsvParserDomainService _csvParserService;

    public ApplicationManager(
        IRepositoryFactory repositoryFactory,
        IDomainServiceFactory domainServiceFactory)
    {
        _tripsRepository = repositoryFactory.CreateTripsRepository();
        _csvParserService = domainServiceFactory.CreateCsvParser();
    }

    public async Task<int> BulkInsertData(string csvFilePath)
    {
        return await _tripsRepository.BulkInsertFromCsv(csvFilePath);
    }

    public async Task<int> RemoveDuplicates()
    {
        return await _csvParserService.RemoveDuplicates();
    }

    //public async Task<int> FindPuLocationIdWithHighestAvgTipAmount()
    //{

    //}

    //public async Task<IEnumerable<double>> GetTopFaresByTripDistance(int topCount)
    //{

    //}

    //public async Task<IEnumerable<double>> GetTopFaresByTimeTravelling(int topCount)
    //{

    //}

    //public async Task<IEnumerable<TripEntity>> Search(int puLocationId)
    //{

    //}
}
