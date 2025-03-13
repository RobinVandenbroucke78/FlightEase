using System;
using System.Collections.Generic;

namespace FlightEase.Domains.Entities;

public partial class Flight
{
    public int FlightId { get; set; }

    public string? Name { get; set; }

    public string? FirstName { get; set; }

    public double Price { get; set; }

    public string SeatName { get; set; } = null!;

    public string GateName { get; set; } = null!;

    public DateTime DepartureTime { get; set; }

    public DateTime ArrivalTime { get; set; }

    public int FromAirportId { get; set; }

    public int ToAirportId { get; set; }

    public int TransferId { get; set; }

    public int MealId { get; set; }

    public int ClassTypeId { get; set; }

    public int SeasonId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ClassType ClassType { get; set; } = null!;

    public virtual Airport FromAirport { get; set; } = null!;

    public virtual Meal Meal { get; set; } = null!;

    public virtual Season Season { get; set; } = null!;

    public virtual Airport ToAirport { get; set; } = null!;

    public virtual Transfer Transfer { get; set; } = null!;
}
