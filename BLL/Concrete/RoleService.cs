using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Abstract;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BLL.Concrete
{
    public class RoleService : IRoleService
    {
        private IUnitOfWork _unitOfWork;

        public RoleService (IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public Task CreateAsync(Role role)
        {
            Guard.ArgumentNotNull(role, nameof(role) + "should not be null");

            _unitOfWork.RoleRepository.Add(role);

            return _unitOfWork.SaveChangesAsync();
        }

        public Task DeleteAsync(Role role)
        {
            Guard.ArgumentNotNull(role, nameof(role) + "should not be null");

            _unitOfWork.RoleRepository.Remove(role);

            return _unitOfWork.SaveChangesAsync();
        }

        public Task<Role> FindByIdAsync(Guid roleId)
        {
            return _unitOfWork.RoleRepository.FindByIdAsync(roleId);
        }

        public async Task<Role> FindByNameAsync(string roleName)
        {
            Guard.ArgumentNotWhiteSpaceOrNull(roleName);

            return await _unitOfWork.RoleRepository.FindByNameAsync(roleName);
        }

        public Task UpdateAsync(Role role)
        {
            _unitOfWork.RoleRepository.Update(role);

            return _unitOfWork.SaveChangesAsync();
        }

        public IQueryable<Role> GetRolesAsQueryable()
        {
            return _unitOfWork.RoleRepository.GetAll().AsQueryable();
        }
    }
}