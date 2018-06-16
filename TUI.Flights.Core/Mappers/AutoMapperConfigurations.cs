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
        //private readonly IDictionaryHelper _dictionaryHelper;

        //public AutoMapperConfigurations(IDictionaryHelper dictionaryHelper)
        //{
        //    _dictionaryHelper = dictionaryHelper ?? throw new ArgumentNullException("dictionaryHelper");
        //}

        public AutoMapperConfigurations()
        {
            CreateMap<Flight, FlightDto>().ReverseMap();
                //.ForMember(dest => dest.AirportDepartureId, options => options.MapFrom(source => source.AirportDepartureId))
                //.ForMember(dest => dest.AirportDestinationId, options => options.MapFrom(source => source.AirportDestinationId))
                //.ForMember(dest => dest.AirportDeparture, options => options.MapFrom(source => _dictionaryHelper.GetAirportName(source.AirportDepartureId)))
                //.ForMember(dest => dest.AirportDestination, options => options.MapFrom(source => _dictionaryHelper.GetAirportName(source.AirportDestinationId)))
                //.ForMember(dest => dest.Aircraft, options => options.MapFrom(source => _dictionaryHelper.GetAircraftName(source.AircraftId)))
                //;

            CreateMap<Airport, AirportDto>();

            CreateMap<Aircraft, AircraftDto>();
        }
    }
}
