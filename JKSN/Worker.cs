using JKSN.Configuration;
using System.Diagnostics;
using Tomlyn;

namespace JKSN
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;


        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Worker started at: {DateTimeOffset.Now}");
            _logger.LogInformation($"Loading configuration from: {Program.ConfigPath}");
            LoadConfig();
            _logger.LogInformation("Configuration loaded successfully.");


            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var task in Program.Configs.Tasks.Where(t => t.LastRun.AddSeconds(t.Interval) < DateTimeOffset.Now).ToArray())
                {
                    try
                    {
                        _logger.LogInformation($"Running task: {task.Name}");
                        await task.RunAsync();
                        _logger.LogInformation($"Task {task.Name} completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"An error occurred while running task: {task.Name}");
                    }
                }
                await Task.Delay(5000, stoppingToken);
            }

            _logger.LogInformation("Worker stopping at: {time}", DateTimeOffset.Now);
            _logger.LogInformation("Worker has stopped.");
        }

        private void LoadConfig()
        {
            var path = Path.Combine(Program.ConfigPath, "config.toml");
            if (!File.Exists(path))
            {
                _logger.LogCritical($"Configuration file not found at {path}. Please ensure the file exists.");
                _logger.LogCritical("You need to install using the install.sh script");
                _logger.LogCritical("Exiting application due to missing configuration file.");
                return;
            }
            Program.Configs = Toml.ToModel<Config>(File.ReadAllText(path));
            foreach (var torrent in Program.Configs.Torrents)
            {
                if (!Program.Configs.Tasks.TryAdd(torrent))
                    _logger.LogError($"Duplicate torrent configuration found for {torrent.Name}. Please check your config file.");
            }
            foreach (var pingUp in Program.Configs.PingUps)
            {
                if (!Program.Configs.Tasks.TryAdd(pingUp))
                    _logger.LogError($"Duplicate pingup configuration found for {pingUp.Name}. Please check your config file.");
            }
            foreach (var task in Program.Configs.Tasks)
            {
                task.Load(_logger);
            }
        }
    }
}
