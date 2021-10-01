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
        const string QUEUE_NAME = "myqueue";
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
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.Span);
                Logger.WriteLine($"🠗 EventReceived from RabbitMQ: {message}");
                try
                {
                    processor.ProcessMessage(message).Wait();
                }catch(Exception ex)
                {
                    logger.LogError(ex, "Processing resulted in exception");
                }
            };
            model.BasicConsume(queue: QUEUE_NAME,
                                    autoAck: true,
                                    consumer: consumer);
            logger.LogInformation($"{nameof(ExecuteAsync)} - Started listening on {QUEUE_NAME}");
            return Task.CompletedTask;
        }
    }
}