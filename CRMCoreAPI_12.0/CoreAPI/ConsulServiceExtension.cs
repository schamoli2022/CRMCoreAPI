using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CRMCoreAPI
{
    public static class ConsulServiceExtension
    {
        public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                var host = configuration.GetValue<string>("ConsulConfig:ConsulHost");
                consulConfig.Address = new Uri(host);
            }));

            return services;
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IConfiguration configuration)
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("AppExtensions");
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            if (app.Properties["server.Features"] is not FeatureCollection features)
            {
                return app;
            }

            var servicePort = configuration.GetValue<int>("ConsulConfig:ServicePort");
            //  var serviceIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            var serviceIp = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

            var serviceName = configuration.GetValue<string>("ConsulConfig:ServiceName");
            var serviceId = serviceName + "-" + Guid.NewGuid();

            var registration = new AgentServiceRegistration()
            {
                ID = serviceId,
                Name = serviceName,
                Port = servicePort,
                Address = serviceIp.ToString(),//"172.19.128.111",//serviceIp.ToString(),

                Check = new AgentCheckRegistration()
                {
                    HTTP = $"http://{serviceIp}:{servicePort}/health",
                    Interval = TimeSpan.FromSeconds(10)
                }
            };

            logger.LogInformation("Registering with Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            consulClient.Agent.ServiceRegister(registration).ConfigureAwait(true);

            lifetime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("Unregistering from Consul");
                consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            });

            return app;
        }
    }
}
