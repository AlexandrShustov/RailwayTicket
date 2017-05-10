using System.Data.Entity.ModelConfiguration;
using Domain.Entities;

namespace DAL
{
    public class CarriageConfiguration : EntityTypeConfiguration<Carriage>
    {
        internal CarriageConfiguration()
        {
            ToTable("Carriages");

            HasKey(x => x.Id)
                .Property(x => x.Id)
                .IsRequired();

            Property(x => x.Number)
                .IsRequired();

            Property(x => x.CarriageType)
                .IsRequired();

            Property(x => x.IsDeleted)
                .IsRequired();

            HasMany(x => x.Places);
        }
    }
}