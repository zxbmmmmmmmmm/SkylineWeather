using System;
using System.Net;

namespace FluentWeather.Abstraction.Models.Exceptions;

[Serializable]
public class HttpResponseException : Exception
{
    public HttpStatusCode Code { get; set; }

    public HttpResponseException(HttpStatusCode code)
    {
        Code = code;
    }
    public HttpResponseException(string message, HttpStatusCode code) : base(message)
    {
        Code = code;
    }
    public HttpResponseException(string message, Exception innerException, HttpStatusCode code) : base(message, innerException)
    {
        Code = code;
    }
}