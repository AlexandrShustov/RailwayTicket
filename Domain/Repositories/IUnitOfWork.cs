using System;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRoleRepository RoleRepository { get; }
        IUserRepository UserRepository { get; }
        IRouteRepository RouteRepository { get; }
        IStationRepository StationRepository { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}