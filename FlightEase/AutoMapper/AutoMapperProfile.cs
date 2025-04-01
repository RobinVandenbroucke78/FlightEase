using AutoMapper;
using FlightEase.Domains.Entities;
using FlightEase.ViewModels;

namespace FlightEase.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Flight, FlightVM>()
                .ForMember(dest => dest.FromAirportName, opt => opt.MapFrom(src => src.FromAirport.City.CityName))
                .ForMember(dest => dest.ToAirportName, opt => opt.MapFrom(src => src.ToAirport.City.CityName))
                .ForMember(dest => dest.FirstTransferName, opt => opt.MapFrom(src => src.Transfer.FirstAirport.City.CityName))
                .ForMember(dest => dest.SecondTransferName, opt => opt.MapFrom(src => src.Transfer.SecondAirport.City.CityName));
        }
    }
}
