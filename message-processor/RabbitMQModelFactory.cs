using RabbitMQ.Client;

namespace message_processor
{
    class RabbitMQModelFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>Configure all the hard coded values.</remarks>
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