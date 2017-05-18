using System.Data.Entity;
using Domain.Entities;

namespace DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        internal IDbSet<User> Users { get; set; }
        internal IDbSet<Role> Roles { get; set; }

        internal IDbSet<Route> Routes               { get; set; }
        internal IDbSet<Train> Trains               { get; set; }
        internal IDbSet<Carriage> Carriages         { get; set; }
        internal IDbSet<Place> Places               { get; set; }
        internal IDbSet<RouteStation> RouteStations { get; set; }
        internal IDbSet<Station> Stations           { get; set; }
        internal IDbSet<Ticket> Tickets             { get; set; }
        internal IDbSet<Feedback> Feedbacks         { get; set; }
             
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());

            modelBuilder.Configurations.Add(new RouteConfiguration());
            modelBuilder.Configurations.Add(new TrainConfiguration());
            modelBuilder.Configurations.Add(new CarriageConfiguration());
            modelBuilder.Configurations.Add(new PlaceConfiguration());
            modelBuilder.Configurations.Add(new RouteStationConfiguration());
            modelBuilder.Configurations.Add(new StationConfiguration());
            modelBuilder.Configurations.Add(new TicketConfiguration());
            modelBuilder.Configurations.Add(new FeedbackConfiguration());
        }
    }
}