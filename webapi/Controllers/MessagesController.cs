using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SimpleK8sHosting.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(ILogger<MessagesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return $"Messages in {Configurations.QueueName} is {RabbitMQModelFactory.Get().MessageCount(Configurations.QueueName)}";
        }
        [HttpPost]
        public void Post([FromBody] string message)
        {
            Console.WriteLine($"{nameof(MessagesController)}.{nameof(Post)} - {message}");
            Queue(message);
        }
        ConnectionFactory _factory;
        IConnection _conn;
        IModel _channel;
        private void Queue(string message) 
        {
            var body = Encoding.UTF8.GetBytes(message);
            RabbitMQModelFactory.Get().BasicPublish(exchange: "",
                                routingKey: Configurations.QueueName,
                                basicProperties: null,
                                body: body);
            Console.WriteLine("🠕 Queued {0} to RabbitMQ", message);
        }
    }
}
