using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;

namespace BLL.Abstract
{
    public interface IUserService
    {
        Task CreateUser(User user);

        Task DeleteAsync(User user);

        Task<User> FindByIdAsync(Guid userId);

        User FindById(Guid id);

        User FindByEmail(string email);

        Task<User> FindByEmailAsync(string email);

        Task UpdateAsync(User user);

        Task AddToRoleAsync(Guid userId, string roleName);

        Task RemoveFromRoleAsync(Guid userId, string roleName);

        Task<IList<string>> GetRolesAsync(Guid userId);

        Task<bool> IsInRoleAsync(Guid userId, string roleName);

        bool IsInRole(Guid userId, string roleName);

        Task<List<User>> GetAll();

        Task ChangeRoleTo(string role, Guid userId);
    }
}