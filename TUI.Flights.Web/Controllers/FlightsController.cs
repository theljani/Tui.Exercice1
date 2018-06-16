using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TUI.Flights.Core.Services.FlightServices;
using TUI.Flights.Common.Args.Common;
using TUI.Flights.Common.Dtos.Flight;
using TUI.Flights.Common.Args.Flight;

namespace TUI.Flights.Web.Controllers
{
    public class FlightsController : Controller
    {
        private readonly IFlightServices _flightServices;

        public FlightsController(IFlightServices flightServices)
        {
            _flightServices = flightServices ?? throw new ArgumentNullException("flightServices");
        }

        public async Task<IActionResult> GetFlightById(int flightId)
        {
            var flight = await _flightServices.GetFlight(new GetFlightArgs()
            {
                FlightId = flightId
            });

            return View(flight);
        }

        public async Task<IActionResult> Index(int pageSize = 100, int startIndex = 0)
        {
            var flights = await _flightServices.GetAllFlights(new PaginationArgs
            {
                PageSize = pageSize,
                StartIndex = startIndex
            });

            return View(flights);
        }

        public IActionResult CreateFlight()
        {
            return View();
        }

        public async Task<IActionResult> CreateFlightPost(CreateFlightArgs newFlight)
        {
            var createdFlight = await _flightServices.CreateFlight(newFlight);
            return RedirectToAction("index");
        }

        public async Task<IActionResult> UpdateFlightPost(FlightDto flight)
        {
            var flightArgs = new UpdateFlightArgs
            {
                Id = flight.Id,
                FlightNumber = flight.FlightNumber,
                AirportDepartureId = flight.AirportDepartureId,
                AirportDestinationId = flight.AirportDestinationId,
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

        public async Task<bool> ConfirmDeleteFlight(int flightId)
        {
            var deleted = await _flightServices.DeleteFlight(flightId);

            //var flights = await _flightServices.GetAllFlights(new PaginationArgs()
            //{
            //    PageSize = 100,
            //    StartIndex = 0
            //});

            return deleted;
        }

    }
}