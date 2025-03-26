using System;
using System.Collections.Generic;

namespace FlightEase.Domains.Entities;

public partial class Airport
{
    public int AirportId { get; set; }

    public int CityId { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual ICollection<Flight> FlightFromAirports { get; set; } = new List<Flight>();

    public virtual ICollection<Flight> FlightToAirports { get; set; } = new List<Flight>();

    public virtual ICollection<Transfer> TransferFirstAirports { get; set; } = new List<Transfer>();

    public virtual ICollection<Transfer> TransferSecondAirports { get; set; } = new List<Transfer>();
}
