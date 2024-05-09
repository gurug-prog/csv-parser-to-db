using CsvParserToDb.Domain.Shared.Entities;
using System.Data.SqlClient;

namespace CsvParserToDb.DataAccess.Repositories;

public class TripsRepository : ITripsRepository
{
    public readonly string _connectionString;

    public TripsRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<int> BulkInsertFromCsv(string csvFilePath)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var bulkInsertCommand = $@"
            BULK INSERT Trips
            FROM '{csvFilePath}'
            WITH (
                FIRSTROW = 2,
                FIELDTERMINATOR = ',',
                ROWTERMINATOR = '\n',
                TABLOCK,
                KEEPNULLS
            )";

        using var command = new SqlCommand(bulkInsertCommand, connection);
        return await command.ExecuteNonQueryAsync();
    }

    public async Task<IEnumerable<TripEntity>> Search(int puLocationId)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var query = @"
            SELECT *
            FROM Trips
            WHERE PULocationID = @PULocationID";

        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@PULocationID", puLocationId);
        
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            Console.WriteLine($"Trip ID: {reader["Id"]}, Distance: {reader["trip_distance"]}");
        }
    }
}
