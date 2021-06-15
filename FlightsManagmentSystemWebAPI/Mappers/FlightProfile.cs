using AutoMapper;
using Domain.Entities;
using FlightsManagmentSystemWebAPI.CountriesManagerService;
using FlightsManagmentSystemWebAPI.Dtos;

namespace FlightsManagmentSystemWebAPI.Mappers
{
    public class FlightProfile : Profile
    {
        public FlightProfile(ICountriesManager countriesManager)
        {
            CreateMap<CreateFlightDTO, Flight>();
            CreateMap<UpdateFlightDTO, Flight>();
            CreateMap<Flight, FlightDetailsDTO>()
                .ForMember(dest => dest.OriginCountryName,
                            opt => opt.MapFrom(src => countriesManager.GetCountryName(src.OriginCountryId)))
                .ForMember(dest => dest.DestinationCountryName,
                            opt => opt.MapFrom(src => countriesManager.GetCountryName(src.DestinationCountryId)));
        }
    }
}
