using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL;
using BLL.Abstract;
using Microsoft.AspNet.Identity;

namespace WebUI.Identity
{
    public class UserStore :
        IUserRoleStore<IdentityUser, Guid>,
        IUserPasswordStore<IdentityUser, Guid>,
        IUserSecurityStampStore<IdentityUser, Guid>
    {
        private readonly IUserService _userService;

        public UserStore(IUserService userService)
        {
            _userService = userService;
        }

        public Task CreateAsync(IdentityUser identityUser)
        {
            Guard.ArgumentNotNull(identityUser, "IdentityUser should not be null");

            var user = GetUser(identityUser);

            return _userService.CreateUser(user); ;
        }

        public Task DeleteAsync(IdentityUser identityUser)
        {
            Guard.ArgumentNotNull(identityUser, "User should not be null");

            var user = GetUser(identityUser);

            return _userService.DeleteAsync(user);
        }

        public async Task<IdentityUser> FindByIdAsync(Guid userId)
        {
            return GetIdentityUser(await _userService.FindByIdAsync(userId));
        }

        public async Task<IdentityUser> FindByNameAsync(string userEmail)
        { 
            return GetIdentityUser(await _userService.FindbyEmailAsync(userEmail));
        }

        public async Task UpdateAsync(IdentityUser identityUser)
        {
            Guard.ArgumentNotNull(identityUser, "user should not be null");

            var user = await _userService.FindByIdAsync(identityUser.Id);

            Guard.ArgumentNotNull(user, "IdentityUser does not correspond to a User entity.");

            PopulateUser(user, identityUser);

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
            return Task.FromResult<bool>(!string.IsNullOrWhiteSpace(user.PasswordHash));
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

        private Domain.Entities.User GetUser(IdentityUser identityUser)
        {
            if (identityUser == null)
                return null;
            
            var user = new Domain.Entities.User();
            PopulateUser(user, identityUser);

            return user;
        }

        private void PopulateUser(Domain.Entities.User user, IdentityUser identityUser)
        {
            user.UserId = identityUser.Id;
            user.UserName = identityUser.UserName;
            user.PasswordHash = identityUser.PasswordHash;
            user.SecurityStamp = identityUser.SecurityStamp;
        }

        private IdentityUser GetIdentityUser(Domain.Entities.User user)
        {
            if (user == null)
                return null;

            var identityUser = new IdentityUser();
            PopulateIdentityUser(identityUser, user);

            return identityUser;
        }

        private void PopulateIdentityUser(IdentityUser identityUser, Domain.Entities.User user)
        {
            identityUser.Id = user.UserId;
            identityUser.UserName = user.UserName;
            identityUser.PasswordHash = user.PasswordHash;
            identityUser.SecurityStamp = user.SecurityStamp;
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