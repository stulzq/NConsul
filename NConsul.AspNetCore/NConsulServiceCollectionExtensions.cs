using System;
using Microsoft.Extensions.DependencyInjection;

namespace NConsul.AspNetCore
{
    public static class NConsulServiceCollectionExtensions
    {
        public static NConsulBuilder AddConsul(this IServiceCollection service, NConsulOptions options)
        {
            var consulClient = new ConsulClient(x => x.Address = new Uri(options.Address));
            service.AddSingleton(consulClient);
            service.Configure(new Action<NConsulOptions>(op =>
            {
                op.Address = options.Address;
                op.Token = options.Token;
            }));

            return new NConsulBuilder(consulClient);
        }

        public static NConsulBuilder AddConsul(this IServiceCollection service, string url)
        {
            var consulClient = new ConsulClient(x => x.Address = new Uri(url));
            service.AddSingleton(consulClient);
            service.Configure(new Action<NConsulOptions>(op => op.Address = url));

            return new NConsulBuilder(consulClient);
        }
    }
}