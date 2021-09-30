using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace message_processor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
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
                //await TryUsingEventingConsumer(channel);
                await TryUsingBasicGet(channel);
                return;
            }
            catch(BrokerUnreachableException bue)
            {
                Console.WriteLine("Broker seems offline. Let's wait for 2 seconds");
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception : {ex}");
            }
            Console.WriteLine($"Completed {nameof(message_processor)}. Ideally this is not expected as it should run for ever.");
        }

        private static async Task TryUsingEventingConsumer(IModel channel)
        {
            Console.WriteLine($"{nameof(TryUsingEventingConsumer)} start");
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.Span;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"🠗 EventReceived from RabbitMQ: {message}");
            };
            channel.BasicConsume(queue: "myqueue",
                                    autoAck: true,
                                    consumer: consumer);
            await Task.Delay(5000);
            Console.WriteLine("Subscribed to the queue.Waiting for messages using ReadLine...");
            Console.ReadLine();
            Console.WriteLine("Unfortunately the Console.ReadLine is not holding exe running...");
        }
        private static async Task TryUsingBasicGet(IModel channel)
        {
            Console.WriteLine($"{nameof(TryUsingBasicGet)} start");

            while (true)
            {
                var queueingConsumer = new DefaultBasicConsumer(channel);
                var result = channel.BasicGet("myqueue", false);

                if (result == null)
                {
                    // This 100ms wait allows the processor to go do other work.
                    // No sense in going back to an empty queue immediately. 
                    // CancellationToken intentionally not used!
                    // ReSharper disable once MethodSupportsCancellation

                    Console.WriteLine($"{nameof(TryUsingBasicGet)} - No messages found. Sleeping for 5 secs");
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    continue;
                }
                else
                {
                    IBasicProperties props = result.BasicProperties;
                    ReadOnlyMemory<byte> body = result.Body;
                    var message = Encoding.UTF8.GetString(body.Span);
                    Console.WriteLine($"🠗 {nameof(TryUsingBasicGet)} - Dequeued from RabbitMQ: {message}");
                }
            }
        }
    }
}