using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TUI.Flights.Infrastructure.Helpers
{
    public static class JsonMapper
    {
        public static IEnumerable<T> ReadFromJson<T>(string jsonFileUrl)
        {
            string jsonData = string.Empty;
            using (WebClient client = new WebClient())
            {
                jsonData = client.DownloadString(jsonFileUrl);
            }

            return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonData);
        }
    }
}
