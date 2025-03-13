using System;
using System.Collections.Generic;

namespace FlightEase.Domains.Entities;

public partial class City
{
    public int CityId { get; set; }

    public string CityName { get; set; } = null!;

    public virtual ICollection<Airport> Airports { get; set; } = new List<Airport>();
}
