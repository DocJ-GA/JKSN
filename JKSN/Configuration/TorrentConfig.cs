using System.Net.Http.Json;
using JKSN.Gluetun;
using JKSN.qBittorrent;
using Newtonsoft.Json;

namespace JKSN.Configuration
{
    public class TorrentConfig
        : ITask
    {
        private string _varPath = string.Empty;
        private ILogger _logger = null!;
        private int _messageCount = 0;

        public string Name { get; set; } = "TorrentConfig";
        public string GluetunUri { get; set; } = "http://localhost:8082";
        public string qBittorrentUri { get; set; } = "http://localhost:8080";
        public string qBittorrentUsername { get; set; } = "admin";
        public string qBittorrentPassword { get; set; } = "adminadmin";
        public int Interval { get; set; } = 600; // Seconds: Default to 5 minutes
        public DateTimeOffset LastRun { get; set; } = DateTimeOffset.MinValue;
        public long Checks { get; set; } = 0;

        public TorrentConfig()
        {
            _varPath = Path.Combine(Program.VariablePath, $"torrentconfig-{Name}.json");
        }

        public void Load(ILogger logger)
            => LoadAsync(logger).Wait();

        public async Task LoadAsync(ILogger logger)
        {
            _logger = logger;

            try
            {
                var data = JsonConvert.DeserializeObject<Data>(await File.ReadAllTextAsync(_varPath));
                if (data != null)
                {
                    LastRun = data.LastRun ?? DateTimeOffset.MinValue;
                    Checks = data.Checks;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Failed to load PingUp {Name} data from variable file.");
            }
        }

        public void Run()
            => RunAsync().Wait();

        public async Task RunAsync()
        {
            if (Checks % 1000 == 0)
                _logger.LogInformation($"{Checks} done on {qBittorrentUri}.");
            var client = new HttpClient();
            var port = await client.GetFromJsonAsync<PortForwarded>(GluetunUri);
            if (port == null)
            {
                _logger.LogWarning("Port Forwarding not enabled or Gluetun is not running.");
                return;
            }
            var qBit = new qBittorrentClient(new Uri(qBittorrentUri));
            qBit.Login(qBittorrentUsername, qBittorrentPassword);
            var portUsing = qBit.GetPreferences().ListenPort;
            if (port.Port != portUsing)
            {
                _logger.LogInformation("Port Mismatch: " + portUsing + " != " + port.Port);
                _logger.LogInformation("Updating qBittorrent port. Count correct was " + _messageCount + ".");
                qBit.SetPreferences(new Dictionary<string, object>
                {
                    { "listen_port", port.Port }
                });
                _messageCount = 0;
                return;
            }

            _messageCount++;
            Checks++;
            await SaveAsync();
        }

        public void Save()
            => SaveAsync().Wait();

        public async Task SaveAsync()
        {
            try
            {
                var data = JsonConvert.SerializeObject(new { LastRun, Checks });
                await File.WriteAllTextAsync(_varPath, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to save TorrentConfig {Name} data to variable file.");
            }
        }

        private class Data
        {
            public DateTimeOffset? LastRun { get; set; }
            public long Checks { get; set; }
        }
    }
}
