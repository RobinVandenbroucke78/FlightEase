using System;
using System.Collections.Generic;

namespace FlightEase.Domains.Entities;

public partial class Seat
{
    public int SeatId { get; set; }

    public string SeatNumber { get; set; } = null!;

    public double Price { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
