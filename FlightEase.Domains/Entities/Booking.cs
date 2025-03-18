using System;
using System.Collections.Generic;

namespace FlightEase.Domains.Entities;

public partial class Booking
{
    public int BookingId { get; set; }

    public string UserId { get; set; } = null!;

    public int TicketId { get; set; }

    public DateTime BookingDate { get; set; }

    public string BookingName { get; set; } = null!;

    public double Price { get; set; }

    public virtual Ticket Ticket { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
