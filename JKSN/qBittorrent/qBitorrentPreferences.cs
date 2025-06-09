using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKSN.qBittorrent
{
    public class qBittorrentPreferences
    {
        /// <summary>
        /// Currently selected language (e.g. en_GB for English)
        /// </summary>
        public string Locale { get; set; } = string.Empty;

        /// <summary>
        /// True if a subfolder should be created when adding a torrent
        /// </summary>
        public bool CreateSubfolderEnabled { get; set; } = false;

        /// <summary>
        /// True if torrents should be added in a Paused state
        /// </summary>
        public bool StartPausedEnabled { get; set; } = false;

        /// <summary>
        /// TODO
        /// </summary>
        public int AutoDeleteMode { get; set; } = -1;

        /// <summary>
        /// True if disk space should be pre-allocated for all files
        /// </summary>
        public bool PreallocateAll { get; set; } = false;

        /// <summary>
        /// True if ".!qB" should be appended to incomplete files
        /// </summary>
        public bool IncompleteFilesExt { get; set; } = false;

        /// <summary>
        /// True if Automatic Torrent Management is enabled by default
        /// </summary>
        public bool AutoTmmEnabled { get; set; } = false;

        /// <summary>
        /// True if torrent should be relocated when its Category changes
        /// </summary>
        public bool TorrentChangedTmmEnabled { get; set; } = false;

        /// <summary>
        /// True if torrent should be relocated when the default save path changes
        /// </summary>
        public bool SavePathChangedTmmEnabled { get; set; } = false;

        /// <summary>
        /// True if torrent should be relocated when its Category's save path changes
        /// </summary>
        public bool CategoryChangedTmmEnabled { get; set; } = false;

        /// <summary>
        /// Default save path for torrents, separated by slashes
        /// </summary>
        public string SavePath { get; set; } = string.Empty;

        /// <summary>
        /// True if folder for incomplete torrents is enabled
        /// </summary>
        public bool TempPathEnabled { get; set; } = false;

        /// <summary>
        /// Path for incomplete torrents, separated by slashes
        /// </summary>
        public string TempPath { get; set; } = string.Empty;

        /// <summary>
        /// Property: directory to watch for torrent files, value: where torrents loaded from this directory should be downloaded to (see list of possible values below). Slashes are used as path separators; multiple key/value pairs can be specified
        /// </summary>
        public Dictionary<string, string> ScanDirs { get; set; }

        /// <summary>
        /// Path to directory to copy .torrent files to. Slashes are used as path separators
        /// </summary>
        public string ExportDir { get; set; } = string.Empty;

        /// <summary>
        /// Path to directory to copy .torrent files of completed downloads to. Slashes are used as path separators
        /// </summary>
        public string ExportDirFin { get; set; } = string.Empty;

        /// <summary>
        /// True if e-mail notification should be enabled
        /// </summary>
        public bool MailNotificationEnabled { get; set; } = false;

        /// <summary>
        /// e-mail where notifications should originate from
        /// </summary>
        public string MailNotificationSender { get; set; } = string.Empty;

        /// <summary>
        /// e-mail to send notifications to
        /// </summary>
        public string MailNotificationEmail { get; set; } = string.Empty;

        /// <summary>
        /// smtp server for e-mail notifications
        /// </summary>
        public string MailNotificationSmtp { get; set; } = string.Empty;

        /// <summary>
        /// True if smtp server requires SSL connection
        /// </summary>
        public bool MailNotificationSslEnabled { get; set; } = false;

        /// <summary>
        /// True if smtp server requires authentication
        /// </summary>
        public bool MailNotificationAuthEnabled { get; set; } = false;

        /// <summary>
        /// Username for smtp authentication
        /// </summary>
        public string MailNotificationUsername { get; set; } = string.Empty;

        /// <summary>
        /// Password for smtp authentication
        /// </summary>
        public string MailNotificationPassword { get; set; } = string.Empty;

        /// <summary>
        /// True if external program should be run after torrent has finished downloading
        /// </summary>
        public bool AutorunEnabled { get; set; } = false;

        /// <summary>
        /// Program path/name/arguments to run if autorun_enabled is enabled; path is separated by slashes; you can use %f and %n arguments, which will be expanded by qBittorent as path_to_torrent_file and torrent_name (from the GUI; not the .torrent file name) respectively
        /// </summary>
        public string AutorunProgram { get; set; } = string.Empty;

        /// <summary>
        /// True if torrent queuing is enabled
        /// </summary>
        public bool QueueingEnabled { get; set; } = false;

        /// <summary>
        /// Maximum number of active simultaneous downloads
        /// </summary>
        public int MaxActiveDownloads { get; set; } = 0;

        /// <summary>
        /// Maximum number of active simultaneous downloads and uploads
        /// </summary>
        public int MaxActiveTorrents { get; set; } = 0;

        /// <summary>
        /// Maximum number of active simultaneous uploads
        /// </summary>
        public int MaxActiveUploads { get; set; } = 0;

        /// <summary>
        /// If true torrents w/o any activity (stalled ones) will not be counted towards max_active_* limits.
        /// </summary>
        public bool DontCountSlowTorrents { get; set; } = false;

        /// <summary>
        /// Download rate in KiB/s for a torrent to be considered "slow"
        /// </summary>
        public int SlowTorrentDlRateThreshold { get; set; } = 0;

        /// <summary>
        /// Upload rate in KiB/s for a torrent to be considered "slow"
        /// </summary>
        public int SlowTorrentUlRateThreshold { get; set; } = 0;

        /// <summary>
        /// Seconds a torrent should be inactive before considered "slow"
        /// </summary>
        public int SlowTorrentInactiveTimer { get; set; } = 0;

        /// <summary>
        /// True if share ratio limit is enabled
        /// </summary>
        public bool MaxRationEnabled { get; set; } = false;

        /// <summary>
        /// Get the global share ratio limit
        /// </summary>
        public float MaxRatio { get; set; } = 0f;

        /// <summary>
        /// Action performed when a torrent reaches the maximum share ratio. See list of possible values here below.
        /// 0	Pause torrent
        /// 1	Remove torrent
        /// </summary>
        public int MaxRatioAct { get; set; } = 1;

        /// <summary>
        /// Port for incoming connections
        /// </summary>
        public int ListenPort { get; set; } = -1;

        /// <summary>
        /// True if UPnP/NAT-PMP is enabled
        /// </summary>
        public bool Upnp { get; set; } = true;

        /// <summary>
        /// True if the port is randomly selected
        /// </summary>
        public bool RandomPort { get; set; } = false;

        /// <summary>
        /// Global upload speed limit in KiB/s; -1 means no limit is applied
        /// </summary>
        public int DlLimit { get; set; } = -1;

        /// <summary>
        /// Maximum global number of simultaneous connections
        /// </summary>
        public int UpLimit { get; set; } = -1;

        /// <summary>
        /// Maximum number of simultaneous connections per torrent
        /// </summary>
        public int MaxConnec { get; set; } = 500;

        /// <summary>
        /// Maximum number of simultaneous connections per torrent
        /// </summary>
        public int MaxConnecPerTorrent { get; set; } = 5;

        /// <summary>
        /// Maximum number of upload slots
        /// </summary>
        public int MaxUploads { get; set; } = 100;

        /// <summary>
        /// Maximum number of upload slots per torren
        /// </summary>
        public int MaxUploadsPerTorrent { get; set; } = 50;

        /// <summary>
        /// Timeout in seconds for a stopped announce request to trackers
        /// </summary>
        public int StopTrackerTimeout { get; set; } = 300;

        /// <summary>
        /// True if the advanced libtorrent option piece_extent_affinity is enabled
        /// </summary>
        public bool EnabledPieceExtentAffinity { get; set; } = false;

        /// <summary>
        /// Bittorrent Protocol to use (see list of possible values below)
        /// 0	TCP and μTP
        /// 1	TCP
        /// 2	μTP
        /// </summary>
        public int BittorrentProtocol { get; set; } = 0;

        /// <summary>
        /// True if [du]l_limit should be applied to uTP connections; this option is only available in qBittorent built against libtorrent version 0.16.X and higher
        /// </summary>
        public bool LimitUtpRate { get; set; } = false;

        /// <summary>
        /// True if [du]l_limit should be applied to estimated TCP overhead (service data: e.g. packet headers)
        /// </summary>
        public bool LimitTcpOverhead { get; set; } = false;

        /// <summary>
        /// True if [du]l_limit should be applied to peers on the LAN
        /// </summary>
        public bool LimitLanPeers { get; set; } = false;

        /// <summary>
        /// Alternative global download speed limit in KiB/s
        /// </summary>
        public int AltDlLimit { get; set; } = 10;

        /// <summary>
        /// Alternative global upload speed limit in KiB/s
        /// </summary>
        public int AltUpLimit { get; set; } = 10;

        /// <summary>
        /// True if alternative limits should be applied according to schedule
        /// </summary>
        public bool SchedulerEnabled { get; set; } = false;

        /// <summary>
        /// Scheduler starting hour
        /// </summary>
        public int ScheduleFromHour { get; set; } = 20;

        /// <summary>
        /// Scheduler starting minute
        /// </summary>
        public int ScheduleFromMin { get; set; } = 0;

        /// <summary>
        /// Scheduler ending hour
        /// </summary>
        public int ScheduleToHour { get; set; } = 6;

        /// <summary>
        /// Scheduler ending minute
        /// </summary>
        public int ScheduleToMin { get; set; } = 0;

        /// <summary>
        /// Scheduler days. See possible values here below.
        /// 0	Every day
        /// 1	Every weekday
        /// 2	Every weekend
        /// 3	Every Monday
        /// 4	Every Tuesday
        /// 5	Every Wednesday
        /// 6	Every Thursday
        /// 7	Every Friday
        /// 8	Every Saturday
        /// 9	Every Sunday
        /// </summary>
        public int SchedulerDays { get; set; } = 0;

        /// <summary>
        /// True if DHT is enabled
        /// </summary>
        public bool Dht { get; set; } = false;

        /// <summary>
        /// True if PeX is enabled
        /// </summary>
        public bool Pex { get; set; } = false;

        /// <summary>
        /// True if LSD is enabled
        /// </summary>
        public bool Lsd { get; set; } = false;

        /// <summary>
        /// See list of possible values here below
        /// 0	Prefer encryption
        /// 1	Force encryption on
        /// 2	Force encryption off
        /// </summary>
        public int Encryption { get; set; } = 0;

        /// <summary>
        /// If true anonymous mode will be enabled; this option is only available in qBittorent built against libtorrent version 0.16.X and higher
        /// </summary>
        public bool AnonymousMode { get; set; } = false;

        /// <summary>
        /// See list of possible values here below
        /// -1	Proxy is disabled
        /// 1	HTTP proxy without authentication
        /// 2	SOCKS5 proxy without authentication
        /// 3	HTTP proxy with authentication
        /// 4	SOCKS5 proxy with authentication
        /// 5	SOCKS4 proxy without authentication
        /// </summary>
        public string ProxyType { get; set; } = "None";

        /// <summary>
        /// Proxy IP address or domain name
        /// </summary>
        public string ProxyIp { get; set; } = string.Empty;

        /// <summary>
        /// Proxy port
        /// </summary>
        public int ProxyPort { get; set; } = -1;

        /// <summary>
        /// True if peer and web seed connections should be proxified; this option will have any effect only in qBittorent built against libtorrent version 0.16.X and higher
        /// </summary>
        public bool ProxyPeerConnection { get; set; } = false;

        /// <summary>
        /// True proxy requires authentication; doesn't apply to SOCKS4 proxies
        /// </summary>
        public bool ProxyAuthEnabled { get; set; } = false;

        /// <summary>
        /// Username for proxy authentication
        /// </summary>
        public string ProxyUsername { get; set; } = string.Empty;

        /// <summary>
        /// Password for proxy authentication
        /// </summary>
        public string ProxyPassword { get; set; } = string.Empty;

        /// <summary>
        /// True if proxy is only used for torrents
        /// </summary>
        public bool ProxyTorrentsOnly { get; set; } = false;

        /// <summary>
        /// True if external IP filter should be enabled
        /// </summary>
        public bool IpFilterEnabled { get; set; } = false;

        /// <summary>
        /// Path to IP filter file (.dat, .p2p, .p2b files are supported); path is separated by slashes
        /// </summary>
        public string IpFilterPath { get; set; } = string.Empty;

        /// <summary>
        /// True if IP filters are applied to trackers
        /// </summary>
        public bool IpFilterTrackers { get; set; } = false;

        /// <summary>
        /// Semicolon-separated list of domains to accept when performing Host header validation
        /// </summary>
        public string WebUiDomainList { get; set; } = string.Empty;

        /// <summary>
        /// IP address to use for the WebUI
        /// </summary>
        public string WebUiAddress { get; set; } = string.Empty;

        /// <summary>
        /// WebUI port
        /// </summary>
        public int WebUiPort { get; set; } = -1;

        /// <summary>
        /// True if UPnP is used for the WebUI port
        /// </summary>
        public bool WebUiUpnp { get; set; } = false;

        /// <summary>
        /// WebUI username
        /// </summary>
        public string WebUiUsername { get; set; } = string.Empty;

        /// <summary>
        /// For API ≥ v2.3.0: Plaintext WebUI password, not readable, write-only. For API < v2.3.0: MD5 hash of WebUI password, hash is generated from the following string: username:Web UI Access:plain_text_web_ui_password
        /// </summary>
        public string WebUiPassword { get; set; } = string.Empty;

        /// <summary>
        /// True if WebUI CSRF protection is enabled
        /// </summary>
        public bool WebUiCsrfProtectionEnabled { get; set; } = false;

        /// <summary>
        /// True if WebUI clickjacking protection is enabled
        /// </summary>
        public bool WebUiClickjackingProtectionEnabled { get; set; } = false;

        /// <summary>
        /// True if WebUI cookie Secure flag is enabled
        /// </summary>
        public bool WebUiSecureCookieEnabled { get; set; } = false;

        /// <summary>
        /// Maximum number of authentication failures before WebUI access ban
        /// </summary>
        public int WebUiMaxAuthFailCount { get; set; } = 5;

        /// <summary>
        /// WebUI access ban duration in seconds
        /// </summary>
        public int WebUiBanDuration { get; set; } = 300;

        /// <summary>
        /// Seconds until WebUI is automatically signed off
        /// </summary>
        public int WebUiSessionTimeout { get; set; } = 1000;

        /// <summary>
        /// True if WebUI host header validation is enabled
        /// </summary>
        public bool WebUiHostHeaderValidationEnabled { get; set; } = false;

        /// <summary>
        /// True if authentication challenge for loopback address (127.0.0.1) should be disabled
        /// </summary>
        public bool BypassLocalAuth { get; set; } = false;

        /// <summary>
        /// True if webui authentication should be bypassed for clients whose ip resides within (at least) one of the subnets on the whitelist
        /// </summary>
        public bool BypassAuthSubnetWhitelistEnabled { get; set; } = false;

        /// <summary>
        /// Whitelist of ipv4/ipv6 subnets for which webui authentication should be bypassed; list entries are separated by commas
        /// </summary>
        public string BypassAuthSubnetWhitelist { get; set; } = string.Empty;

        /// <summary>
        /// True if an alternative WebUI should be used
        /// </summary>
        public bool AlternativeWebuiEnabled { get; set; } = false;

        /// <summary>
        /// File path to the alternative WebUI
        /// </summary>
        public string AlternativeWebuiPath { get; set; } = string.Empty;

        /// <summary>
        /// True if WebUI HTTPS access is enabled
        /// </summary>
        public bool UseHttps { get; set; } = false;

        /// <summary>
        /// For API < v2.0.1: SSL keyfile contents (this is a not a path)
        /// </summary>
        public string SslKey { get; set; } = string.Empty;

        /// <summary>
        /// For API < v2.0.1: SSL certificate contents (this is a not a path)
        /// </summary>
        public string SslCert { get; set; } = string.Empty;

        /// <summary>
        /// For API ≥ v2.0.1: Path to SSL keyfile
        /// </summary>
        public string WebUiHttpsKeyPath { get; set; } = string.Empty;

        /// <summary>
        /// For API ≥ v2.0.1: Path to SSL certificate
        /// </summary>
        public string WebUiHttpsCertPath { get; set; } = string.Empty;

        /// <summary>
        /// True if server DNS should be updated dynamically
        /// </summary>
        public bool DyndnsEnabled { get; set; } = false;

        /// <summary>
        /// See list of possible values here below
        /// 0	Use DyDNS
        /// 1	Use NOIP
        /// </summary>
        public int DyndnsService { get; set; } = 0;

        /// <summary>
        /// Username for DDNS service
        /// </summary>
        public string DyndnsUsername { get; set; } = string.Empty;

        /// <summary>
        /// Password for DDNS service
        /// </summary>
        public string DyndnsPassword { get; set; } = string.Empty;

        /// <summary>
        /// Your DDNS domain name
        /// </summary>
        public string DyndnsDomain { get; set; } = string.Empty;

        /// <summary>
        /// RSS refresh interval
        /// </summary>
        public int RssRefreshInterval { get; set; } = 0;

        /// <summary>
        /// Max stored articles per RSS feed
        /// </summary>
        public int RssMaxArticlesPerFeed { get; set; } = 0;

        /// <summary>
        /// Enable processing of RSS feeds
        /// </summary>
        public bool RssProcessingEnabled { get; set; } = false;

        /// <summary>
        /// Enable auto-downloading of torrents from the RSS feeds
        /// </summary>
        public bool RssAutoDownloadingEnabled { get; set; } = false;

        /// <summary>
        /// For API ≥ v2.5.1: Enable downloading of repack/proper Episodes
        /// </summary>
        public bool RssDownloadRepackProperEpisodes { get; set; } = false;

        /// <summary>
        /// For API ≥ v2.5.1: List of RSS Smart Episode Filters
        /// </summary>
        public string RssSmartEpisodeFilters { get; set; } = string.Empty;

        /// <summary>
        /// Enable automatic adding of trackers to new torrents
        /// </summary>
        public bool AddTrackersEnabled { get; set; } = false;

        /// <summary>
        /// List of trackers to add to new torrent
        /// </summary>
        public string AddTrackers { get; set; } = string.Empty;

        /// <summary>
        /// For API ≥ v2.5.1: Enable custom http headers
        /// </summary>
        public bool WebUiUseCustomHttpHeadersEnabled { get; set; } = false;

        /// <summary>
        /// For API ≥ v2.5.1: List of custom http headers
        /// </summary>
        public string WebUiCustomHttpHeaders { get; set; } = string.Empty;

        /// <summary>
        /// True enables max seeding time
        /// </summary>
        public bool MaxSeedingTimeEnabled { get; set; } = false;

        /// <summary>
        /// Number of minutes to seed a torrent
        /// </summary>
        public int MaxSeedingTime { get; set; } = 0;

        /// <summary>
        /// TODO
        /// </summary>
        public string AnnounceIp { get; set; } = string.Empty;

        /// <summary>
        /// True always announce to all tiers
        /// </summary>
        public bool AnnounceToAllTiers { get; set; } = false;

        /// <summary>
        /// True always announce to all trackers in a tier
        /// </summary>
        public bool AnnounceToAllTrackers { get; set; } = false;

        /// <summary>
        /// Number of asynchronous I/O threads
        /// </summary>
        public int AsyncIoThreads { get; set; } = 0;

        /// <summary>
        /// List of banned IPs
        /// </summary>
        public string BannedIps { get; set; } = string.Empty;

        /// <summary>
        /// Outstanding memory when checking torrents in MiB
        /// </summary>
        public int CheckingMemoryUse { get; set; } = 0;

        /// <summary>
        /// IP Address to bind to. Empty String means All addresses
        /// </summary>
        public string CurrentInterfaceAddress { get; set; } = string.Empty;

        /// <summary>
        /// Network Interface used
        /// </summary>
        public string CurrentNetworkInterface { get; set; } = string.Empty;

        /// <summary>
        /// Disk cache used in MiB
        /// </summary>
        public int DiskCache { get; set; } = 0;

        /// <summary>
        /// Disk cache expiry interval in seconds
        /// </summary>
        public int DiskCacheTtl { get; set; } = 0;

        /// <summary>
        /// Port used for embedded tracker
        /// </summary>
        public int EmbeddedTrackerPort { get; set; } = 0;

        /// <summary>
        /// True enables coalesce reads & writes
        /// </summary>
        public bool EnableCoalesceReadWrite { get; set; } = false;

        /// <summary>
        /// True enables embedded tracker
        /// </summary>
        public bool EnableEmbeddedTracker { get; set; } = false;

        /// <summary>
        /// True allows multiple connections from the same IP address
        /// </summary>
        public bool EnableMultiConnectionsFromSameIp { get; set; } = false;

        /// <summary>
        /// True enables os cache
        /// </summary>
        public bool EnableOsCache { get; set; } = false;

        /// <summary>
        /// True enables sending of upload piece suggestions
        /// </summary>
        public bool EnableUploadSuggestions { get; set; } = false;

        /// <summary>
        /// File pool size
        /// </summary>
        public int FilePoolSize { get; set; } = 0;

        /// <summary>
        /// Maximal outgoing port (0: Disabled)
        /// </summary>
        public int OutgoingPortsMax { get; set; } = 0;

        /// <summary>
        /// Minimal outgoing port (0: Disabled)
        /// </summary>
        public int OutgoingPortsMin { get; set; } = 0;

        /// <summary>
        /// True rechecks torrents on completion
        /// </summary>
        public bool RecheckCompletedTorrents { get; set; } = false;

        /// <summary>
        /// True resolves peer countries
        /// </summary>
        public bool ResolvePeerCountries { get; set; } = false;

        /// <summary>
        /// Save resume data interval in min
        /// </summary>
        public int SaveResumeDataInterval { get; set; } = 0;

        /// <summary>
        /// Send buffer low watermark in KiB
        /// </summary>
        public int SendBufferLowWatermark { get; set; } = 0;

        /// <summary>
        /// Send buffer watermark in KiB
        /// </summary>
        public int SendBufferWatermark { get; set; } = 0;

        /// <summary>
        /// Send buffer watermark factor in percent
        /// </summary>
        public int SendBufferWatermarkFactor { get; set; } = 0;

        /// <summary>
        /// Socket backlog size
        /// </summary>
        public int SocketBacklogSize { get; set; } = 0;

        /// <summary>
        /// Upload choking algorithm used (see list of possible values below)
        /// 0	Round-robin
        /// 1	Fastest upload
        /// 2	Anti-leech
        /// </summary>
        public int UploadChokingAlgorithm { get; set; } = 0;

        /// <summary>
        /// Upload slots behavior used (see list of possible values below)
        /// 0	Fixed slots
        /// 1	Upload rate based
        /// </summary>
        public int UploadSlotsBehavior { get; set; } = 0;

        /// <summary>
        /// UPnP lease duration (0: Permanent lease)
        /// </summary>
        public int UpnpLeaseDuration { get; set; } = 0;

        /// <summary>
        /// μTP-TCP mixed mode algorithm (see list of possible values below)
        /// 0	Prefer TCP
        /// 1	Peer proportional
        /// </summary>
        public int UtpTcpMixedMode { get; set; } = 0;

        /// <summary>
        /// Initializes the preferences for qBittorrent.
        /// </summary>
        public qBittorrentPreferences()
        {
            ScanDirs = new Dictionary<string, string>();
        }
    }
}
