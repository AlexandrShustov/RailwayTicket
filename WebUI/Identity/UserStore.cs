using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BLL;
using BLL.Abstract;
using Domain.Entities;
using Microsoft.AspNet.Identity;

namespace WebUI.Identity
{
    public class UserStore :
        IUserRoleStore<IdentityUser, Guid>,
        IUserPasswordStore<IdentityUser, Guid>,
        IUserSecurityStampStore<IdentityUser, Guid>
    {
        private readonly IUserService _userService;
        private IMapper _mapper;

        public UserStore(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public Task CreateAsync(IdentityUser identityUser)
        {
            Guard.ArgumentNotNull(identityUser, "IdentityUser should not be null");

            var user = new User();
            _mapper.Map(identityUser, user);

            return _userService.CreateUser(user); ;
        }

        public Task DeleteAsync(IdentityUser identityUser)
        {
            Guard.ArgumentNotNull(identityUser, "User should not be null");

            var user = new User();
            _mapper.Map(identityUser, user);

            return _userService.DeleteAsync(user);
        }

        public async Task<IdentityUser> FindByIdAsync(Guid userId)
        {
            var user = await _userService.FindByIdAsync(userId);
            var identityUser = new IdentityUser();

            return _mapper.Map(user, identityUser);
        }

        public async Task<IdentityUser> FindByNameAsync(string userEmail)
        {
            var user = await _userService.FindByEmailAsync(userEmail);
            var identityUser = new IdentityUser();

            return _mapper.Map(user, identityUser);
        }

        public async Task UpdateAsync(IdentityUser identityUser)
        {
            Guard.ArgumentNotNull(identityUser, "user should not be null");

            var user = await _userService.FindByIdAsync(identityUser.Id);

            Guard.ArgumentNotNull(user, "IdentityUser does not correspond to a User entity.");

            _mapper.Map(identityUser, user);

            await _userService.UpdateAsync(user);
        }

        public void Dispose()
        {

        }

        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            Guard.ArgumentNotNull(user, "User should not be null");

            return Task.FromResult<string>(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            Guard.ArgumentNotNull(user, "User should not be null");

            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;

            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(IdentityUser user)
        {
            Guard.ArgumentNotNull(user, "User should not be null");

            return Task.FromResult<string>(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(IdentityUser user, string stamp)
        {
            user.SecurityStamp = stamp;

            return Task.FromResult(0);
        }

        public async Task AddToRoleAsync(IdentityUser user, string roleName)
        {
            Guard.ArgumentNotNull(user, "User should not be null");
            Guard.ArgumentNotWhiteSpaceOrNull(roleName, "Role name should not be null.");

            await _userService.AddToRoleAsync(user.Id, roleName);
        }

        public async Task RemoveFromRoleAsync(IdentityUser user, string roleName)
        {
            Guard.ArgumentNotNull(user, "User should not be null");
            Guard.ArgumentNotWhiteSpaceOrNull(roleName, "Role name should not be null.");

            await _userService.RemoveFromRoleAsync(user.Id, roleName);
        }

        public Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            Guard.ArgumentNotNull(user);

            return _userService.GetRolesAsync(user.Id);
        }

        public Task<bool> IsInRoleAsync(IdentityUser user, string roleName)
        {
            Guard.ArgumentNotNull(user);
            Guard.ArgumentNotWhiteSpaceOrNull(roleName);

            return _userService.IsInRoleAsync(user.Id, roleName);
        }
    }
}