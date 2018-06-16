using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TUI.Flights.Common.Args.Airport;
using TUI.Flights.Common.Args.Common;
using TUI.Flights.Common.Dtos.Airport;

namespace TUI.Flights.Core.Services.AirportServices
{
    public interface IAirportServices
    {
        Task<IEnumerable<AirportDto>> GetAllAirports(PaginationArgs pagination);
        Task<IEnumerable<AirportDto>> SearchAirports(SearchAirportsArgs searchArgs);
    }
}
