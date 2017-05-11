using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;

namespace DAL.Repositories
{
    public class CarriageRepository : Repository<Carriage>, ICarriageRepository
    {
        public CarriageRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        Task<List<Carriage>> IRepository<Carriage>.GetAllAsync()
        {
            return Set.Where(c => !c.IsDeleted).ToListAsync();
        }
    }
}