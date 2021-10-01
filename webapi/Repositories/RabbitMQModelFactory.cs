using RabbitMQ.Client;
using Shared;

namespace webapi.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <remarks>Configure all the hard coded values.</remarks>
    class RabbitMQModelFactory
    {
        internal static IModel Get()
        {
            ConnectionFactory factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
            factory.UserName = "guest";
            factory.Password = "guest";
            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();
            channel.QueueDeclare(queue: Configurations.QueueName,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
            return channel;
        }
    }
}