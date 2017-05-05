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

            HasMany(x => x.Carriage);
        }
    }
}