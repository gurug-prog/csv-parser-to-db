using CsvParserToDb.DataAccess;
using CsvParserToDb.DataAccess.Factories;
using CsvParserToDb.Domain.Factories;

namespace CsvParserToDb.ConsoleHost;

public class Program
{
    private static readonly string baseDataDir =
        Path.Combine(Directory.GetCurrentDirectory(), "..", "Data");
    
    private static readonly string inputFilePath =
        Path.Combine(baseDataDir, "sample-cab-data.csv");
    
    private static readonly string duplicatesFilePath =
        Path.Combine(baseDataDir, "duplicates.csv");

    private static readonly string dataSchemaPath =
        Path.Combine(baseDataDir, "DataSchema.xml");

    private static readonly string dbConnectionString =
        @"Server=(LocalDB)\MSSQLLocalDB;Database=TestTaskDb;Trusted_Connection=True;";

    public static async Task Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "--initdb")
        {
            var schemaBuilder = new SchemaBuilder();
            schemaBuilder.CreateDatabaseSchema();
            schemaBuilder.CreateTripsTable();
        }

        var repositoryFactory = new DefaultRepositoryFactory(dbConnectionString);
        var domainServiceFactory = new DefaultDomainServiceFactory(inputFilePath);
        var appManager = new ApplicationManager(repositoryFactory, domainServiceFactory);

        //var duplicatesCount = await appManager.RemoveDuplicates(duplicatesFilePath);
        var insertedRowsCount = await appManager.BulkInsertData(inputFilePath, dataSchemaPath);

        await Console.Out.WriteLineAsync($"Successfully inserted {insertedRowsCount} processed rows.");
    }
}
