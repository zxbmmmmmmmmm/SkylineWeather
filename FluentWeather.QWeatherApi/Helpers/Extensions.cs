using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace FluentWeather.QWeatherApi.Helpers
{
    public static class Extensions
    {
        public static HttpRequestMessage AddQuery(this HttpRequestMessage message, string query)
        {
            var uri = message.RequestUri.ToString();
            message.RequestUri = new Uri(uri += query);
            return message;
        }
        public static NameValueCollection Sort(this NameValueCollection collection)
        {
            var orderedKeys = collection.Cast<string>().Where(k => k != null).OrderBy(k => k);
            var newCollection = HttpUtility.ParseQueryString(string.Empty);
            foreach (var key in orderedKeys)
            {
                foreach (var val in collection.GetValues(key)!.Select(x => x).OrderBy(x => x).ToArray())
                {
                    newCollection.Add(key, val);
                }
            }
            return newCollection;
        }
    }
}
