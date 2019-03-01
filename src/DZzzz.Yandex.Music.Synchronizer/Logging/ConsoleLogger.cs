using System;

using DZzzz.Net.Logging.Interfaces;
using DZzzz.Net.Logging.Model;

namespace DZzzz.Yandex.Music.Synchronizer.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Write<T>(string message, LogLevel logLevel = LogLevel.Info, Exception e = null, string callerMemberName = null)
        {
        }
    }
}