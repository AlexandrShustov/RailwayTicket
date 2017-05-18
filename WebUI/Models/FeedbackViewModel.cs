using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebUI.Models
{
    public class FeedbackViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public Guid RelatedUserId { get; set; }

        [HiddenInput]
        public string UserName { get; set; }

        [HiddenInput]
        public DateTime PostingDate { get; set; }

        [Required(ErrorMessage = "Field <text> is required")]
        [StringLength(250, ErrorMessage = "The feedback is too long.")]
        public string FeedbackText { get; set; }
    }
}