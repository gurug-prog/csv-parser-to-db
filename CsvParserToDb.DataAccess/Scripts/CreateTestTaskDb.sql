USE [master]

CREATE DATABASE [TestTaskDb]
ON PRIMARY (
    NAME = N'TestTaskDb',
    FILENAME = N'C:\TestTaskData\TestTaskDb.mdf',
    SIZE = 8192KB,
    MAXSIZE = UNLIMITED,
    FILEGROWTH = 65536KB
)
LOG ON (
    NAME = N'TestTaskDb_log',
    FILENAME = N'C:\TestTaskData\TaxiData_log.ldf',
    SIZE = 8192KB,
    MAXSIZE = 2048GB,
    FILEGROWTH = 65536KB
)
