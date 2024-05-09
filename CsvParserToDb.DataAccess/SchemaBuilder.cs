using System.Data.SqlClient;


namespace CsvParserToDb.DataAccess;

public class SchemaBuilder
{
    private readonly string _connectionString;
    private readonly string _scriptsPath;

    public SchemaBuilder()
    {
        _connectionString = @"Server=(LocalDB)\MSSQLLocalDB;Trusted_Connection=True;";
        _scriptsPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "CsvParserToDb.DataAccess", "Scripts");
    }

    public SchemaBuilder(string connectionString, string scriptsPath)
    {
        _connectionString = connectionString;
        _scriptsPath = scriptsPath;
    }

    public void CreateDatabaseSchema()
    {
        var sqlQuery = File.ReadAllText(Path.Combine(_scriptsPath, "CreateTestTaskDb.sql"));
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = new SqlCommand(sqlQuery, connection);
        command.ExecuteNonQuery();
        Console.WriteLine("Schema created successfully.");
    }

    public void CreateTripsTable()
    {
        var sqlQuery = File.ReadAllText(Path.Combine(_scriptsPath, "CreateTripsTable.sql"));
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = new SqlCommand(sqlQuery, connection);
        command.ExecuteNonQuery();
        Console.WriteLine("Table created successfully.");
    }
}
