using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TUI.Flights.Common.Args.Aircraft;
using TUI.Flights.Common.Args.Airport;
using TUI.Flights.Common.Args.Common;
using TUI.Flights.Common.Dtos.Aircraft;
using TUI.Flights.Common.Entities;
using TUI.Flights.Infrastructure.Base;

namespace TUI.Flights.Core.Services.AirportServices
{
    public class AircraftServices : IAircraftServices
    {
        private readonly IRepository<Aircraft> _aircraftsRepository;
        private readonly IMapper _autoMapper;

        public AircraftServices(IRepository<Aircraft> aircraftsRepository, IMapper autoMapper)
        {
            _aircraftsRepository = aircraftsRepository ?? throw new ArgumentNullException("aircraftsRepository");
            _autoMapper = autoMapper ?? throw new ArgumentNullException("autoMapper");
        }

        public async Task<IEnumerable<AircraftDto>> SearchAircrafts(SearchAircraftsArgs searchArgs)
        {

            Expression<Func<Aircraft, bool>> expression = null;

            if (searchArgs != null && searchArgs.Filters != null)
            {
                expression = (Aircraft aircraft) => (!string.IsNullOrEmpty(searchArgs.Filters.Code) && aircraft.Code.ToLower().Contains(searchArgs.Filters.Code.ToLower()));
            }

            var aircrafts = await _aircraftsRepository.SearchAsync(expression, searchArgs.Pagination.PageSize.Value, searchArgs.Pagination.StartIndex.Value);

            return _autoMapper.Map<IEnumerable<AircraftDto>>(aircrafts);
        }
    }
}
