using System;
using System.Collections.Generic;

namespace FlightEase.Domains.Entities;

public partial class Ticket
{
    public int TicketId { get; set; }

    public int FlightId { get; set; }

    public double Price { get; set; }

    public string SeatNumber { get; set; } = null!;

    public DateOnly IssueDate { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Flight Flight { get; set; } = null!;
}
