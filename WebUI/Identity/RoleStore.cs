using System;
using System.Threading.Tasks;
using AutoMapper;
using BLL;
using BLL.Abstract;
using Domain.Entities;
using Microsoft.AspNet.Identity;

namespace WebUI.Identity
{
    public class RoleStore : IRoleStore<IdentityRole, Guid>
    {
        private readonly IRoleService _roleService;
        private IMapper _mapper;

        public RoleStore(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        public Task CreateAsync(IdentityRole identityRole)
        {
            Guard.ArgumentNotNull(identityRole, nameof(identityRole) + "should be not null.");

            var role = _mapper.Map<Role>(identityRole);
            role.RoleId = identityRole.Id;

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
            var role = await _roleService.FindByNameAsync(roleName);

            if (role == null)
                return null;

            return Mapper.Map<IdentityRole>(role);
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