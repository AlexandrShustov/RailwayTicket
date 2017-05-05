using Domain.Entities;
using Domain.Repositories;

namespace DAL.Repositories
{
    public class RouteRepository : Repository<Route>, IRouteRepository
    {

        public RouteRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}