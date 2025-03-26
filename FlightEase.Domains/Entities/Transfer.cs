using System;
using System.Collections.Generic;

namespace FlightEase.Domains.Entities;

public partial class Transfer
{
    public int TransferId { get; set; }

    public int FirstAirportId { get; set; }

    public int? SecondAirportId { get; set; }

    public virtual Airport FirstAirport { get; set; } = null!;

    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();

    public virtual Airport? SecondAirport { get; set; }
}
