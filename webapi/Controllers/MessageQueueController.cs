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
    public class MessageQueueController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<MessageQueueController> _logger;

        public MessageQueueController(ILogger<MessageQueueController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "Not implemented";
        }
        [HttpPost]
        public void Post([FromBody] string message)
        {
            Console.WriteLine($"{nameof(MessageQueueController)}.{nameof(Post)} - {message}");
            Queue(message);
        }
        ConnectionFactory _factory;
        IConnection _conn;
        IModel _channel;
        private void Queue(string message) 
        {
            _factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
            _factory.UserName = "guest";
            _factory.Password = "guest";
            _conn = _factory.CreateConnection();
            _channel = _conn.CreateModel();
            _channel.QueueDeclare(queue: Configurations.QueueName,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "",
                                routingKey: "myqueue",
                                basicProperties: null,
                                body: body);
            Console.WriteLine("🠕 Queued {0} to RabbitMQ", message);
        }
    }
}
