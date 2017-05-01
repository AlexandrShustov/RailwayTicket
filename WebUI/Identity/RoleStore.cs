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
    public class RoleStore : IRoleStore<IdentityRole, Guid>
    {
        private readonly IRoleService _roleService;

        public RoleStore(IRoleService roleService)
        {
            _roleService = roleService;
        }
        
        public Task CreateAsync(IdentityRole identityRole)
        {
            Guard.ArgumentNotNull(identityRole, nameof(identityRole) + "should be not null.");

            //TODO use mapper
            var role = GetRole(identityRole);

            return _roleService.CreateAsync(role);
        }

        public Task DeleteAsync(IdentityRole identityRole)
        {
            Guard.ArgumentNotNull(identityRole);

            //TODO use mapper
            var role = GetRole(identityRole);

            return _roleService.DeleteAsync(role);
        }

        public async Task<IdentityRole> FindByIdAsync(Guid roleId)
        {
            //TODO use mapper
            return GetIdentityRole(await _roleService.FindByIdAsync(roleId));
        }

        public async Task<IdentityRole> FindByNameAsync(string roleName)
        {
            //TODO use mapper
            return GetIdentityRole(await _roleService.FindByNameAsync(roleName));
        }

        public Task UpdateAsync(IdentityRole identityRole)
        {
            Guard.ArgumentNotNull(identityRole, nameof(identityRole) + "should not be null.");

            //TODO use mapper
            var role = GetRole(identityRole);

            return _roleService.UpdateAsync(role);
        }

        //TODO use mapper
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

        //TODO use mapper
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

        public void Dispose()
        {
        }
    }
}