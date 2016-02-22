using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Ninject;
using Owin;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR.Hubs;

[assembly: OwinStartup(typeof(DependencyInjection.Startup))]

namespace DependencyInjection
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var kernel = new StandardKernel();
            var resolver = new NinjectDependencyResolver(kernel);

            kernel.Bind<IClockService>()
                  .To<ClockService>()
                  .InSingletonScope();

            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

            kernel.Bind(typeof(IHubConnectionContext<dynamic>))
                .ToMethod(context =>
                    resolver.Resolve<IConnectionManager>()
                            .GetHubContext<Clock>()
                            .Clients
                   ).WhenInjectedInto<ClockService>();

            var config = new HubConfiguration();
            config.Resolver = resolver;
            ConfigureSignalR(app, config);
        }

        public static void ConfigureSignalR(IAppBuilder app, HubConfiguration config)
        {
            app.MapSignalR(config);
        }
    }

    public class NinjectDependencyResolver : DefaultDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
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
}
