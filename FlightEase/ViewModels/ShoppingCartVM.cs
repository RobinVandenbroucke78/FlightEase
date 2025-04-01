using FlightEase.Domains.Entities;
using System.Collections.Generic;

namespace FlightEase.ViewModels
{
    public class ShoppingCartVM
    {
        public List<TicketVM> Tickets { get; set; } = new List<TicketVM>();
        public List<ClassType> ClassTypes { get; set; } = new List<ClassType>();
        public List<Meal> Meals { get; set; } = new List<Meal>();
        public List<Seat> Seats { get; set; } = new List<Seat>();
        public List<Season> Seasons { get; set; } = new List<Season>();
    }

    public class TicketVM
    {
        public int TicketId { get; set; }

        public int FlightId { get; set; }

        public double Price { get; set; }

        public List<Seat>? Seats { get; set; }

        public List<Meal>? Meals { get; set; }

        public List<ClassType>? ClassTypes { get; set; }

        public List<Season>? Seasons { get; set; }

        public System.DateTime IssueDate { get; set; }

        public int Count { get; set; }
    }
}
