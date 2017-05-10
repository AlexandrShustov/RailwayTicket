using Domain.Entities;
using Domain.Repositories;

namespace DAL.Repositories
{
    public class PlaceRepository : Repository<Place>, IPlaceRepository
    {
        public PlaceRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}