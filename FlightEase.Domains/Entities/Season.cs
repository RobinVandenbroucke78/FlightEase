using System;
using System.Collections.Generic;

namespace FlightEase.Domains.Entities;

public partial class Season
{
    public int SeasonId { get; set; }

    public string Name { get; set; } = null!;

    public double Price { get; set; }

    public DateOnly? BeginDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();
}
