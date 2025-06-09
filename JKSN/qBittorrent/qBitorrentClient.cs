using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JKSN.qBittorrent
{
    public class qBittorrentClient
    {
        private JsonSerializerSettings _serializerSettings;

        /// <summary>
        /// The authorization cookie to use when accessing the API.
        /// </summary>
        public string? AuthCookie { get; private set; }

        /// <summary>
        /// The base URI of the API.  It should not include the trailing "/" or the "/api/v2/".
        /// </summary>
        public Uri URI { get; private set; }

        /// <summary>
        /// Constructs the client.
        /// </summary>
        /// <param name="uri">The base URI of the API.  It should not include the trailing "/" or the "/api/v2/".</param>
        public qBittorrentClient(Uri uri)
        {
            URI = uri;
            _serializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };
        }

        /// <summary>
        /// Gets the authorization cookie and sets it to the <seealso cref="AuthCookie"/>.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password</param>
        /// <exception cref="NullReferenceException">Thrown if the cookie wasn't obtained.</exception>
        public void Login(string username, string password)
        {
            var client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });
            var response = client.PostAsync(URI + "api/v2/auth/login", content).Result;
            var cookies = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
            if (cookies == null)
                throw new NullReferenceException("Cannot get cookie header.");
            var match = Regex.Matches(cookies, @"(SID=[^;]+);");
            if (match.Count < 1)
                throw new NullReferenceException("Cannot get cookie header.");
            if (match[0].Groups.Count < 2)
                throw new NullReferenceException("Cannot get cookie header.");
            AuthCookie = match[0].Groups[1].Value;

        }

        /// <summary>
        /// Gets a list of preferences and their values from the api.
        /// </summary>
        /// <returns>The <seealso cref="qBittorrentPreferences"/> with the values.</returns>
        /// <exception cref="Exception">Thrown if not authorized.</exception>
        /// <exception cref="NullReferenceException">Thrown if the data was unable to be retrieved.</exception>
        public qBittorrentPreferences GetPreferences()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("cookie", AuthCookie);
            var response = client.GetAsync(URI + "api/v2/app/preferences").Result;
            if (!response.IsSuccessStatusCode)
                throw new Exception("Not authorized.");
            var data = response.Content.ReadAsStringAsync().Result;
            var json = JsonConvert.DeserializeObject<qBittorrentPreferences>(data, _serializerSettings);
            if (json == null)
                throw new NullReferenceException("Could not deserialize data.");
            return json;
        }

        /// <summary>
        /// Gets the listen port for the qBittorrent process.
        /// </summary>
        /// <returns>The port being listened too.</returns>
        public int GetListenPort()
        {
            return GetPreferences().ListenPort;
        }

        /// <summary>
        /// Sets the given preferences in the web api.
        /// </summary>
        /// <param name="settings">The key value pair of preferences.</param>
        /// <returns>True if successful, false if not.</returns>
        public bool SetPreferences(Dictionary<string, object> settings)
        {
            var client = GetClient();
            var json = JsonConvert.SerializeObject(settings, _serializerSettings);
            var result = client.PostAsync(URI + "api/v2/app/setPreferences", new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("json", json) })).Result;
            return result.IsSuccessStatusCode;
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("cookie", AuthCookie);
            return client;
        }
    }
}
