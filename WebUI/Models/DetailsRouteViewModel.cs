using System.Collections.Generic;
using BLL.Concrete;
using Domain.Entities;

namespace WebUI.Models
{
    public class DetailsRouteViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int TrainNumber { get; set; }

        public int CommonCarriagesFreePlaces { get; set; }

        public int CompartmentCarriagesFreePlaces { get; set; }

        public int ReservedSeatCarriagesFreePlaces { get; set; }

        public int CarriagesCount { get; set; }

        public List<RouteStation> RouteStations { get; set; }
    }
}