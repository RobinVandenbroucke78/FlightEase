using FlightEase.Domains.Entities;

namespace FlightEase.ViewModels
{
    public class FlightVM
    {
        public int FlightId { get; set; }
        public string? GateName { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string? FromAirportName { get; set; }
        public string? ToAirportName { get; set; }
        public string? FirstTransferName { get; set; }
        public string? SecondTransferName { get; set; }
        public double Price { get; set; }
        public int AvailableSeats { get; set; }
    }
}   
