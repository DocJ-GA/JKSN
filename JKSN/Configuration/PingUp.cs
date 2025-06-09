using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JKSN.PushoverAPI;
using System.Threading;

namespace JKSN.Configuration
{
    public class PingUp :
        ITask
    {
        protected string varPath = string.Empty;
        protected ILogger _logger = null!;
        protected string _varPath = string.Empty;

        public string Name { get; set; } = "PingUp";
        public string Url { get; set; } = "https://google.com";
        public int Port { get; set; } = 443; // Default to HTTPS port
        public int Interval
        {
            get
            {
                return IsUp ? UpInterval : DownInterval;
            }
        }
        public int DownInterval { get; set; } = 300; // Default to 5 minutes for down status
        public int UpInterval { get; set; } = 1800; // Default to 30 minutes for up status
        public bool RefusedIsUp { get; set; } = true; // Default to false, treat refused connections as down
        public string PushoverApiKey { get; set; } = "";
        public string PushoverUserKey { get; set; } = "";
        public bool NotifyOnDown { get; set; } = true; // Default to true, error on down
        public bool NotifyOnUp { get; set; } = true; // Default to true, notify on up
        public DateTimeOffset LastRun { get; set; } = DateTimeOffset.MinValue;
        public DateTimeOffset? StatusTime { get; set; } = null;
        public bool IsUp { get; set; } = false; // Default to false, initially down
        public DateTimeOffset LastStatusChange { get; set; } = DateTimeOffset.MinValue;

        public PingUp()
        {
            _varPath = Path.Combine(Program.VariablePath, $"pingup-{Name}.json");
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
                    StatusTime = data.StatusTime ?? DateTimeOffset.MinValue;
                    IsUp = data.IsUp;
                    LastStatusChange = data.LastStatusChange ?? DateTimeOffset.MinValue;
                }
            }
            catch (Exception)
            {
                _logger.LogWarning($"Failed to load PingUp {Name} data from variable file.");
            }
        }

        public void Save()
            => SaveAsync().Wait();

        public async Task SaveAsync()
        {
            try
            {
                var data = JsonConvert.SerializeObject(new Data() { IsUp = IsUp, LastRun = LastRun, StatusTime = StatusTime, LastStatusChange = LastStatusChange });
                await File.WriteAllTextAsync(_varPath, data);
            }
            catch (Exception)
            {
                _logger.LogError($"Failed to save PingUp {Name} data to variable file '{varPath}'.");
            }
        }

        public void Run()
            => RunAsync().Wait();

        public async Task RunAsync()
        {
            LastRun = DateTimeOffset.Now;
            bool isUp = false;
            try
            {
                using var client = new TcpClient();
                await client.ConnectAsync(Url, Port);
                client.Close();
                isUp = true;
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.ConnectionRefused && RefusedIsUp)
                    isUp = true;
            }

            if (isUp != IsUp)
            {
                LastStatusChange = LastRun;
                _logger.LogInformation($"PingUp {Name} is {(isUp ? "UP" : "DOWN")} at {LastStatusChange}.");
                if (!isUp && NotifyOnDown)
                {
                    // Log error and throw exception for DOWN status
                    _logger.LogInformation($"Sending pushover for {Name}.");
                    await Pushover.SendAsync(PushoverApiKey, PushoverUserKey, $"PingUp {Name} is DOWN at {LastStatusChange}, URL: {Url}, Port: {Port}", $"{Name} is DOWN", priority: 2, sound: "siren");
                }
                if (isUp && NotifyOnUp)
                {
                    // Log error and throw exception for UP status
                    _logger.LogInformation($"Sending pushover for {Name}.");
                    await Pushover.SendAsync(PushoverApiKey, PushoverUserKey, $"PingUp {Name} is UP at {LastStatusChange}, URL: {Url}, Port: {Port}", $"{Name} is UP", priority: 2, sound: "siren");
                }
            }
            IsUp = isUp;
            await SaveAsync();
        }

        private class Data
        {
            public DateTimeOffset? LastRun { get; set; }
            public DateTimeOffset? StatusTime { get; set; }
            public bool IsUp { get; set; } = false;
            public DateTimeOffset? LastStatusChange { get; set; }
        }
    }
}
