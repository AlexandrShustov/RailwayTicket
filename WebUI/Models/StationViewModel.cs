using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class StationViewModel
    {
        [Required(ErrorMessage = "Field Name is required.")]
        [StringLength(50, ErrorMessage = "Name length must be less than 50 chars")]
        public string Name { get; set; }
    }
}