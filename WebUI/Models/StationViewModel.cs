using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class StationViewModel
    {
        [Required(ErrorMessage = "Field Name is required.")]
        public string Name { get; set; }
    }
}