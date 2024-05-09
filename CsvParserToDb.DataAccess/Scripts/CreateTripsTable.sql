USE [TestTaskDb]

CREATE TABLE [dbo].[Trips] (
    Id INT IDENTITY(1,1) NOT NULL,
    tpep_pickup_datetime DATETIME2 NOT NULL,
    tpep_dropoff_datetime DATETIME2 NOT NULL,
    passenger_count INT NULL,
    trip_distance FLOAT NULL,
    store_and_fwd_flag NVARCHAR(3) NULL,
    PULocationID INT NULL,
    DOLocationID INT NULL,
    fare_amount FLOAT NULL,
    tip_amount FLOAT NULL,
    CONSTRAINT [PK_Trips] PRIMARY KEY CLUSTERED ([Id] ASC)
)
