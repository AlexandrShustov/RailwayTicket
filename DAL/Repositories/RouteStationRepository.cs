using Domain.Entities;
using Domain.Repositories;

namespace DAL.Repositories
{
    public class RouteStationRepository : Repository<RouteStation>, IRouteStationRepository
    {
        public RouteStationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}