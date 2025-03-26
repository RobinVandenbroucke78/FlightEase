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

    public int MealId { get; set; }

    public int ClassTypeId { get; set; }

    public int SeasonId { get; set; }

    public int SeatId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ClassType ClassType { get; set; } = null!;

    public virtual Flight Flight { get; set; } = null!;

    public virtual Meal Meal { get; set; } = null!;

    public virtual Season Season { get; set; } = null!;

    public virtual Seat Seat { get; set; } = null!;
}
