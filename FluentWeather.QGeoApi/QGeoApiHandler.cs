using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using FluentWeather.QGeoApi.ApiContracts;

namespace FluentWeather.QGeoApi;

public class QGeoApiHandler
{
    private readonly HttpClientHandler _httpClientHandler =
        new()
        {
            UseCookies = false,
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        };

    public async Task<TResponse> RequestAsync<TResponse>(
        GeolocationApi<TResponse> contract, IQGeolocationRequest request, ApiHandlerOption option) 
    {
        var client = new HttpClient(_httpClientHandler);
        contract.Request = request;
        var response = await client.SendAsync(await contract.GenerateRequestMessageAsync(option));
        return await contract.ProcessResponseAsync(response, option);
    }
}