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

            //Ticket
            CreateMap<Ticket, TicketVM>()
                .ForMember(dest => dest.ClassTypes, opt => opt.MapFrom(src => src.ClassType.ClassName))
                .ForMember(dest => dest.Meals, opt => opt.MapFrom(src => src.Meal.MealName))
                .ForMember(dest => dest.Seats, opt => opt.MapFrom(src => src.Seat.SeatNumber))
                .ForMember(dest => dest.Seasons, opt => opt.MapFrom(src => src.Season.Name));
        }
    }
}
