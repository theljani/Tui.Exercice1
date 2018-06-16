using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TUI.Flights.Common.Args.Aircraft;
using TUI.Flights.Common.Args.Common;
using TUI.Flights.Core.Services.AirportServices;

namespace TUI.Flights.Web.Controllers
{
    public class AircraftsController : Controller
    {
        private readonly IAircraftServices _aircraftServices;

        public AircraftsController(IAircraftServices aircraftServices)
        {
            _aircraftServices = aircraftServices ?? throw new ArgumentNullException("aircraftServices");
        }

        public async Task<JsonResult> SearchAircrafts(string term, int pageSize = 10, int startIndex = 0)
        {
            var aircrafts = await _aircraftServices.SearchAircrafts(new SearchAircraftsArgs
            {
                Pagination = new PaginationArgs
                {
                    PageSize = pageSize,
                    StartIndex = startIndex
                },
                Filters = new AircraftSearchFiltersArgs
                {
                    Code = term
                }
            });

            return Json(aircrafts);
        }
    }
}