using Domain.Interfaces;
using Newtonsoft.Json;
using System;

namespace Domain.Entities
{
    public class Flight : IPoco
    {
        public long Id { get; set; }
        public AirlineCompany AirlineCompany { get; set; }
        public int OriginCountryId { get; set; }
        public int DestinationCountryId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime LandingTime { get; set; }
        public int RemainingTickets { get; set; }

        public Flight()
        {

        }

        public Flight(AirlineCompany airlineCompany, int originCountryId, int destinationCountryId, DateTime departureTime, DateTime landingTime, int remainingTickets, long id = 0)
        {
            AirlineCompany = airlineCompany;
            OriginCountryId = originCountryId;
            DestinationCountryId = destinationCountryId;
            DepartureTime = departureTime;
            LandingTime = landingTime;
            RemainingTickets = remainingTickets;
            Id = id;
        }

        public static bool operator ==(Flight flight1, Flight flight2)
        {
            if (ReferenceEquals(flight1, null) && ReferenceEquals(flight2, null))
                return true;
            if (ReferenceEquals(flight1, null) || ReferenceEquals(flight2, null))
                return false;

            return flight1.Id == flight2.Id;
        }
        public static bool operator !=(Flight flight1, Flight flight2)
        {
            return !(flight1 == flight2);
        }
        public override bool Equals(object obj)
        {
            Flight flight = obj as Flight;
            return this == flight;
        }

        public override int GetHashCode()
        {
            return (int)this.Id;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
