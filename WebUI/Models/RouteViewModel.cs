using System;

namespace WebUI.Models
{
    public class RouteViewModel
    {
        public int Id { get; set; }

        public string FirstStationName { get; set; }

        public string LastStationName { get; set; }

        public int FreePlacesCount { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArriveTime { get; set; }
    }
}