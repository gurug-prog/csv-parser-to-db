using CsvParserToDb.DataAccess.Factories;
using CsvParserToDb.DataAccess.Repositories;
using CsvParserToDb.Domain.Factories;
using CsvParserToDb.Domain.Services;
using CsvParserToDb.Domain.Shared.Entities;

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

    public async Task<int> BulkInsertData(string csvFilePath, string formatFile)
    {
        return await _tripsRepository.BulkInsertFromCsv(csvFilePath, formatFile);
    }

    public async Task<int> RemoveDuplicates(string outputDuplicates)
    {
        return await _csvParserService.RemoveDuplicates(outputDuplicates);
    }

    public async Task<int> FindPuLocationIdWithHighestAvgTipAmount()
    {
        return await _tripsRepository.FindPuLocationIdWithHighestAvgTipAmount();
    }

    public async Task<IEnumerable<double>> GetTopFaresByTripDistance(int topCount)
    {
        return await _tripsRepository.GetTopFaresByTripDistance(topCount);
    }

    public async Task<IEnumerable<double>> GetTopFaresByTimeTravelling(int topCount)
    {
        return await _tripsRepository.GetTopFaresByTimeTravelling(topCount);
    }

    public async Task<IEnumerable<TripEntity>> Search(int puLocationId)
    {
        return await _tripsRepository.Search(puLocationId);
    }
}
