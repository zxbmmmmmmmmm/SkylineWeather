using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluentWeather.QWeatherApi.Bases;

public abstract class ApiContractBase<TRequest, TResponse>
{
    public abstract HttpMethod Method { get; }
    public abstract string Path { get; }
    public TRequest Request { get; set; }
    public virtual Dictionary<string, string> Cookies { get; } = new();
    public abstract Task<HttpRequestMessage> GenerateRequestMessageAsync(ApiHandlerOption option);
    public abstract Task<TResponse> ProcessResponseAsync(HttpResponseMessage response, ApiHandlerOption option) ;
}