using CsvParserToDb.DataAccess;
using CsvParserToDb.DataAccess.Factories;

namespace CsvParserToDb.ConsoleHost;

public class Program
{
    public static void Main(string args[])
    {
        if (args[0] == "--initdb")
        {
            var schemaBuilder = new SchemaBuilder();
            schemaBuilder.CreateDatabaseSchema();
            schemaBuilder.CreateTripsTable();
        }

        //var repositoryFactory = new RepositoryFactory();
        //var applicationManager = new ApplicationManager(repositoryFactory);


    }
}
