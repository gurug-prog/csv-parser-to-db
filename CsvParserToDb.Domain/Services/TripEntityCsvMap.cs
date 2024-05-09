using CsvHelper.Configuration;
using CsvParserToDb.Domain.Shared.Entities;

namespace CsvParserToDb.Domain.Services
{
    public class TripEntityCsvMap : ClassMap<TripEntity>
    {
        public TripEntityCsvMap()
        {
            Map(t => t.TpepPickupDatetime).Name("tpep_pickup_datetime");
            Map(t => t.TpepDropoffDatetime).Name("tpep_dropoff_datetime");
            Map(t => t.PassengerCount).Name("passenger_count");
            Map(t => t.TripDistance).Name("trip_distance");
            Map(t => t.StoreAndFwdFlag).Name("store_and_fwd_flag");
            Map(t => t.PULocationID).Name("PULocationID");
            Map(t => t.DOLocationID).Name("DOLocationID");
            Map(t => t.FareAmount).Name("fare_amount");
            Map(t => t.TipAmount).Name("tip_amount");
        }
    }
}
