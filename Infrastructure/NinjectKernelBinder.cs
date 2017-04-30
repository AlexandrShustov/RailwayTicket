using System;
using BLL.Abstract;
using BLL.Concrete;
using DAL;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNet.Identity;
using Ninject;

namespace Infrastructure
{
    public class NinjectKernelBinder
    {
        public void AddBindings(IKernel kernel)
        {
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument("nameOrConnectionString", "RailwayTickets");
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IRoleService>().To<RoleService>();
        }
    }
}