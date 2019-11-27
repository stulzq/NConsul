using System;
using System.Threading.Tasks;
using NConsul;

namespace GRPCHealthCheckTests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var consulClient = new ConsulClient(x => x.Address = new Uri($"http://localhost:8500"));
            var grpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                Interval = TimeSpan.FromSeconds(10),
                GRPC = "127.0.0.1:5000",
                GRPCUseTLS = false,
                Timeout = TimeSpan.FromSeconds(10)
            };

            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { grpCheck },
                ID = Guid.NewGuid().ToString(),
                Name = "grpctest",
                Address = "localhost",
                Port = 5000,
                Tags = new[] { $"xc/grpc/test" }
            };

            var res = await consulClient.Agent.ServiceRegister(registration);
            Console.WriteLine(res.StatusCode);
            Console.ReadKey();

        }
    }
}
