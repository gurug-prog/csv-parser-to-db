USE [TestTaskDb]

CREATE TABLE [dbo].[Trips] (
    Id INT IDENTITY(1,1) NOT NULL,
    tpep_pickup_datetime DATETIME2 NOT NULL,
    tpep_dropoff_datetime DATETIME2 NOT NULL,
    passenger_count INT NOT NULL,
    trip_distance FLOAT NOT NULL,
    store_and_fwd_flag NVARCHAR(3) NOT NULL,
    PULocationID INT NOT NULL,
    DOLocationID INT NOT NULL,
    fare_amount FLOAT NOT NULL,
    tip_amount FLOAT NOT NULL,
    CONSTRAINT [PK_Trips] PRIMARY KEY CLUSTERED ([Id] ASC)
)
