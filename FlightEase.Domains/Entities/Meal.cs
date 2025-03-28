﻿using System;
using System.Collections.Generic;

namespace FlightEase.Domains.Entities;

public partial class Meal
{
    public int MealId { get; set; }

    public string MealName { get; set; } = null!;

    public string MealDescription { get; set; } = null!;

    public double Price { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
