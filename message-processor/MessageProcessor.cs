using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace message_processor
{
    class MessageProcessor : IMessageProcessor
    {
        ILogger<MessageProcessor> logger;
        public MessageProcessor(ILogger<MessageProcessor> logger)
        {
            this.logger = logger;
        }
        async Task IMessageProcessor.ProcessMessage(string message)
        {
            logger.LogTrace($"{nameof(MessageProcessor)} - Started");
            switch (message)
            {
                case "crash":
                    logger.LogInformation($"Processing crash message with Environment.Exit(1). A new process will start after this.");
                    Environment.Exit(1);
                    break;
                case "exception": throw new InvalidOperationException("Simulated exception");
                case "delay":
                    logger.LogInformation($"Processing delay message with simulated 10s delay");
                    await Task.Delay(TimeSpan.FromSeconds(10));
                    break;
                default:
                    logger.LogInformation("Unknown message");
                    break;
            }
            logger.LogTrace($"{nameof(MessageProcessor)} - Completed");
        }
    }
}