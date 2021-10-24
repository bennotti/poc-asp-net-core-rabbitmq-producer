using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poc_asp_net_core_rabbitmq_producer
{
    public interface IConnectionFactory
    {
        ConnectionFactory Get();
        ConnectionFactory Get(string uri);
    }
}
