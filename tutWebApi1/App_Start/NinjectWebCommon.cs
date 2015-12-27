[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(tutWebApi1.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(tutWebApi1.App_Start.NinjectWebCommon), "Stop")]

namespace tutWebApi1.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using tutWepApi1.Repository;
    using System.Web.Http;
    using WebApiContrib.IoC.Ninject;
    using Models;
    using Services;
    using System.Web.Http.Filters;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);

                // We installed Ninject.MVC3 via nuget.
                // This pakcage is for ASP.NET MVC nd below line is for Web project
                // So, Ninject.MVC3 does not know a WebApi
                // In order to work Ninjtec.MVC3 with WebApi, we need to install a package : WebApiContrib.ioc.Ninject
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                // After installing WebApiContrib.ioc.ninject, add the blow line
                // this is for supporting WebApi
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectResolver(kernel);
                GlobalConfiguration.Configuration.Services.Add(typeof(IFilterProvider), 
                    new NinjectWebApiFilterProvider(kernel));

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Load(new RepositoryModule());
            kernel.Bind<IIdentityService>().To<IdentityService>();
        }        
    }
}
