using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebUI.Models
{
    public class SearchViewModel
    {
        [Required(ErrorMessage = "Field From is required.")]
        public string StationFrom { get; set; }

        [Required(ErrorMessage = "Field To is required.")]
        [Compare("StationFrom", ErrorMessage = "Stations are the same")]
        public string StationTo { get; set; }
    }
}