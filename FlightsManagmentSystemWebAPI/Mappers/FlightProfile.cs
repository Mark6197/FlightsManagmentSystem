using AutoMapper;
using Domain.Entities;
using FlightsManagmentSystemWebAPI.Dtos;

namespace FlightsManagmentSystemWebAPI.Mappers
{
    public class FlightProfile : Profile
    {
        public FlightProfile()
        {
            CreateMap<CreateFlightDTO, Flight>();
            CreateMap<UpdateFlightDTO, Flight>();
            CreateMap<Flight, FlightDetailsDTO>();
        }
    }
}
