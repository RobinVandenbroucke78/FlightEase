using FlightEase.Domains;

namespace FlightEase.ViewModels
{
    public class HotelVM
    {
        public string? Name { get; set; }
        public GeoCodeVM GeoCodeVM { get; set; } = new GeoCodeVM();
        public AddressVM AddressVM { get; set; } = new AddressVM(); // Direct instantiëren  
        public DateTime? LastUpdate { get; set; }  // Datumveld voor lastUpdate
    }

    public class GeoCodeVM
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }

    public class AddressVM
    {
        public string? CountryCode { get; set; }
    }
}
