using FlightEase.Domains.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FlightEase.ViewModels
{
    public class ShoppingCartVM
    {
        public List<TicketVM> Tickets { get; set; } = new List<TicketVM>();
        public IEnumerable<SelectListItem>? ClassTypes { get; set; }
        public IEnumerable<SelectListItem>? Meals { get; set; }
        public IEnumerable<SelectListItem>? Seats { get; set; }
        public IEnumerable<SelectListItem>? Seasons { get; set; }

    }

    public class TicketVM
    {
        public int TicketId { get; set; }

        public int FlightId { get; set; }

        public double Price { get; set; }

        public int? Seats { get; set; }

        public int? Meals { get; set; }

        public int? ClassTypes { get; set; }

        public int? Seasons { get; set; }

        public System.DateTime IssueDate { get; set; }

        public int Count { get; set; }

        //Flight Details
        public string? FromAirport { get; set; }
        public string? ToAirport { get; set; }
    }
}
