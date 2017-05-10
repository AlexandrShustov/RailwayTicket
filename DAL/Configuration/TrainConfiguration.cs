using System.Data.Entity.ModelConfiguration;
using Domain.Entities;

namespace DAL
{
    public class TrainConfiguration : EntityTypeConfiguration<Train>
    {
        internal TrainConfiguration()
        {
            ToTable("Train");

            HasKey(x => x.Id)
                .Property(x => x.Id)
                .IsRequired();

            Property(x => x.Number)
                .IsRequired();

            Property(x => x.IsDeleted)
                .IsRequired();

            HasMany(x => x.Carriages);
        }
    }
}