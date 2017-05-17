using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure;
using Microsoft.AspNet.Identity;
using Ninject;
using NLog;
using WebUI.Identity;
using WebUI.Infrastructure;

namespace WebUI.App_Start
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            Mapper.Initialize((cfg)=> cfg.AddProfile<MapperProfile>());

            var binder = new NinjectKernelBinder();

            kernel.Bind<IUserStore<IdentityUser, Guid>>().To<UserStore>();
            kernel.Bind<IRoleStore<IdentityRole, Guid>>().To<RoleStore>();
            kernel.Bind<IMapper>().ToMethod(c => Mapper.Instance);
            kernel.Bind<Logger>().ToMethod(l => LogManager.GetCurrentClassLogger());

            binder.AddBindings(kernel);
        }
    }
}