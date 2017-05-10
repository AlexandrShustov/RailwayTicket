using Domain.Entities;
using Domain.Repositories;

namespace DAL.Repositories
{
    public class TrainRepository : Repository<Train>, ITrainRepository
    {
        public TrainRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}