using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BLL.Abstract
{
    public interface IRoleService
    {
        Task AddToRoleAsync(IdentityUser user, string roleName);

        Task<IList<string>> GetRolesAsync(IdentityUser user);

        Task<bool> IsInRoleAsync(IdentityUser user, string roleName);

        Task RemoveFromRoleAsync(IdentityUser user, string roleName);


    }
}