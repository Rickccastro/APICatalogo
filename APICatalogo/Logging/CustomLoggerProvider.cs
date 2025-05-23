﻿using System.Collections.Concurrent;

namespace APICatalogo.Logging;

public class CustomLoggerProvider : ILoggerProvider
{
   private readonly CustomLoggerProviderConfiguration _loggerConfig;
   private readonly ConcurrentDictionary<string, CustomerLogger> _loggers = new ConcurrentDictionary<string, CustomerLogger>();

    public CustomLoggerProvider(CustomLoggerProviderConfiguration config)
    {
        _loggerConfig = config;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new CustomerLogger(name, _loggerConfig));
    }

    public void Dispose()
    {
        _loggers.Clear();
    }
}
