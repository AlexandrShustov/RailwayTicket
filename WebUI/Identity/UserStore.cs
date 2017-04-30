using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using BLL.Abstract;
using Domain.Repositories;
using Microsoft.AspNet.Identity;
using Claim = Domain.Entities.Claim;

namespace WebUI.Identity
{
    public class UserStore :
        IUserLoginStore<IdentityUser, Guid>,
        IUserClaimStore<IdentityUser, Guid>,
        IUserRoleStore<IdentityUser, Guid>,
        IUserPasswordStore<IdentityUser, Guid>,
        IUserSecurityStampStore<IdentityUser, Guid>,
        IUserStore<IdentityUser, Guid>,
        IDisposable
    {
        private  IUserService _userService;

        public UserStore()
        {
            
        }


        public Task CreateAsync(IdentityUser identityUser)
        {
            Guard.ArgumentNotNull(identityUser, "User should not be null");

            var user = GetUser(identityUser);

            return _userService.CreateUser(user); ;
        }

        public Task DeleteAsync(IdentityUser identityUser)
        {
            Guard.ArgumentNotNull(identityUser, "User should not be null");

            var user = GetUser(identityUser);

            return _userService.DeleteAsync(user);
        }

        public Task<IdentityUser> FindByIdAsync(Guid userId)
        {
            return Task.FromResult(GetIdentityUser(_userService.FindByIdAsync(userId).Result));
        }

        public Task<IdentityUser> FindByNameAsync(string userName)
        {
            return Task.FromResult(GetIdentityUser(_userService.FindByNameAsync(userName).Result));
        }

        public Task UpdateAsync(IdentityUser identityUser)
        {
            Guard.ArgumentNotNull(identityUser, "user should not be null");

            var user = _userService.FindByIdAsync(identityUser.Id).Result;

            Guard.ArgumentNotNull(user, "IdentityUser does not correspond to a User entity.");

            PopulateUser(user, identityUser);

            return _userService.UpdateAsync(user);
        }

        public void Dispose()
        {

        }

        public Task AddClaimAsync(IdentityUser identityUser, System.Security.Claims.Claim claim)
        {
            Guard.ArgumentNotNull(identityUser, "User should not be null");

            Guard.ArgumentNotNull(claim, "Claim should not be null");

            return _userService.AddClaimsAsync(identityUser.Id, claim);
        }

        public Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(IdentityUser identityUser)
        {
            Guard.ArgumentNotNull(identityUser, "User should not be null");

            return _userService.GetClaimsAsync(identityUser.Id);
        }

        public async Task RemoveClaimAsync(IdentityUser identityUser, System.Security.Claims.Claim claim)
        {
            Guard.ArgumentNotNull(identityUser, "User should not be null");
            Guard.ArgumentNotNull(claim, "Claim should not be null");

            await _userService.RemoveClaimAsync(identityUser.Id, claim);
        }

        public Task AddLoginAsync(IdentityUser identityUser, UserLoginInfo userloginInfo)
        {
            Guard.ArgumentNotNull(identityUser, "User should not be null");
            Guard.ArgumentNotNull(userloginInfo, "Login should not be null");

            return _userService.AddLoginAsync(identityUser.Id, userloginInfo);
        }

        public async Task<IdentityUser> FindAsync(UserLoginInfo userLoginInfo)
        {
            Guard.ArgumentNotNull(userLoginInfo, "Login should not be null");

            var identityUser = default(IdentityUser);

            var externalLogin = await _userService.FindAsync(userLoginInfo);
            identityUser = GetIdentityUser(externalLogin.User);

            return identityUser;
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user)
        {
            Guard.ArgumentNotNull(user, "User should not be null");

            return _userService.GetLoginsAsync(user.Id);
        }

        public Task RemoveLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            Guard.ArgumentNotNull(user, "User should not be null");
            Guard.ArgumentNotNull(login, "Login should not be null");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            Guard.ArgumentNotNull(u, "IdentityUser does not correspond to a User entity.");

            var l = u.Logins.FirstOrDefault(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey);
            u.Logins.Remove(l);

            _unitOfWork.UserRepository.Update(u);
            return _unitOfWork.SaveChangesAsync();
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
    }
}