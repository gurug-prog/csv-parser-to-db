# CsvParserToDb

To successfully run application you need to run SQL scripts that creates Test DB and Trips table manually on you MS SQL Server (the scripts are located in [CsvParserToDb.DataAccess/Scripts](./CsvParserToDb.DataAccess/Scripts/)).

Or you can simply do this work using parametrized startup:

```
dotnet run --initdb
```

## Results

- Implemented efficient parsing and duplications removing in Domain layer
- DataAccess layer contains logic with DB access methods and raw SQL queries
- The repositories are protected from SQL injections, cause they don't use raw strings in their queries

## Comments

### Handling big CSV Files (over 10 GB size)
You may consider these optimizations:
- Split the file into smaller chunks for processing.
- Implement more low-level parsing of data to avoid loading and processing waste of data and just skip it.
- Use a database engine with support for large-scale data processing, ensuring sufficient resources (CPU, memory, disk I/O).
- Developing parallel algorithm for parsing and data access and use it with parallel processing or cloud-based distributed computing for faster operations.
