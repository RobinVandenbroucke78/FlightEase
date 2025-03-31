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
                .ForMember(dest => dest.FromAirport, opt => opt.MapFrom(src => src.FromAirport.City))
                .ForMember(dest => dest.ToAirport, opt => opt.MapFrom(src => src.ToAirport.City))
                .ForMember(dest => dest.Transfer, opt => opt.MapFrom(src => src.Transfer.FirstAirport.City));
        }
    }
}
