using System;
using NLog;

namespace StudyFiles.Logging
{
    public static class LoggerFactory
    {
        public static ILogger CreateLoggerFor<T>()
        {
            var type = typeof(T);

            string fullName = type.FullName 
                              ?? throw new ArgumentException($"Could not get full name of class {type}.");

            return new LoggerAdapter(fullName);
        }

    }
}
