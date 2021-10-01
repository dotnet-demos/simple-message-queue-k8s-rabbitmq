using System;
using System.Threading.Tasks;

namespace message_processor
{
    class MessageProcessor : IMessageProcessor
    {
        async Task IMessageProcessor.ProcessMessage(string message)
        {
            Logger.WriteLine($"{nameof(MessageProcessor)} - Started");
            switch (message)
            {
                case "crash":
                    Logger.WriteLine($"Processing crash message with Environment.Exit(1). A new process will start after this.");
                    Environment.Exit(1);
                    break;
                case "exception": throw new InvalidOperationException("Simulated exception");
                case "delay":
                    Logger.WriteLine($"Processing delay message with simulated 10s delay");
                    await Task.Delay(TimeSpan.FromSeconds(10));
                    break;
                default:
                    Logger.WriteLine("Unknown message");
                    break;
            }
            Logger.WriteLine($"{nameof(MessageProcessor)} - Completed");
        }
    }
}