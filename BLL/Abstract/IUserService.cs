using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BLL.Abstract
{
    public interface IUserService
    {
        Task CreateUser(User user);

        Task DeleteAsync(User user);

        Task<User> FindByIdAsync(Guid userId);

        Task<User> FindByNameAsync(string userName);

        Task UpdateAsync(User user);

        Task AddClaimsAsync(Guid userId, System.Security.Claims.Claim claim);

        Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(Guid identityUserId);

        Task RemoveClaimAsync(Guid identityUserId, System.Security.Claims.Claim claim);

        Task AddLoginAsync(Guid identityUserId, UserLoginInfo userloginInfo);

        Task<ExternalLogin> FindAsync(UserLoginInfo login);

        Task<IList<UserLoginInfo>> GetLoginsAsync(Guid userId);

        Task RemoveLoginAsync(Guid userId, UserLoginInfo login);

        Task AddToRoleAsync(Guid userId, string roleName);

        Task RemoveFromRoleAsync(Guid userId, string roleName);

        Task<IList<string>> GetRolesAsync(Guid userId);

        Task<bool> IsInRoleAsync(Guid userId, string roleName);
    }
}