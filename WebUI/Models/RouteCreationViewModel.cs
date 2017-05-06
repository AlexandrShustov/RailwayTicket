using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Domain.Entities;

namespace WebUI.Models
{
    public class RouteCreationViewModel
    {
        public List<Station> AllStations { get; set; }

        public SelectList StationsSelectItems { get; set; }

        public Station SelectedStation { get; set; }

        public DateTime DepartureTime { get; set; }

        public DateTime ArriveTime { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime ArriveDate { get; set; }
    }
}