using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Abstract;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Claim = System.Security.Claims.Claim;

namespace BLL.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService (IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task CreateUser (User user)
        {
            Guard.ArgumentNotNull(user, "User should not be null");

            _unitOfWork.UserRepository.Add(user);

            return _unitOfWork.SaveChangesAsync();
        }

        public Task DeleteAsync(User user)
        {
            Guard.ArgumentNotNull(user, "User should not be null");

            _unitOfWork.UserRepository.Remove(user);

            return _unitOfWork.SaveChangesAsync();
        }

        public Task<User> FindByIdAsync(Guid userId)
        {
            return _unitOfWork.UserRepository.FindByIdAsync(userId);
        }

        public Task<User> FindByEmailAsync(string email)
        {
            return _unitOfWork.UserRepository.FindByEmailAsync(email);
        }

        public Task UpdateAsync(User user)
        {
            _unitOfWork.UserRepository.Update(user);

            return _unitOfWork.SaveChangesAsync();
        }

        public async Task AddToRoleAsync(Guid userId, string roleName)
        {
            Guard.ArgumentNotWhiteSpaceOrNull(roleName, "RoleName should not be null or white space.");

            var user = await FindByIdAsync(userId);

            //TODO use variable
            user.Roles.Add(_unitOfWork.RoleRepository.FindByName(roleName));

            await UpdateAsync(user);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveFromRoleAsync(Guid userId, string roleName)
        {
            Guard.ArgumentNotWhiteSpaceOrNull(roleName);

            var user = await FindByIdAsync(userId);

            var role = _unitOfWork.RoleRepository.FindByName(roleName);
            
            user.Roles.Remove(role);

            await UpdateAsync(user);

            await _unitOfWork.SaveChangesAsync();
        }
        
        public async Task<IList<string>> GetRolesAsync(Guid userId)
        {
            var user = await FindByIdAsync(userId);
            Guard.ArgumentNotNull(user, "User should not be null.");

            var roles = user.Roles.Select(r => r.Name).ToList();

            return roles;
        }

        public async Task<bool> IsInRoleAsync(Guid userId, string roleName)
        {
            var user = await FindByIdAsync(userId);
            Guard.ArgumentNotNull(user, "User should not be null.");
            Guard.ArgumentNotWhiteSpaceOrNull(roleName);

            return user.Roles.Select(x => x.Name).Contains(roleName);
        }
    }
}