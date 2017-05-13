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

        public List<Station> AllStations { get; set; }
        
        [HiddenInput]
        public SelectList StationsSelectItems { get; set; }

        [Required]
        public int SelectedStation { get; set; }

        [Required]
        [DataType(DataType.Time, ErrorMessage = "Please, input a correct datatime value.")]
        public DateTime DepartureTime { get; set; }

        [Required]
        [DataType(DataType.Time, ErrorMessage = "Please, input a correct datatime value.")]
        public DateTime ArriveTime { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Please, input a correct datatime value.")]
        public DateTime DepartureDate { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Please, input a correct datatime value.")]
        public DateTime ArriveDate { get; set; }
    }
}