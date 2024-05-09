using System.Data.SqlClient;
using System.Reflection;


namespace CsvParserToDb.DataAccess;

public class SchemaBuilder
{
    public const string CONNECTION_STRING =
        @"Server=(LocalDB)\MSSQLLocalDB;Trusted_Connection=True;";

    //private static readonly string SCRIPTS_PATH =
    //    Path.Combine(Directory.GetCurrentDirectory(), "..", "CsvParserToDb.DataAccess", "Scripts");

    private static readonly string SCRIPTS_PATH =
        Path.Combine(Assembly.GetExecutingAssembly().Location);

    public void CreateDatabaseSchema()
    {
        var sqlQuery = File.ReadAllText(Path.Combine(SCRIPTS_PATH, "CreateTestTaskDb.sql"));
        using var connection = new SqlConnection(CONNECTION_STRING);
        connection.Open();

        using var command = new SqlCommand(sqlQuery, connection);
        command.ExecuteNonQuery();
        Console.WriteLine("Schema created successfully.");
    }

    public void CreateTripsTable()
    {
        var sqlQuery = File.ReadAllText(Path.Combine(SCRIPTS_PATH, "CreateTripsTable.sql"));
        using var connection = new SqlConnection(CONNECTION_STRING);
        connection.Open();

        using var command = new SqlCommand(sqlQuery, connection);
        command.ExecuteNonQuery();
        Console.WriteLine("Table created successfully.");
    }
}
