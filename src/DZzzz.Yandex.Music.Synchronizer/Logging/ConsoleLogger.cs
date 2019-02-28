using System;
using System.Threading;

using DZzzz.Net.Logging.Interfaces;
using DZzzz.Net.Logging.Model;

namespace DZzzz.Yandex.Music.Synchronizer.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Write<T>(string message, LogLevel logLevel = LogLevel.Info, Exception e = null)
        {
            string formattedMessage = $"[{logLevel,-5}] [{Thread.CurrentThread.ManagedThreadId,-10}] {message}";

            if (e != null)
            {
                formattedMessage = $"{formattedMessage} [{e}]";
            }

            Console.WriteLine(formattedMessage);
        }
    }
}