using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Actor1.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Remoting.V2.FabricTransport.Client;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using Microsoft.ServiceFabric.Services.Runtime;
using Stateless3;

namespace Stateless2
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class Stateless2 : StatelessService
    {
        public Stateless2(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[0];
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            //long iterations = 0;

            while (true)
            {
                var proxyFactory = new ServiceProxyFactory((c) =>
                {
                    return new FabricTransportServiceRemotingClientFactory();
                });
                // 
                var proxy = proxyFactory.CreateServiceProxy<ISimpleInterface>(new Uri("fabric:/SimpleListenerNetCore/Stateless3"));
                Activity.DefaultIdFormat = ActivityIdFormat.W3C;
                Activity.ForceDefaultIdFormat = true;
                var activity1 = new Activity("RunAsync");
                activity1.Start();
                try
                {
                    var result = await proxy.HelloHiNamaste("ameya");
                    Console.WriteLine(result);
                }
                catch (Exception e)
                {

                }
                Thread.Sleep(5 * 1000);
                var activity2 = new Activity("Actorcall");
                activity2.Start();
                var actorProxyFactory = new ActorProxyFactory((c) =>
                {
                    return new FabricTransportActorRemotingClientFactory(callbackMessageHandler: c);
                });
                var actorProxy = actorProxyFactory.CreateActorProxy<IActor1>(new Uri("fabric:/SimpleListenerNetCore/Actor1ActorService"), new ActorId(1));
                try
                {
                    var result = await actorProxy.GetCountAsync(new CancellationToken());
                    Console.WriteLine(result);
                }
                catch (Exception e)
                {

                }
                Thread.Sleep(5 * 1000);
            }
        }
    }
}
