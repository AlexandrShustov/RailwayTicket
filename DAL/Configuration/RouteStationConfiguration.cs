using System.Data.Entity.ModelConfiguration;
using Domain.Entities;

namespace DAL
{
    public class RouteStationConfiguration : EntityTypeConfiguration<RouteStation>
    {
        internal RouteStationConfiguration()
        {
            ToTable("RouteStation");

            HasKey(x => x.Id)
                .Property(x => x.Id)
                .IsRequired();

            Property(x => x.ArriveTime)
                .HasColumnType("datetime2")
                .IsOptional();

            Property(x => x.DepartureTime)
                .HasColumnType("datetime2")
                .IsOptional();

            HasRequired(x => x.Station);
        }
    }
}