using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Threading.Tasks;

namespace message_processor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Logger.WriteLine($"Started {nameof(message_processor)}");
                AppDomain.CurrentDomain.UnhandledException += (sender, e) => Console.WriteLine($"UnhandledException {e.ExceptionObject}");
                //IModel channel = GetModel();
                //await TryUsingEventingConsumer(channel);
                //await BasicDequeuer.TryUsingBasicGet(channel);
                Host.CreateDefaultBuilder(args)
                    .ConfigureServices((hostBuilderContext, services) =>
                    {
                        services.AddSingleton<IMessageProcessor, MessageProcessor>();
                        services.AddTransient<IModel>((provider) => RabbitMQModelFactory.Get());
                        services.AddHostedService<DequeueBackgroudService>();
                    })
                    .UseConsoleLifetime()
                    .Build()
                    .Run();
            }
            catch (BrokerUnreachableException bue)
            {
                Logger.WriteLine("Broker seems offline. Realy solution is to make sure message processor starts after RabbitMQ is up and running. Hack: Let's wait for 5 seconds. Next invocation of exe will work hopefully.");
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Exception : {ex}");
            }
            Logger.WriteLine($"Exiting {nameof(message_processor)}. Ideally this is not expected as it should run for ever.");
        }
    }
}