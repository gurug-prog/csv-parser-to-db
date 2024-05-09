namespace CsvParserToDb.Domain.Entities;

public class TripDuplicateEqualityComparer : IEqualityComparer<TripEntity>
{
    public bool Equals(TripEntity? x, TripEntity? y)
    {
        if (x is null || y is null)
        {
            return false;
        }

        return x.TpepPickupDatetime == y.TpepPickupDatetime
            && x.TpepDropoffDatetime == y.TpepDropoffDatetime
            && x.PassengerCount == y.PassengerCount;
    }

    public int GetHashCode(TripEntity obj)
    {
        return HashCode.Combine(
            obj.TpepPickupDatetime,
            obj.TpepDropoffDatetime,
            obj.PassengerCount);
    }
}
