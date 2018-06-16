using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TUI.Flights.Common.Args.Airport;
using TUI.Flights.Common.Args.Common;
using TUI.Flights.Core.Services.AirportServices;

namespace TUI.Flights.Web.Controllers
{
    public class AirportsController : Controller
    {
        private readonly IAirportServices _airportServices;

        public AirportsController(IAirportServices airportServices)
        {
            _airportServices = airportServices ?? throw new ArgumentNullException("airportServices");
        }


        public async Task<JsonResult> SearchAirports(string term, int pageSize = 10, int startIndex = 0)
        {
            var airports = await _airportServices.SearchAirports(new SearchAirportsArgs
            {
                Pagination = new PaginationArgs
                {
                    PageSize = pageSize,
                    StartIndex = startIndex
                },
                Filters = new AirportSearchFiltersArgs
                {
                    AirportName = term
                }
            });

            return Json(airports);
        }
    }
}