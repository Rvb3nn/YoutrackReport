using Serilog;

namespace YoutrackReport.Utils
{
    public static class Logger
    {

        public static Task logInfo(this string a)
        {
            return Task.Run(() =>
            {
                Serilog.Core.Logger log;
                log = LoggerSingleton.getLogger();
                log.Information(a);
            });
        }
        public static Task logError(this string a)
        {
            return Task.Run(() =>
            {
                Serilog.Core.Logger log;
                log = LoggerSingleton.getLogger();
                log.Error(a);
            });
        }

        public static Task logFatal(this string? a, Exception ex)
        {
            return Task.Run(() =>
            {
                Serilog.Core.Logger log;
                log = LoggerSingleton.getLogger();
                log.Fatal(ex, a ?? string.Empty);
            });
        }

        public static Task logFatal(this string? a)
        {
            return Task.Run(() =>
            {
                Serilog.Core.Logger log;
                log = LoggerSingleton.getLogger();
                log.Fatal(a ?? string.Empty);
            });
        }

        public static Task logError(this string a, Exception ex)
        {
            return Task.Run(() =>
            {
                Serilog.Core.Logger log;
                log = LoggerSingleton.getLogger();
                log.Error(ex, a);
            });
        }
    }

    public class LoggerSingleton : IDisposable
    {
        private IConfiguration myConfig;

        private static LoggerSingleton? instance;
        public Serilog.Core.Logger log;
        public string? rutaLog;
        public long tamanoLog;

        // el constructor debe estar public para que el singleton lo maneje la inyección de dependencias de asp.net core 6
        private LoggerSingleton()
        {
            myConfig = new ConfigurationBuilder()
              .SetBasePath(Path.GetFullPath(AppContext.BaseDirectory).ToString())
              .AddJsonFile("appSettings.json", false, true)
              .Build();

            rutaLog = Ambiente.getLogPath();
            tamanoLog = 1024 * 1024 * long.Parse(myConfig["ConfigLog:SizeLogGB"].ToString() ?? "0");
            log = new LoggerConfiguration().WriteTo.Async(a => a.File(rutaLog, fileSizeLimitBytes: tamanoLog, rollingInterval: RollingInterval.Day)).CreateLogger();
        }

        public static LoggerSingleton getInstance()
        {
            if (instance == null)
                instance = new LoggerSingleton();
            return instance;
        }

        public static Serilog.Core.Logger getLogger() => getInstance().log;

        public void Dispose() => log.Dispose();

    }
}
