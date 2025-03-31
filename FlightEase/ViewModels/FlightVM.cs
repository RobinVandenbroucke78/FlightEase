using FlightEase.Domains.Entities;

namespace FlightEase.ViewModels
{
    public class FlightVM
    {
        public int FlightId { get; set; }

        public string GateName { get; set; } = null!;

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public string? FromAirport { get; set; }

        public string? ToAirport { get; set; }

        public string? Transfer { get; set; }

        public double Price { get; set; }

        public int AvailableSeats { get; set; }

    }
}
