using System;

namespace Domain.Entities
{
    public class Feedback
    {
        public int Id { get; set; }

        public Guid RelatedUserId { get; set; }

        public DateTime PostingDate { get; set; }

        public string FeedbackText { get; set; }

        public bool IsDeleted { get; set; }
    }
}