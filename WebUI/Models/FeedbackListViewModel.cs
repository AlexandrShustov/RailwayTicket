using System.Collections.Generic;

namespace WebUI.Models
{
    public class FeedbackListViewModel
    {
        public List<FeedbackViewModel> Feedbacks { get; set; }

        public FeedbackViewModel NewFeedback { get; set; }
    }
}