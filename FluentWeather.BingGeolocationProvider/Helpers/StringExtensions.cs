using System;
using System.Text;

namespace FluentWeather.BingGeolocationProvider.Helpers;

public static class StringExtensions
{
    extension(string str)
    {
        public string ReplaceOnce(string oldStr, string newStr)
        {
            var sb = new StringBuilder();
            var span = str.AsSpan();
            var index = span.IndexOf(oldStr.AsSpan(), StringComparison.Ordinal);
            if (index <= -1) return str;

            sb.Append(span.Slice(0, index).ToString())
                .Append(newStr)
                .Append(span.Slice(index + oldStr.Length).ToString());
            return sb.ToString();
        }
    }
}
