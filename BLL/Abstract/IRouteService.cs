using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace BLL.Abstract
{
    public interface IRouteService
    {
        Task<List<Route>> GetAll();

        Task DeleteRoute(int id);
    }
}