using CsvParserToDb.DataAccess;
using CsvParserToDb.DataAccess.Factories;
using CsvParserToDb.Domain.Factories;

namespace CsvParserToDb.ConsoleHost;

public class Program
{
    private const string inputFilePath = "";
    private const string duplicatesFilePath = "";

    public static async Task Main(string[] args)
    {
        if (args[0] == "--initdb")
        {
            var schemaBuilder = new SchemaBuilder();
            schemaBuilder.CreateDatabaseSchema();
            schemaBuilder.CreateTripsTable();
        }

        var repositoryFactory = new DefaultRepositoryFactory();
        var domainServiceFactory = new DefaultDomainServiceFactory(inputFilePath, duplicatesFilePath);
        var applicationManager = new ApplicationManager(repositoryFactory, domainServiceFactory);

        var duplicatesCount = await applicationManager.RemoveDuplicates();
        var insertedRowsCount = await applicationManager.BulkInsertData(inputFilePath);

        Console.WriteLine($"Successfully inserted {insertedRowsCount} processed rows.");
    }
}
