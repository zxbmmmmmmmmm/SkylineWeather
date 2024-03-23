using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
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
        public static string MD5Encrypt64(this string data)
        {
            var md5 = MD5.Create(); 
            var s = md5.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(s);
        }

        public static string MD5Encrypt32(this string data)
        {
            var md5 = MD5.Create();
            var t = md5.ComputeHash(Encoding.UTF8.GetBytes(data));
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < t.Length; i++)
            {
                stringBuilder.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }
        public static string GetTimeStamp(this DateTime dt)
        {
            var ts = dt - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();//精确到秒
        }
    }
}
