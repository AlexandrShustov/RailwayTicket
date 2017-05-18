using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;

namespace DAL.Repositories
{
    public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(ApplicationDbContext context) : base(context)
        {

        }

        Task<List<Feedback>> IRepository<Feedback>.GetAllAsync()
        {
            return Set.Where(f => !f.IsDeleted).ToListAsync();
        }

        List<Feedback> IRepository<Feedback>.GetAll()
        {
            return Set.Where(f => !f.IsDeleted).ToList();
        }
    }
}