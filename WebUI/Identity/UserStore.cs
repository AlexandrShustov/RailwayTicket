using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using Domain.Repositories;
using Microsoft.AspNet.Identity;

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
        private readonly IUnitOfWork _unitOfWork;

        public UserStore(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

      
        public Task CreateAsync(IdentityUser user)
        {
            Guard.ArgumentNotNull(user, "User should not be null");

            var u = GetUser(user);

            _unitOfWork.UserRepository.Add(u);

            return _unitOfWork.SaveChangesAsync();
        }

        public Task DeleteAsync(IdentityUser user)
        {
            Guard.ArgumentNotNull(user, "User should not be null");

            var u = GetUser(user);

            _unitOfWork.UserRepository.Remove(u);
            return _unitOfWork.SaveChangesAsync();
        }

        public Task<IdentityUser> FindByIdAsync(Guid userId)
        {
            var user = _unitOfWork.UserRepository.FindById(userId);
            return Task.FromResult<IdentityUser>(GetIdentityUser(user));
        }

        public Task<IdentityUser> FindByNameAsync(string userName)
        {
            var user = _unitOfWork.UserRepository.FindByUserName(userName);
            return Task.FromResult<IdentityUser>(GetIdentityUser(user));
        }

        public Task UpdateAsync(IdentityUser user)
        {
            Guard.ArgumentNotNull(user, "user should not be null");

            var u = _unitOfWork.UserRepository.FindById(user.Id);

            Guard.ArgumentNotNull(u, "IdentityUser does not correspond to a User entity.");

            PopulateUser(u, user);

            _unitOfWork.UserRepository.Update(u);

            return _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            
        }

        public Task AddClaimAsync(IdentityUser user, System.Security.Claims.Claim claim)
        {
            Guard.ArgumentNotNull(user, "User should not be null");

            Guard.ArgumentNotNull(claim, "Claim should not be null");

            var u = _unitOfWork.UserRepository.FindById(user.Id);

            Guard.ArgumentNotNull(u, "IdentityUser does not correspond to a User entity.");

            var c = new Domain.Entities.Claim
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                User = u
            };
            u.Claims.Add(c);

            _unitOfWork.UserRepository.Update(u);
            return _unitOfWork.SaveChangesAsync();
        }

        public Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(IdentityUser user)
        {
            Guard.ArgumentNotNull(user, "User should not be null");

            var u = _unitOfWork.UserRepository.FindById(user.Id);

            Guard.ArgumentNotNull(u, "IdentityUser does not correspond to a User entity.");

            return Task.FromResult<IList<System.Security.Claims.Claim>>(u.Claims.Select(x => new System.Security.Claims.Claim(x.ClaimType, x.ClaimValue)).ToList());
        }

        public Task RemoveClaimAsync(IdentityUser user, System.Security.Claims.Claim claim)
        {
            Guard.ArgumentNotNull(user, "User should not be null");
            Guard.ArgumentNotNull(claim, "Claim should not be null");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            Guard.ArgumentNotNull(u, "IdentityUser does not correspond to a User entity.");

            var c = u.Claims.FirstOrDefault(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
            u.Claims.Remove(c);

            _unitOfWork.UserRepository.Update(u);

            return _unitOfWork.SaveChangesAsync();
        }

        public Task AddLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            Guard.ArgumentNotNull(user, "User should not be null");
            Guard.ArgumentNotNull(login, "Login should not be null");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            Guard.ArgumentNotNull(u, "IdentityUser does not correspond to a User entity.");

            var l = new Domain.Entities.ExternalLogin
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                User = u
            };
            u.Logins.Add(l);

            _unitOfWork.UserRepository.Update(u);
            return _unitOfWork.SaveChangesAsync();
        }

        public Task<IdentityUser> FindAsync(UserLoginInfo login)
        {
            Guard.ArgumentNotNull(login, "Login should not be null");

            var identityUser = default(IdentityUser);

            var l = _unitOfWork.ExternalLoginRepository.GetByProviderAndKey(login.LoginProvider, login.ProviderKey);
            if (l != null)
                identityUser = GetIdentityUser(l.User);

            return Task.FromResult<IdentityUser>(identityUser);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user)
        {
            Guard.ArgumentNotNull(user, "User should not be null");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            Guard.ArgumentNotNull(u, "IdentityUser does not correspond to a User entity.");

            return Task.FromResult<IList<UserLoginInfo>>(u.Logins.Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey)).ToList());
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

        public Task AddToRoleAsync(IdentityUser user, string roleName)
        {
            Guard.ArgumentNotNull(user, "User should not be null");
            Guard.ArgumentNotWhiteSpaceOrNull(roleName, "Role name should not be null or white space.");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            Guard.ArgumentNotNull(u, "IdentityUser does not correspond to a User entity.");

            var r = _unitOfWork.RoleRepository.FindByName(roleName);
            Guard.ArgumentNotNull(r, "RoleName does not correspond to a Role entity.");

            u.Roles.Add(r);
            _unitOfWork.UserRepository.Update(u);

            return _unitOfWork.SaveChangesAsync();
        }

        public Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            Guard.ArgumentNotNull(user, "User should not be null");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            Guard.ArgumentNotNull(u, "IdentityUser does not correspond to a User entity.");

            return Task.FromResult<IList<string>>(u.Roles.Select(x => x.Name).ToList());
        }

        public Task<bool> IsInRoleAsync(IdentityUser user, string roleName)
        {
            Guard.ArgumentNotNull(user, "User should not be null");
            Guard.ArgumentNotWhiteSpaceOrNull(roleName, "Role name should not be null or white space.");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            Guard.ArgumentNotNull(u, "IdentityUser does not correspond to a User entity.");

            return Task.FromResult<bool>(u.Roles.Any(x => x.Name == roleName));
        }

        public Task RemoveFromRoleAsync(IdentityUser user, string roleName)
        {
            Guard.ArgumentNotNull(user, "User should not be null");
            Guard.ArgumentNotWhiteSpaceOrNull(roleName, "Role name should not be null or white space.");

            var u = _unitOfWork.UserRepository.FindById(user.Id);
            Guard.ArgumentNotNull(u, "IdentityUser does not correspond to a User entity.");

            var r = u.Roles.FirstOrDefault(x => x.Name == roleName);
            u.Roles.Remove(r);

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