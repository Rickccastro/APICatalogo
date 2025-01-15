namespace APICatalogo.Logging;

public class CustomerLogger : ILogger
{
    private string _loggerName;
    private CustomLoggerProviderConfiguration _loggerConfig;

    public CustomerLogger(string name, CustomLoggerProviderConfiguration loggerConfig)
    {
        this._loggerName = name;
        this._loggerConfig = loggerConfig;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
       return logLevel == _loggerConfig.LogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var mensagem = $"{logLevel.ToString()}:{eventId} -{formatter(state, exception)}";
        EscreverTextoArquivo(mensagem);
    }

    private void EscreverTextoArquivo(string mensagem)
    {
        var caminhoArquivoLog = @"D:\DEV\VISUAL STUDIO C\trabalhos\APICatalogo\APICatalogo\Logging\logs\LogsInformation.txt";
        using (var streamWriter = new StreamWriter(caminhoArquivoLog, true))
        {
            try
            {
                streamWriter.WriteLine(mensagem);

                streamWriter.Close();
            }
            catch(Exception)
            {
                throw;
            }
        }

    }
}
