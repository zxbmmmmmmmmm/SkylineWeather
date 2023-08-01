using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

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
    }
}
