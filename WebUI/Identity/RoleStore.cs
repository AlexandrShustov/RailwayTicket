using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private IMapper _mapper;

        public RoleStore(IRoleService roleService)
        {
            _roleService = roleService;
        }
        
        public Task CreateAsync(IdentityRole identityRole)
        {
            Guard.ArgumentNotNull(identityRole, nameof(identityRole) + "should be not null.");

            var role = _mapper.Map<Role>(identityRole);

            return _roleService.CreateAsync(role);
        }

        public Task DeleteAsync(IdentityRole identityRole)
        {
            Guard.ArgumentNotNull(identityRole);

            var role = _mapper.Map<Role>(identityRole);

            return _roleService.DeleteAsync(role);
        }

        public async Task<IdentityRole> FindByIdAsync(Guid roleId)
        {
            return _mapper.Map<IdentityRole>(await _roleService.FindByIdAsync(roleId));
        }

        public async Task<IdentityRole> FindByNameAsync(string roleName)
        {
            return _mapper.Map<IdentityRole>(await _roleService.FindByNameAsync(roleName));
        }

        public Task UpdateAsync(IdentityRole identityRole)
        {
            Guard.ArgumentNotNull(identityRole, nameof(identityRole) + "should not be null.");

            var role = _mapper.Map<Role>(identityRole);

            return _roleService.UpdateAsync(role);
        }

        public void Dispose()
        {
        }
    }
}