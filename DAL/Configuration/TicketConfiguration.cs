using System.Data.Entity.ModelConfiguration;
using Domain.Entities;

namespace DAL
{
    public class TicketConfiguration : EntityTypeConfiguration<Ticket>
    {
        internal TicketConfiguration()
        {
            ToTable("Ticket");

            HasKey(x => x.Id)
                .Property(x => x.Id)
                .IsRequired();

            Property(x => x.RelatedUserId)
                .IsRequired();

            Property(x => x.RouteId)
                .IsRequired();

            Property(x => x.ArriveTime)
                .HasColumnType("datetime")
                .IsRequired();

            Property(x => x.DepartureTime)
                .HasColumnType("datetime")
                .IsRequired();

            Property(x => x.CarriageNumber)
                .IsRequired();

            Property(x => x.PlaceNumber)
                .IsRequired();

            Property(x => x.PassangerName)
                .IsRequired();

            Property(x => x.Price)
                .IsRequired();

            Property(x => x.TrainNumber)
                .IsRequired();

            Property(x => x.ArriveStationName)
                .IsRequired();

            Property(x => x.DepartureStationName)
                .IsRequired();

            Property(x => x.TeaCount)
                .IsRequired();

            Property(x => x.IsNeedLinen)
                .IsRequired();
        }
    }
}