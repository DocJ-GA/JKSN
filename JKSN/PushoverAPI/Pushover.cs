using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKSN.PushoverAPI
{
    public static class Pushover
    {
        public static async Task<HttpResponseMessage> SendAsync(string appKey, string userKey, string message, string title = "JKSN Notification", int priority = 0, string sound = "pushover")
        {
            var parameters = new Dictionary<string, string>
            {
                ["token"] = appKey,
                ["user"] = userKey,
                ["message"] = message,
                ["title"] = title,
                ["sound"] = sound,
                ["priority"] = priority.ToString()
            };
            if (priority == 2)
            {
                parameters["retry"] = "30"; // Retry every 30 seconds
                parameters["expire"] = "3600"; // Expire after 1 hour
            }
            using var client = new HttpClient();
            return await client.PostAsync("https://api.pushover.net/1/messages.json",
                new FormUrlEncodedContent(parameters));
        }
    }
}
