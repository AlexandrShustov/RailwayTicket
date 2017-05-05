using System;
using BLL.Abstract;
using BLL.Concrete;
using DAL;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNet.Identity;
using Ninject;
using Ninject.Web.Common;

namespace Infrastructure
{
    public class NinjectKernelBinder
    {
        public void AddBindings(IKernel kernel)
        {
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InTransientScope().WithConstructorArgument("nameOrConnectionString", "RailwayTickets");
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IRoleService>().To<RoleService>();
            kernel.Bind<IRouteService>().To<RouteService>();
        }
    }
}