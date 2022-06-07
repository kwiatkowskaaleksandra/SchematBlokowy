[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SchematBlokowy.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(SchematBlokowy.App_Start.NinjectWebCommon), "Stop")]


namespace SchematBlokowy.App_Start
{
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Modules;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using SchematBlokowy.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

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
            var modules = new INinjectModule[] {
            new NinjectCoreApp()
            };
            var kernel = new StandardKernel(modules);


            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            }
            catch
            {
                kernel.Dispose();
                throw;
            }


            GlobalHost.DependencyResolver = new NinjectSignalRDependencyResolver(kernel);
            System.Web.Mvc.DependencyResolver.SetResolver(new Ninject.Web.Mvc.NinjectDependencyResolver(kernel));
            GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => new HubActivator(kernel));
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
        }

        internal class NinjectSignalRDependencyResolver : DefaultDependencyResolver
        {
            private readonly IKernel _kernel;
            public NinjectSignalRDependencyResolver(IKernel kernel)
            {
                _kernel = kernel;
            }

            public override object GetService(Type serviceType)
            {
                return _kernel.TryGet(serviceType) ?? base.GetService(serviceType);
            }

            public override IEnumerable<object> GetServices(Type serviceType)
            {
                return _kernel.GetAll(serviceType).Concat(base.GetServices(serviceType));
            }
        }

        public class HubActivator : IHubActivator
        {
            private readonly IKernel container;

            public HubActivator(IKernel container)
            {
                this.container = container;
            }

            public IHub Create(HubDescriptor descriptor)
            {
                return (IHub)container.GetService(descriptor.HubType);
            }
        }
    }
}