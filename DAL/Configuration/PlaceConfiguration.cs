using System.Data.Entity.ModelConfiguration;
using Domain.Entities;

namespace DAL
{
    public class PlaceConfiguration : EntityTypeConfiguration<Place>
    {
        internal PlaceConfiguration()
        {
            ToTable("Place");

            HasKey(x => x.Id)
                .Property(x => x.Id)
                .IsRequired();

            Property(x => x.Number)
                .IsRequired();

            Property(x => x.IsFree)
                .IsRequired();
        }
    }
}