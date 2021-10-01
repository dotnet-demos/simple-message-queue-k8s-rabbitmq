using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace message_processor
{
    class DequeueBackgroudService : BackgroundService
    {
        private readonly ILogger<DequeueBackgroudService> logger;
        private readonly IModel model;
        IMessageProcessor processor;
        
        public DequeueBackgroudService(IModel model, IMessageProcessor processor, ILogger<DequeueBackgroudService> logger)
        {
            this.logger = logger;
            this.model = model;
            this.processor = processor;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Below code is to throttle the messages to 1 at a time. Change based on processing power.
            //https://stackoverflow.com/a/19163868/181832
            //model.BasicQos(0, 1, false);
            EventingBasicConsumer consumer = new EventingBasicConsumer(model);
            consumer.Received += (sender, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.Span);
                logger.LogInformation($"🠗 Dequeued from RabbitMQ - DeliveryTag:{ea.DeliveryTag},Redelivered: {ea.Redelivered},msg: {message}");
                try
                {
                    processor.ProcessMessage(message).Wait();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Exception in processing");
                }
                model.BasicAck(ea.DeliveryTag, false);
            };
            model.BasicConsume(queue:Configurations.QueueName,
                                    autoAck: false,
                                    consumer: consumer);
            logger.LogInformation($"{nameof(ExecuteAsync)} - Started listening on {Configurations.QueueName}");
            return Task.CompletedTask;
        }
    }
}