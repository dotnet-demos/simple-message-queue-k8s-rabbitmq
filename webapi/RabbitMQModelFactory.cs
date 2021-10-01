using RabbitMQ.Client;
namespace SimpleK8sHosting
{
    class RabbitMQModelFactory
    {
        internal static IModel Get()
        {
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
            return channel;
        }
    }
}
