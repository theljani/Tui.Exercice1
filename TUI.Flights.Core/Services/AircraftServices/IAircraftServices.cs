using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TUI.Flights.Common.Args.Aircraft;
using TUI.Flights.Common.Args.Airport;
using TUI.Flights.Common.Args.Common;
using TUI.Flights.Common.Dtos.Aircraft;
using TUI.Flights.Common.Dtos.Airport;

namespace TUI.Flights.Core.Services.AirportServices
{
    public interface IAircraftServices
    {
        Task<IEnumerable<AircraftDto>> SearchAircrafts(SearchAircraftsArgs searchArgs);
    }
}
