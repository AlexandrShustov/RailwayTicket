using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Domain.Entities;

namespace WebUI.Models
{
    public class RouteStationCreateViewModel
    {
        [HiddenInput]
        [Required]
        public int RouteId { get; set; }

        [Required]
        public List<Station> AllStations { get; set; }

        public SelectList StationsSelectItems { get; set; }

        [Required]
        public int SelectedStation { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArriveTime { get; set; }

        [Required]
        [Remote("IsValidDates", "Route", AdditionalFields = "ArriveDate", ErrorMessage = "Departure date must be greater than arrive date!")]
        public DateTime DepartureDate { get; set; }

        [Required]
        public DateTime ArriveDate { get; set; }
    }
}