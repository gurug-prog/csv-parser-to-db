using CsvParserToDb.Domain.Shared.Entities;

namespace CsvParserToDb.DataAccess.Repositories;

public interface ITripsRepository
{
    Task<int> BulkInsertFromCsv(string csvFilePath);
    Task<IEnumerable<TripEntity>> Search(int puLocationId);

}
