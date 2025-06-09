using JKSN.Configuration;
using Tomlyn;

namespace JKSN
{
    public class Program
    {
        public static Config Configs { get; set; } = new Config();
        public static string ConfigPath { get; set; } = string.Empty;
        public static string VariablePath { get; set; } = string.Empty;

        public static void Main(string[] args)
        {
            var configPath = Environment.GetEnvironmentVariable("JKSN_CONFIG_PATH");
            var variablePath = Environment.GetEnvironmentVariable("JKSN_VARIABLE_PATH");
            if (OperatingSystem.IsLinux())
            {
                ConfigPath = configPath ?? Path.GetDirectoryName(Environment.ProcessPath)!.PrependPath("etc");
                VariablePath = variablePath ?? Path.GetDirectoryName(Environment.ProcessPath)!.PrependPath("var");
            }
            else
            {
                ConfigPath = configPath ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "JKSN", "config");
                VariablePath = variablePath ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "JKSN", "var");
            }

            var builder = Host.CreateApplicationBuilder(args);
            builder.Services
                .AddSystemd()
                .AddHostedService<Worker>();

            var host = builder.Build();

            host.Run();
        }

    }
}