using System.Data.Entity.ModelConfiguration;
using Domain.Entities;

namespace DAL
{
    public class CarriageConfiguration : EntityTypeConfiguration<Carriage>
    {
        internal CarriageConfiguration()
        {
            ToTable("Carriage");

            HasKey(x => x.Id)
                .Property(x => x.Id)
                .IsRequired();

            Property(x => x.Number)
                .IsRequired();

            HasMany(x => x.Places);
        }
    }
}