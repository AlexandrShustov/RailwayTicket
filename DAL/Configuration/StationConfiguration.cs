using System.Data.Entity.ModelConfiguration;
using Domain.Entities;

namespace DAL
{
    public class StationConfiguration : EntityTypeConfiguration<Station>
    {
        internal StationConfiguration()
        {
            ToTable("Station");

            HasKey(x => x.Id)
                .Property(x => x.Id)
                .IsRequired();

            Property(x => x.Name)
                .IsRequired();
        }
    }
}