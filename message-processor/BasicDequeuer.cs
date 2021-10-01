using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace message_processor
{
    /// <summary>
    /// Procedural way to dequeue messages. This is NOT RECOMMENDED.
    /// </summary>
    [Obsolete]
    class BasicDequeuer
    {
        //Long technique to create ILogger.
        static Lazy<IMessageProcessor> processor = new Lazy<IMessageProcessor>(() =>
        {
            using ILoggerFactory loggerFactory =
                LoggerFactory.Create(builder =>
                    builder.AddSimpleConsole(options =>
                    {
                        options.IncludeScopes = true;
                        options.UseUtcTimestamp = true;
                    }));

            ILogger<MessageProcessor> logger = loggerFactory.CreateLogger<MessageProcessor>();

            return new MessageProcessor(logger);
        });
        internal static async Task TryUsingBasicGet(IModel channel)
        {
            Logger.WriteLine($"{nameof(TryUsingBasicGet)} start - infinite loop");
            while (true)
            {
                var queueingConsumer = new DefaultBasicConsumer(channel);
                var result = channel.BasicGet(Configurations.QueueName, false);
                if (result == null)
                {
                    Logger.WriteLine($"{nameof(TryUsingBasicGet)} - No messages found. Sleeping for 5 secs");
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    continue;
                }
                else
                {
                    IBasicProperties props = result.BasicProperties;
                    ReadOnlyMemory<byte> body = result.Body;
                    var message = Encoding.UTF8.GetString(body.Span);
                    Logger.WriteLine($"{nameof(TryUsingBasicGet)} - 🠗 Dequeued from RabbitMQ - DeliveryTag:{result.DeliveryTag},Redelivered: {result.Redelivered},msg: {message}");
                    try
                    {
                        await processor.Value.ProcessMessage(message);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"Exception in processing - {ex}");
                    }
                    channel.BasicAck(result.DeliveryTag, false);
                }
            }
        }
    }
}