using System;
using System.Threading;

namespace message_processor
{
    class Logger
    {
        static internal void WriteLine(string message)
        {
            Console.WriteLine($"{DateTime.UtcNow} | {Environment.MachineName} | {Thread.CurrentThread.ManagedThreadId} | {message}");
        }
    }
}