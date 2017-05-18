using System.Data.Entity.ModelConfiguration;
using Domain.Entities;

namespace DAL
{
    public class FeedbackConfiguration : EntityTypeConfiguration<Feedback>
    {
        internal FeedbackConfiguration()
        {
            ToTable("Feedbacks");

            HasKey(x => x.Id)
                .Property(x => x.Id)
                .IsRequired();

            Property(x => x.RelatedUserId)
                .IsRequired();

            Property(x => x.PostingDate)
                .IsRequired();

            Property(x => x.FeedbackText)
                .IsRequired();

            Property(x => x.IsDeleted)
                .IsRequired();
        }
    }
}