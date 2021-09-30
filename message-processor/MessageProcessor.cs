using System;
using System.Threading.Tasks;

namespace message_processor
{
    class MessageProcessor
    {
        internal async static Task ProcessMessage(string message)
        {
            Logger.WriteLine($"{nameof(ProcessMessage)} - Started");
            switch (message)
            {
                case "crash": throw new InvalidOperationException("Simulated crash");
                case "delay":
                    Logger.WriteLine($"Processing delay message with simulated 10s delay");
                    await Task.Delay(TimeSpan.FromSeconds(10));
                    break;
                default:
                    Logger.WriteLine("Unknown message");
                    break;
            }
        }
    }
}