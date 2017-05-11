using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Domain.Enumerations;

namespace WebUI.Models
{
    public class CarriageViewModel
    {
        public CarriageViewModel()
        {
            var items = new List<string>();

            items.Add(CarriageType.Lux.ToString());
            items.Add(CarriageType.Compartments.ToString());
            items.Add(CarriageType.ReservedSeat.ToString());

            CarriageTypes = new SelectList(items);
        }

        [HiddenInput]
        public int Id { get; set; }

        [Required]
        public int Places { get; set; }

        [Required]
        [DataType("int", ErrorMessage = "Please, input an integer.")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Please, input integer in correct range.")]
        public int Number { get; set; }

        [Required]
        public SelectList CarriageTypes { get; set; }

        public string SelectedType { get; set; }

        [Required]
        public int TrainId { get; set; }
    }
}