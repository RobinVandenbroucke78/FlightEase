using System;
using System.Collections.Generic;

namespace FlightEase.Domains.Entities;

public partial class Flight
{
    public int FlightId { get; set; }

    public string GateName { get; set; } = null!;

    public DateTime DepartureTime { get; set; }

    public DateTime ArrivalTime { get; set; }

    public int FromAirportId { get; set; }

    public int ToAirportId { get; set; }

    public int TransferId { get; set; }

    public double Price { get; set; }

    public int AvailableSeats { get; set; }

    public virtual Airport FromAirport { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual Airport ToAirport { get; set; } = null!;

    public virtual Transfer Transfer { get; set; } = null!;
}
