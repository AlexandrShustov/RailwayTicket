using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Domain.Entities;

namespace WebUI.Models
{
    public class TicketViewModel
    {
        public TicketViewModel(IEnumerable<RouteStation> stations, IEnumerable<Carriage> carriages)
        {
            Stations = new SelectList(stations.Select(s => s.Station), "Name", "Name");

            Carriages = carriages.ToList();
        }

        public TicketViewModel() { }

        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public int RouteId { get; set; }

        [HiddenInput]
        public int TrainId { get; set; }

        [Required]
        [HiddenInput]
        public int TrainNumber { get; set; }

        [Required]
        public string DepartureStationName { get; set; }

        [Required]
        public string ArriveStationName { get; set; }

        [Required]
        [Range(1, 50)]
        public int CarriageNumber { get; set; }

        [Required]
        [Range(1, 54)]
        public int PlaceNumber { get; set; }

        public SelectList Stations { get; set; }

        public List<Carriage> Carriages { get; set; }
    }
}