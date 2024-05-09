using CsvParserToDb.Domain.Shared.Entities;

namespace CsvParserToDb.DataAccess.Repositories;

public interface ITripsRepository
{
    Task<int> BulkInsertFromCsv(string csvFilePath);
    Task<int> FindPuLocationIdWithHighestAvgTipAmount();
    Task<IEnumerable<double>> GetTopFaresByTripDistance(int topCount);
    Task<IEnumerable<double>> GetTopFaresByTimeTravelling(int topCount);
    Task<IEnumerable<TripEntity>> Search(int puLocationId);
}
