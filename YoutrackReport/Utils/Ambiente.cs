namespace YoutrackReport.Utils
{
    public class Ambiente
    {
        private static IConfiguration myConfig = new ConfigurationBuilder()
               .SetBasePath(Path.GetFullPath(AppContext.BaseDirectory).ToString())
               .AddJsonFile("appSettings.json", false, true)
               .Build();

        public static string? getLogPath()
        {
            string? rutaAux;
            string? env;

            rutaAux = myConfig["ConfigLog:rutaLog"];
            env = Environment.GetEnvironmentVariable("NETLOGAPPLICATION");

            if (string.IsNullOrEmpty(rutaAux) && string.IsNullOrEmpty(env))
                return Path.Combine($@"C:\logApplication\YoutrackReport", "Log" + DateTime.Today + ".log");
            else if (string.IsNullOrEmpty(env) && !string.IsNullOrEmpty(rutaAux))
                return Path.Combine(@"C:\logApplication\YoutrackReport", rutaAux.TrimStart('\\'));
            else if (!string.IsNullOrEmpty(env) && string.IsNullOrEmpty(rutaAux))
                return Path.Combine(env.TrimEnd('\\'), "Log" + DateTime.Today + ".log");
            else
                return Path.Combine(env!.TrimEnd('\\')!, rutaAux.TrimStart('\\'));
        }
    }
}
