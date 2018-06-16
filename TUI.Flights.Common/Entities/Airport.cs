using TUI.Flights.Common.Entities.Base;
using TUI.Flights.Common.ValueObjects;

namespace TUI.Flights.Common.Entities
{
    public class Airport : Entity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }


        public virtual Location Location { get; set; }
    }
}