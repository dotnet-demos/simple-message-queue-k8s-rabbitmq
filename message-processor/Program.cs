using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace message_processor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Started {nameof(message_processor)}");
            ConnectionFactory factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
            factory.UserName = "guest";
            factory.Password = "guest";
            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();
            channel.QueueDeclare(queue: "myqueue",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.Span;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"🠗 Received from RabbitMQ: {message}");
            };
            Console.WriteLine("Subscribed to the queue");
            channel.BasicConsume(queue: "myqueue",
                                    autoAck: true,
                                    consumer: consumer);
            Task.Delay(1000).Wait();
            Console.WriteLine($"Completed {nameof(message_processor)}");
        }
    }
}
