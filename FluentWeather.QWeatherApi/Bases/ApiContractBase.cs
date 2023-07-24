using System.Net.Http;
using System.Threading.Tasks;

namespace FluentWeather.QWeatherApi.Bases;

public abstract class ApiContractBase<TResponse>
{
    public abstract HttpMethod Method { get; }
    public abstract string Url { get; }
}