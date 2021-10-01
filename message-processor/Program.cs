using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
                AppDomain.CurrentDomain.UnhandledException += (sender, e) => Logger.WriteLine($"UnhandledException {e.ExceptionObject}");
                //await RunAsInfiniteLoop();
                await RunTheDotNetWay(args);
            }
            catch (BrokerUnreachableException bue)
            {
                Logger.WriteLine("Broker seems offline. Real solution is to make sure message processor container starts after RabbitMQ is up and running. Hack: Let's wait for 5 seconds. Next invocation of exe will work hopefully.");
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Exception : {ex}");
            }
            Logger.WriteLine($"Exiting {nameof(message_processor)}. Ideally this is not expected as it should run for ever.");
        }

        async private static Task RunTheDotNetWay(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    services.AddSingleton<IMessageProcessor, MessageProcessor>();
                    services.AddTransient<IModel>((provider) => RabbitMQModelFactory.Get());
                    services.AddHostedService<DequeueBackgroudService>();
                    services.AddLogging(options =>
                    {
                        options.AddSimpleConsole(c => { c.UseUtcTimestamp = true; });
                    });
                })
                .UseConsoleLifetime()
                .Build()
                .RunAsync();
            
        }
        /// <summary>
        /// Runs the application using infinite loop that polls the queue and sleeps.
        /// </summary>
        private async static Task RunAsInfiniteLoop()
        {
            IModel channel = RabbitMQModelFactory.Get();
            await BasicDequeuer.TryUsingBasicGet(channel);
        }
    }
}