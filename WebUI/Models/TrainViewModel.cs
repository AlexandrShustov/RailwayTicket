using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Domain.Entities;

namespace WebUI.Models
{
    public class TrainViewModel
    {
        [Required]
        [HiddenInput]
        public int Id { get; set; }

        [Required (ErrorMessage = nameof(Number) + "is required!")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid number value")]
        public int Number { get; set; }

        public List<Carriage> Carriages { get; set; }
    }
}