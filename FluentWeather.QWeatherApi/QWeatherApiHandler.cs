using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentWeather.QWeatherApi.Bases;

namespace FluentWeather.QWeatherApi;

public class QWeatherApiHandler
{
    private readonly HttpClientHandler _httpClientHandler =
        new()
        {
            UseCookies = false,
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        };

    public async Task<TResponse> RequestAsync<TRequest, TResponse>(
        ApiContractBase<TRequest, TResponse> contract, TRequest request,ApiHandlerOption option)
    {
        var client = new HttpClient(_httpClientHandler);
        contract.Request = request;
        var response = await client.SendAsync(await contract.GenerateRequestMessageAsync(option));
        return await contract.ProcessResponseAsync(response, option);
    }
}