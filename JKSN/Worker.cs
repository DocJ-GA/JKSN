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
                Initiate();
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

        private void InitiateWindows()
        {
            // ===========================
            // Setting up the config file.
            // ===========================
            _logger.LogInformation(" == Creating config file. ==");
            _logger.LogInformation($"Checking if config directory exists: '{Program.ConfigPath}'.");
            if (!Directory.Exists(Program.ConfigPath))
            {
                _logger.LogInformation($"Config directory does not exist. Creating it.");
                try
                {
                    Directory.CreateDirectory(Program.ConfigPath);
                }
                catch (UnauthorizedAccessException e)
                {
                    _logger.LogCritical($"You do not have permission to create the config directory. Please run this with elevated privileges or create the directory manually.");
                    _logger.LogCritical(e.Message);
                    Environment.Exit(1);
                }
                catch (IOException e)
                {
                    _logger.LogCritical($"An error occurred while creating the config directory. Please run this with elevated privileges or create the directory manually.");
                    _logger.LogCritical(e.Message);
                    Environment.Exit(1);
                }
            }
            else
            {
                _logger.LogInformation("Config directory already exists.");
            }

            // =================================
            // Setting up the variable directory
            // =================================
            _logger.LogInformation(" == Creating variable directory. ==");
            _logger.LogInformation($"Checking if variable directory exists: '{Program.VariablePath}'.");
            if (!Directory.Exists(Program.VariablePath))
            {
                _logger.LogInformation($"Variable directory does not exist. Creating it.");
                try
                {
                    Directory.CreateDirectory(Program.VariablePath);
                }
                catch (UnauthorizedAccessException e)
                {
                    _logger.LogCritical($"You do not have permission to create the config directory. Please run this with elevated privileges or create the directory manually.");
                    _logger.LogCritical(e.Message);
                    Environment.Exit(1);
                }
                catch (IOException e)
                {
                    _logger.LogCritical($"An error occurred while creating the config directory. Please run this with elevated privileges or create the directory manually.");
                    _logger.LogCritical(e.Message);
                    Environment.Exit(1);
                }
            }
            else
            {
                _logger.LogInformation("Variable directory already exists.");
            }

            _logger.LogInformation("Populating file.");
            try
            {
                File.WriteAllText(Path.Combine(Program.ConfigPath, "config.toml"), __configFile);
            }
            catch (Exception e)
            {
                _logger.LogCritical($"An error occurred while creating the config file. Please run this with elevated privileges or create the file manually.");
                _logger.LogCritical(e.Message);
                Environment.Exit(1);
            }
            Environment.Exit(0);
        }

        private void Initiate()
        {
            Program.Configs = new Config();
            _logger.LogInformation("Initial Setup");
            _logger.LogInformation("This is the first time you are running JKSN. We will create the config file and variable directory for you.");

            if (OperatingSystem.IsWindows())
            {
                InitiateWindows();
                Environment.Exit(0);
            }

            var superUser = "as root";
            if (Environment.UserName != "root")
            {
                _logger.LogCritical($"Initial setup has not been completed. You need to run this as root or create config file: '{Path.Combine(Program.ConfigPath, "config.toml").ToString()}'");
                Environment.Exit(1);
            }

            // Check if the user jksn exists, if not, create it.
            _logger.LogInformation("Checking if user jksn exists.");

            if (UserExists("jksn"))
            {
                _logger.LogInformation("User 'jksn' already exists.");
            }
            else
            {
                _logger.LogInformation("User 'jksn' does not exist. Creating user jksn.");
                RunCommand("useradd -M jksn");
                RunCommand("usermod -L jksn");
            }

            // ===========================
            // Setting up the config file.
            // ===========================
            _logger.LogInformation(" == Creating config file. ==");
            _logger.LogInformation($"Checking if config directory exists: '{Program.ConfigPath}'.");
            if (!Directory.Exists(Program.ConfigPath))
            {
                _logger.LogInformation($"Config directory does not exist. Creating it.");
                try
                {
                    Directory.CreateDirectory(Program.ConfigPath);
                }
                catch (UnauthorizedAccessException e)
                {
                    _logger.LogCritical($"You do not have permission to create the config directory. Please run this {superUser} or create the directory manually.");
                    _logger.LogCritical(e.Message);
                    Environment.Exit(1);
                }
                catch (IOException e)
                {
                    _logger.LogCritical($"An error occurred while creating the config directory. Please run this {superUser} or create the directory manually.");
                    _logger.LogCritical(e.Message);
                    Environment.Exit(1);
                }
            }
            else
            {
                _logger.LogInformation("Config directory already exists.");
            }

            _logger.LogInformation("Setting permissions.");
            RunCommand($"chmod 770 -R {Program.ConfigPath}");
            _logger.LogInformation($"Changing ownership of config directory to user jksn and group jksn.");
            RunCommand($"chown jksn:jksn -R {Program.ConfigPath}");

            // =================================
            // Setting up the variable directory
            // =================================
            _logger.LogInformation(" == Creating variable directory. ==");
            _logger.LogInformation($"Checking if variable directory exists: '{Program.VariablePath}'.");
            if (!Directory.Exists(Program.VariablePath))
            {
                _logger.LogInformation($"Variable directory does not exist. Creating it.");
                try
                {
                    Directory.CreateDirectory(Program.VariablePath);
                }
                catch (UnauthorizedAccessException e)
                {
                    _logger.LogCritical($"You do not have permission to create the config directory. Please run this {superUser} or create the directory manually.");
                    _logger.LogCritical(e.Message);
                    Environment.Exit(1);
                }
                catch (IOException e)
                {
                    _logger.LogCritical($"An error occurred while creating the config directory. Please run this {superUser} or create the directory manually.");
                    _logger.LogCritical(e.Message);
                    Environment.Exit(1);
                }
            }
            else
            {
                _logger.LogInformation("Variable directory already exists.");
            }

            _logger.LogInformation("Setting permissions for variable directory.");
            RunCommand($"chmod 770 -R {Program.VariablePath}");
            _logger.LogInformation($"Changing ownership of variable directory to user jksn and group jksn.");
            RunCommand($"chown jksn:jksn -R {Program.VariablePath}");


            _logger.LogInformation("Populating file.");
            try
            {
                File.WriteAllText(Path.Combine(Program.ConfigPath, "config.toml"), __configFile);
            }
            catch (Exception e)
            {
                _logger.LogCritical($"An error occurred while creating the config file. Please run this {superUser} or create the file manually.");
                _logger.LogCritical(e.Message);
                Environment.Exit(1);
            }
            Environment.Exit(0);
        }

        protected bool UserExists(string username)
        {
            try
            {
                string[] lines = File.ReadAllLines("/etc/passwd");
                foreach (string line in lines)
                    if (line.StartsWith(username + ":"))
                        return true;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while checking if user exists.");
                Environment.Exit(1);
            }

            return false;
        }

        protected bool RunCommand(string command, bool failOnError = true)
        {
            try
            {
                var createUser = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c '{command}'",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                Process proc = new Process { StartInfo = createUser };
                proc.Start();
                proc.WaitForExit();
                return proc.ExitCode == 0;
            }
            catch (Exception)
            {
                if (failOnError)
                {
                    _logger.LogCritical($"The bash command: '{command}' failed.");
                    Environment.Exit(1);
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private string __configFile = @"# This is the configuration file
# A torrent gluetun check looks like this





# EXAMPLES
#
# the name proptery of any task listed below MUST be uniqu across all task types.
# 
# [[torrents]]
# name = ""New Torrent""							#name of task.  Must be unique.
# gluetun_uri = ""http://localhost:8082""			#gluten url
# q_bittorrent_uri = ""http://localhost:8080""	    #qBitorrent url
# q_bittorrent_username = ""admin""				    #qBittorrent username
# q_bittorrent_password = ""adminadmin""			#qBittorrent password
# interval = 600								    #Interval in seconds
#
# [[ping_ups]]
# name = ""New PingUp""							    #name of task
# url = ""http://localhost:8082""					#the url for the tcp check
# port = 2025								    	#the port for the tcp check
# up_interval = 60000							   	#the interval in seconds to run
# down_interval = 300			    				#the interval in seconds to run when down
# refused_is_up = true			    		    	#treat refused connections as up or down
# pushover_api_key = """"							#the pushover api key
# pushover_user_key = """"					    	#the pushover user key
# notify_on_down = true					    		#send and error on going down
# notify_on_up = true					    		#notify if it comes up from down";
    }
}
