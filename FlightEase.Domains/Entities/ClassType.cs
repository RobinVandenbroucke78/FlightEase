using System;
using System.Collections.Generic;

namespace FlightEase.Domains.Entities;

public partial class ClassType
{
    public int ClassTypeId { get; set; }

    public string ClassName { get; set; } = null!;

    public int Seats { get; set; }

    public double PricePerSeats { get; set; }

    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();
}
