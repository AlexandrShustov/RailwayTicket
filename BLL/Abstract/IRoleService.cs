using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BLL.Abstract
{
    public interface IRoleService
    {
        Task CreateAsync(Role role);

        Task DeleteAsync(Role role);

        Task<Role> FindByIdAsync(Guid roleId);

        Task<Role> FindByNameAsync(string roleName);

        Task UpdateAsync(Role role);

        IQueryable<Role> GetRolesAsQueryable();
    }
}