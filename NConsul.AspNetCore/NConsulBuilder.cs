using System;
using System.Collections.Generic;
using System.Net.Mime;

namespace NConsul.AspNetCore
{
    public class NConsulBuilder
    {
        private readonly ConsulClient _client;
        private readonly List<AgentServiceCheck> _checks=new List<AgentServiceCheck>();

        public NConsulBuilder(ConsulClient client)
        {
            _client = client;
        }
        public NConsulBuilder AddHealthCheck(AgentServiceCheck check)
        {
            _checks.Add(check);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timeout">unit: second</param>
        /// <param name="interval">check interval. unit: second</param>
        /// <returns></returns>
        public NConsulBuilder AddHttpHealthCheck(string url,int timeout=10,int interval=10)
        {
            _checks.Add(new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(20),
                Interval = TimeSpan.FromSeconds(interval),
                HTTP = url,
                Timeout = TimeSpan.FromSeconds(timeout)
            });
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint">GPRC service address.</param>
        /// <param name="grpcUseTls"></param>
        /// <param name="timeout">unit: second</param>
        /// <param name="interval">check interval. unit: second</param>
        /// <returns></returns>
        public NConsulBuilder AddGRPCHealthCheck(string endpoint,bool grpcUseTls=false, int timeout = 10, int interval = 10)
        {
            _checks.Add(new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(20),
                Interval = TimeSpan.FromSeconds(interval),
                GRPC = endpoint,
                GRPCUseTLS = grpcUseTls,
                Timeout = TimeSpan.FromSeconds(timeout)
            });
            return this;
        }

        public void RegisterService(string name,string host,int port,string[] tags)
        {
            var registration = new AgentServiceRegistration()
            {
                Checks = _checks.ToArray(),
                ID = Guid.NewGuid().ToString(),
                Name = name,
                Address = host,
                Port = port,
                Tags = tags
            };

            _client.Agent.ServiceRegister(registration).Wait();

            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                _client.Agent.ServiceDeregister(registration.ID).Wait();
            };
        }

        /// <summary>
        /// 移除服务
        /// </summary>
        /// <param name="serviceId"></param>
        public void Deregister(string serviceId)
        {
            _client?.Agent?.ServiceDeregister(serviceId).GetAwaiter().GetResult();
        }

    }
}
