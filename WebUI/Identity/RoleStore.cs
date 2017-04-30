using System;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using BLL.Abstract;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace WebUI.Identity
{
    public class RoleStore : IRoleStore<IdentityRole, Guid>, IQueryableRoleStore<IdentityRole, Guid>, IDisposable
    {
        private IRoleService _roleService;

        public RoleStore(IRoleService roleService)
        {
            _roleService = roleService;
        }


        public Task CreateAsync(IdentityRole identityRole)
        {
            Guard.ArgumentNotNull(identityRole, nameof(identityRole) + "should be not null.");

            var role = GetRole(identityRole);

            return _roleService.CreateAsync(role);
        }

        public Task DeleteAsync(IdentityRole identityRole)
        {
            Guard.ArgumentNotNull(identityRole);

            var role = GetRole(identityRole);

            return _roleService.DeleteAsync(role);
        }

        public async Task<IdentityRole> FindByIdAsync(Guid roleId)
        {
            return GetIdentityRole(await _roleService.FindByIdAsync(roleId));
        }

        public async Task<IdentityRole> FindByNameAsync(string roleName)
        {
            return GetIdentityRole(await _roleService.FindByNameAsync(roleName));
        }

        public Task UpdateAsync(IdentityRole identityRole)
        {
            Guard.ArgumentNotNull(identityRole, nameof(identityRole) + "should not be null.");

            var role = GetRole(identityRole);

            return _roleService.UpdateAsync(role);
        }

        public void Dispose()
        {
            // Dispose does nothing since we want Unity to manage the lifecycle of our Unit of Work
        }

        public IQueryable<IdentityRole> Roles
            => _roleService.GetRolesAsQueryable().Select(r => GetIdentityRole(r));


        private Role GetRole(IdentityRole identityRole)
        {
            if (identityRole == null)
                return null;
            return new Role
            {
                RoleId = identityRole.Id,
                Name = identityRole.Name
            };
        }

        private IdentityRole GetIdentityRole(Role role)
        {
            if (role == null)
                return null;
            return new IdentityRole
            {
                Id = role.RoleId,
                Name = role.Name
            };
        }
    }
}