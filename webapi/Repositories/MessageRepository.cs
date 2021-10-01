using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Shared;
using System;
using System.Text;

namespace webapi.Repositories
{
    class MessageRepository : IMessageRepository
    {
        private readonly ILogger<MessageRepository> _logger;
        public MessageRepository(ILogger<MessageRepository> logger)
        {
            _logger = logger;
        }
        uint IMessageRepository.Count(string queueName)
        {
            return RabbitMQModelFactory.Get().MessageCount(queueName);
        }
        void IMessageRepository.Send(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            RabbitMQModelFactory.Get().BasicPublish(exchange: "",
                                routingKey: Configurations.QueueName,
                                basicProperties: null,
                                body: body);
            _logger.LogInformation("🠕 Queued {0} to RabbitMQ", message);
        }
    }
}
