using System;
using System.Collections.Generic;

namespace FlightEase.Domains.Entities;

public partial class Transfer
{
    public int TransferId { get; set; }

    public int AirportId { get; set; }

    public virtual Airport Airport { get; set; } = null!;

    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();
}
