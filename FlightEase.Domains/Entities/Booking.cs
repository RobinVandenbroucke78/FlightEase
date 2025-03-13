using System;
using System.Collections.Generic;

namespace FlightEase.Domains.Entities;

public partial class Booking
{
    public int BookingId { get; set; }

    public int FlightId { get; set; }

    public DateTime BookingDate { get; set; }

    public string BookingName { get; set; } = null!;

    public double Price { get; set; }

    public virtual Flight Flight { get; set; } = null!;
}
