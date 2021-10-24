
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace poc_asp_net_core_rabbitmq_producer.Controllers
{
    [ApiExplorerSettings(GroupName = @"RabbitMQ - Producer")]
    [Route("api/rabbitmq")]
    [ApiController]
    public class RabbitMqController : ControllerBase
    {
        private readonly ConnectionFactory _factory;
        public RabbitMqController(IConnectionFactory factory)
        {
            _factory = factory.Get();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("{nomeFila}")]
        public async Task<IActionResult> Salvar([FromRoute] string nomeFila, [FromBody]object message)
        {
            if (string.IsNullOrEmpty(nomeFila) || message == null) return BadRequest();

            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: nomeFila,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    var stringfiedMessage = JsonConvert.SerializeObject(message);
                    var byteMessage = Encoding.UTF8.GetBytes(stringfiedMessage);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: nomeFila,
                        basicProperties: null,
                        body: byteMessage
                    );
                }
            }

            return await Task.FromResult(Ok());
        }
    }
}
