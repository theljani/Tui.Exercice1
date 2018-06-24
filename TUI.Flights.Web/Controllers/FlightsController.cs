using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TUI.Flights.Core.Services.FlightServices;
using TUI.Flights.Common.Args.Common;
using TUI.Flights.Common.Dtos.Flight;
using TUI.Flights.Common.Args.Flight;
using TUI.Flights.Core.Services.AirportServices;
using TUI.Flights.Core.Services.AircraftServices;
using TUI.Flights.Common.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TUI.Flights.Web.Controllers
{
    public class FlightsController : Controller
    {
        private readonly IFlightServices _flightServices;
        private readonly IAirportServices _airportServices;
        private readonly IAircraftServices _aircraftServices;

        public FlightsController(IFlightServices flightServices,
                                 IAirportServices airportServices,
                                 IAircraftServices aircraftServices)
        {
            _flightServices = flightServices ?? throw new ArgumentNullException("flightServices");
            _airportServices = airportServices ?? throw new ArgumentNullException("airportServices");
            _aircraftServices = aircraftServices ?? throw new ArgumentNullException("aircraftServices");
        }

        public async Task<IActionResult> Index(int pageSize = 5, int pageIndex = 1)
        {
            var flights = await _flightServices.GetAllFlights(new PaginationArgs
            {
                PageSize = pageSize,
                StartIndex = (pageIndex - 1) * pageSize
            });

            var totalPages = (int)Math.Ceiling(flights.Total / (double)pageSize);

            var paginatedFlights = new PaginatedFlightList
            {
                Items = flights.Items,
                TotalCount = flights.Total,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                HasNext = pageIndex < totalPages,
                HasPrevious = pageIndex > 1
            };

            return View(paginatedFlights);
        }

        public async Task<IActionResult> GetFlightById(int flightId)
        {
            var aircrafts = await _aircraftServices.GetAllAircrafts(new PaginationArgs());
            var airports = await _airportServices.GetAllAirports(new PaginationArgs());

            ViewData["aircrafts"] = aircrafts;
            ViewData["airports"] = airports;

            var flight = await _flightServices.GetFlight(new GetFlightArgs()
            {
                FlightId = flightId
            });

            return View(flight);
        }

        public async Task<IActionResult> CreateFlight()
        {
            var aircrafts = await _aircraftServices.GetAllAircrafts(new PaginationArgs());
            var airports = await _airportServices.GetAllAirports(new PaginationArgs());

            ViewData["aircrafts"] = aircrafts;
            ViewData["airports"] = airports;

            return View();
        }

        public async Task<IActionResult> CreateFlightPost(CreateFlightArgs newFlight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdFlight = await _flightServices.CreateFlight(newFlight);
            return RedirectToAction("index");
        }

        public async Task<IActionResult> UpdateFlightPost(FlightDto flight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var flightArgs = new UpdateFlightArgs
            {
                Id = flight.Id,
                FlightNumber = flight.FlightNumber,
                DepartureAirportId = flight.DepartureAirportId,
                DestinationAirportId = flight.DestinationAirportId,
                FlightDate = flight.FlightDate,
                FlightTime = flight.FlightTime,
                AircraftId = flight.AircraftId
            };

            var updatedFlight = await _flightServices.UpdateFlight(flightArgs);

            return RedirectToAction("index");

        }

        public IActionResult SearchFlights()
        {
            return View();
        }

        public IActionResult DeleteFlight(int flightId)
        {
            ViewBag.flightId = flightId;

            return PartialView();
        }

        public async Task<IActionResult> ConfirmDeleteFlight(int flightId)
        {
            var deleted = await _flightServices.DeleteFlight(flightId);

            return RedirectToAction("Index");
        }
    }
}