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

    public async Task<int> BulkInsertFromCsv(string csvFilePath, string formatFile)
    {
        int result = 0;
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var createViewNoPKQuery = @"
                CREATE VIEW Trips_NoPK_View AS
                    SELECT tpep_pickup_datetime,
                      tpep_dropoff_datetime,
                      passenger_count,
                      trip_distance,
                      store_and_fwd_flag,
                      PULocationID,
                      DOLocationID,
                      fare_amount,
                      tip_amount
                    FROM Trips;";

            var bulkInsertQuery = $@"
                BULK INSERT Trips_NoPK_View
                FROM '{Path.GetFullPath(csvFilePath)}'
                WITH (
                    FORMATFILE = '{Path.GetFullPath(formatFile)}',
                    FIRSTROW = 2,
                    FIELDTERMINATOR = ',',
                    ROWTERMINATOR = '\n',
                    TABLOCK,
                    KEEPNULLS
                )";

            var dropViewNoPKQuery = @"DROP VIEW Trips_NoPK_View;";

            using var createViewCommand = new SqlCommand(createViewNoPKQuery, connection);
            await createViewCommand.ExecuteNonQueryAsync();

            using var bulkInsertCommand = new SqlCommand(bulkInsertQuery, connection);
            result = await bulkInsertCommand.ExecuteNonQueryAsync();

            using var dropViewCommand = new SqlCommand(dropViewNoPKQuery, connection);
            await dropViewCommand.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            // TODO: logging
            throw;
        }

        return result;
    }

    public async Task<int> FindPuLocationIdWithHighestAvgTipAmount()
    {
        int result = 0;
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                SELECT TOP (1) PULocationID, AVG(tip_amount) AS AverageTip
                FROM Trips
                GROUP BY PULocationID
                ORDER BY AverageTip DESC;";

            using var command = new SqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                result = reader.GetInt32(0);
            }
        }
        catch (Exception ex)
        {
            // TODO: logging
            throw;
        }

        return result;
    }

    public async Task<IEnumerable<double>> GetTopFaresByTripDistance(int topCount)
    {
        List<double> result = [];
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = $@"
                SELECT TOP ({topCount}) fare_amount
                FROM Trips
                ORDER BY trip_distance DESC;";

            using var command = new SqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var fareAmout = reader.GetDouble(0);
                result.Add(fareAmout);
            }
        }
        catch (Exception ex)
        {
            // TODO: logging
            throw;
        }

        return result;
    }

    public async Task<IEnumerable<double>> GetTopFaresByTimeTravelling(int topCount)
    {
        List<double> result = [];
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = $@"
                SELECT TOP ({topCount}) fare_amount,
                    DATEDIFF(MILLISECOND, tpep_pickup_datetime, tpep_dropoff_datetime) AS Duration
                FROM Trips
                ORDER BY Duration DESC";

            using var command = new SqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var fareAmout = reader.GetDouble(0);
                result.Add(fareAmout);
            }
        }
        catch (Exception ex)
        {
            // TODO: logging
            throw;
        }

        return result;
    }

    public async Task<IEnumerable<TripEntity>> Search(int puLocationId)
    {
        List<TripEntity> result = [];
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                SELECT *
                FROM Trips
                WHERE PULocationID = @PULocationID;";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PULocationID", puLocationId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var tripEntity = GetTripFromSqlReader(reader);
                result.Add(tripEntity);
            }
        }
        catch (Exception ex)
        {
            // TODO: logging
            throw;
        }

        return result;
    }

    private static TripEntity GetTripFromSqlReader(SqlDataReader reader)
    {
        return new TripEntity
        {
            Id = reader.GetInt32(0),
            TpepPickupDatetime = reader.GetDateTime(1),
            TpepDropoffDatetime = reader.GetDateTime(2),
            PassengerCount = reader.GetInt32(3),
            TripDistance = reader.GetDouble(4),
            StoreAndFwdFlag = reader.GetString(5),
            PULocationID = reader.GetInt32(6),
            DOLocationID = reader.GetInt32(7),
            FareAmount = reader.GetDouble(8),
            TipAmount = reader.GetDouble(9)
        };
    }
}
