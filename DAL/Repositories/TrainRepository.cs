using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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