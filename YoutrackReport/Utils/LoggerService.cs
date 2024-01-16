namespace YoutrackReport.Utils
{
    public class LoggerService : ILogger
    {
        public IDisposable BeginScope<TState>(TState state) where TState : notnull => default!;
        public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Error;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {

            if (exception is not null)
            {
                switch (logLevel)
                {
                    case LogLevel.Error: Logger.logError(exception.Message, exception); break;
                    case LogLevel.Critical: Logger.logFatal(exception.Message, exception); break;
                    case LogLevel.Debug: Logger.logError(exception.Message, exception); break;
                    case LogLevel.Information: Logger.logInfo(exception.Message); break;
                    case LogLevel.Trace: Logger.logInfo(exception.Message); break;
                    // case LogLevel.Warning: Logger.logInfo(exception.Message); break; // no loguear warnings
                    default:
                        break;
                }

            }
            else
            {
                switch (logLevel)
                {
                    case LogLevel.Error: Logger.logError(state?.ToString()); break;
                    case LogLevel.Critical: Logger.logFatal(state?.ToString()); break;
                    case LogLevel.Debug: Logger.logError(state?.ToString()); break;
                    case LogLevel.Information: Logger.logInfo(state?.ToString()); break;
                    case LogLevel.Trace: Logger.logInfo(state?.ToString()); break;
                    // case LogLevel.Warning: Logger.logInfo(state?.ToString()); break; // no loguear warnings
                    default:
                        break;
                }
            }
            Console.WriteLine($"{formatter(state, exception)}");


        }

    }
}
