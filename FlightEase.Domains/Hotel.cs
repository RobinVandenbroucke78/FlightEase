using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightEase.Domains
{
    public class HotelApiResponse
    {
        public List<Hotel>? Data { get; set; }
        public MethodAccessException Meta { get; set; }
    }

    public class Hotel
    {
        public string? ChainCode { get; set; }
        public string? IataCode { get; set; }
        public long DupeId { get; set; }
        public string? Name { get; set; }
        public string? HotelId { get; set; }
        public GeoCode? GeoCode { get; set; }
        public Address? Address { get; set; }
        public DateTime LastUpdate { get; set; }
    }

    public class GeoCode
    {
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
    }

    public class Address
    {
        public string? CountryCode { get; set; }
    }

    public class Meta
    {
        public int Count { get; set; }
        public Links Links { get; set; }
    }

    public class Links
    {
        public string Self { get; set; }
    }
}
