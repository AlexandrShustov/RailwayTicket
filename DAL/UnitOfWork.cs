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
        private IStationRepository _stationRepository;
        private ITrainRepository _trainRepository;
        private ICarriageRepository _carriageRepository;
        private IPlaceRepository _placeRepository;
        private IRouteStationRepository _routeStationRepository;
        private ITicketRepository _ticketRepository;

        public UnitOfWork(string nameOrConnectionString)
        {
            _context = new ApplicationDbContext(nameOrConnectionString);
        }

        public IRoleRepository RoleRepository => _roleRepository ?? (_roleRepository = new RoleRepository(_context));

        public IUserRepository UserRepository => _userRepository ?? (_userRepository = new UserRepository(_context));

        public IRouteRepository RouteRepository => _routeRepository ?? (_routeRepository = new RouteRepository(_context));

        public IStationRepository StationRepository => _stationRepository ?? (_stationRepository = new StationRepository(_context));

        public ITrainRepository TrainRepository => _trainRepository ?? (_trainRepository = new TrainRepository(_context));

        public ICarriageRepository CarriageRepository => _carriageRepository ?? (_carriageRepository = new CarriageRepository(_context));

        public IPlaceRepository PlaceRepository => _placeRepository ?? (_placeRepository = new PlaceRepository(_context));

        public IRouteStationRepository RouteStationRepository => _routeStationRepository ?? (_routeStationRepository = new RouteStationRepository(_context));

        public ITicketRepository TicketRepository => _ticketRepository ?? (_ticketRepository = new TicketRepository(_context));

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