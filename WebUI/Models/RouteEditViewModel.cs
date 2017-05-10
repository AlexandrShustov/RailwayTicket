using System.Collections.Generic;
using System.Web.Mvc;
using Domain.Entities;

namespace WebUI.Models
{
    public class RouteEditViewModel
    {
        public int Id { get; set; }

        public SelectList Trains { get; set; }

        public int SelectedTrainId { get; set; }

        public List<RouteStation> Stations { get; set; }
    }
}