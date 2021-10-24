using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc_asp_net_core_rabbitmq_producer
{
    public class ConnectionFactoryCreator : IConnectionFactory
    {
        public ConnectionFactory Get(string uri)
        {
            return new ConnectionFactory
            {
                Uri = new Uri(uri)
            };
        }

        public ConnectionFactory Get()
        {
            return new ConnectionFactory
            {
                HostName = "endereco.servidor.rabbitmq",
                UserName = "guest_usr",
                Password = "guest_pass"
            };
        }
    }
}
