using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TUI.Flights.Common.Args.Airport;
using TUI.Flights.Common.Args.Common;
using TUI.Flights.Common.Dtos.Airport;
using TUI.Flights.Common.Entities;
using TUI.Flights.Infrastructure.Base;

namespace TUI.Flights.Core.Services.AirportServices
{
    public class AirportServices : IAirportServices
    {
        private readonly IRepository<Airport> _airportsRepository;
        private readonly IMapper _autoMapper;

        public AirportServices(IRepository<Airport> airportsRepository, IMapper autoMapper)
        {
            _airportsRepository = airportsRepository ?? throw new ArgumentNullException("airportsRepository");
            _autoMapper = autoMapper ?? throw new ArgumentNullException("autoMapper");
        }

        public async Task<IEnumerable<AirportDto>> GetAllAirports(PaginationArgs pagination)
        {
            var airports = await _airportsRepository.GetAllAsync();

            return _autoMapper.Map<IEnumerable<AirportDto>>(airports.ToList().Skip(pagination.StartIndex.Value).Take(pagination.PageSize.Value));
        }

        public async Task<IEnumerable<AirportDto>> SearchAirports(SearchAirportsArgs searchArgs)
        {

            Expression<Func<Airport, bool>> expression = null;

            if (searchArgs != null && searchArgs.Filters != null)
            {
                expression = (Airport airport) => (!string.IsNullOrEmpty(searchArgs.Filters.AirportName) && airport.Name.ToLower().Contains(searchArgs.Filters.AirportName.ToLower()));
            }

            var airports = await _airportsRepository.SearchAsync(expression);

            return _autoMapper.Map<IEnumerable<AirportDto>>(airports.Skip(searchArgs.Pagination.StartIndex.Value).Take(searchArgs.Pagination.PageSize.Value));
        }
    }
}
