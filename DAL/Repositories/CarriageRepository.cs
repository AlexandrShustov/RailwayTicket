using Domain.Entities;
using Domain.Repositories;

namespace DAL.Repositories
{
    public class CarriageRepository : Repository<Carriage>, ICarriageRepository
    {
        public CarriageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}