using Ninject;
using Syngenta.Data;
using Syngenta.Core.Contracts.Data;
using Syngenta.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Syngenta.Web.Configurations
{
    public class IocConfig
    {
        public static void RegisterIoc(HttpConfiguration config)
        {
            var kernel = new StandardKernel(); // Ninject IoC

            // These registrations are "per instance request".
            // See http://blog.bobcravens.com/2010/03/ninject-life-cycle-management-or-scoping/

            kernel.Bind<RepositoryFactories>().To<RepositoryFactories>()
                .InSingletonScope();

            kernel.Bind<IRepositoryProvider>().To<RepositoryProvider>();
            kernel.Bind<ISyngentaUow>().To<SyngentaUow>();

            // Tell WebApi how to use our Ninject IoC
            config.DependencyResolver = new NinjectDependencyResolver(kernel);

            //Tell normal controller how to use our Ninject IOC
            System.Web.Mvc.DependencyResolver.SetResolver(new NinjectMvcDependencyResolver(kernel));
        }
    }
}