using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TUI.Flights.Common.Dtos.Aircraft;
using TUI.Flights.Common.Dtos.Airport;
using TUI.Flights.Common.Dtos.Flight;
using TUI.Flights.Common.Entities;
using TUI.Flights.Core.Helpers;

namespace TUI.Flights.Core.Mappers
{
    public class AutoMapperConfigurations : Profile
    {
        public AutoMapperConfigurations()
        {
            CreateMap<Flight, FlightDto>().ReverseMap();

            CreateMap<Airport, AirportDto>()
                .ForMember(dest => dest.Id, options => options.MapFrom(source => source.Id))
                .ForMember(dest => dest.Value, options => options.MapFrom(source => source.Name));

            CreateMap<Aircraft, AircraftDto>()
            .ForMember(dest => dest.Id, options => options.MapFrom(source => source.Id))
            .ForMember(dest => dest.Value, options => options.MapFrom(source => source.Code));
        }
    }
}
