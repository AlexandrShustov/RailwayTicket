using System;
using System.Threading.Tasks;
using DAL.Repositories;
using Domain.Repositories;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
  
        private IRoleRepository _roleRepository;
        private IUserRepository _userRepository;
        private IRouteRepository _routeRepository;

        public UnitOfWork(string nameOrConnectionString)
        {
            _context = new ApplicationDbContext(nameOrConnectionString);
        }

        public IRoleRepository RoleRepository
        {
            get { return _roleRepository ?? (_roleRepository = new RoleRepository(_context)); }
        }

        public IUserRepository UserRepository
        {
            get { return _userRepository ?? (_userRepository = new UserRepository(_context)); }
        }

        public IRouteRepository RouteRepository
        {
            get { return _routeRepository ?? (_routeRepository = new RouteRepository(_context)); }
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _roleRepository = null;
            _userRepository = null;
            _routeRepository = null;
            _context.Dispose();
        }
    }
}