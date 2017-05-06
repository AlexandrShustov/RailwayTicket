using Domain.Entities;
using Domain.Repositories;

namespace DAL.Repositories
{
    public class StationRepository : Repository<Station>, IStationRepository
    {
        public StationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}