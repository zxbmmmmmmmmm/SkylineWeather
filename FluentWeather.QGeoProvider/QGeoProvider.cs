using FluentWeather.Abstraction;
using FluentWeather.Abstraction.Interfaces.GeolocationProvider;
using FluentWeather.Abstraction.Interfaces.Helpers;
using FluentWeather.Abstraction.Interfaces.Setting;
using FluentWeather.Abstraction.Models;
using FluentWeather.DIContainer;
using FluentWeather.QGeoApi;
using FluentWeather.QGeoApi.ApiContracts;
using FluentWeather.QGeoProvider.Mappers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FluentWeather.QGeoProvider
{
    public class QGeoProvider : ProviderBase,IGeolocationProvider,ISetting
    {
        public ApiHandlerOption Option { get; set; } = new();

        public Enum Settings => new QGeoSettings();
        public QGeoProvider()
        {
            GetSettings();
        }
        public override string Id => "qgeoapi";
        public override string Name => "和风天气地理服务";
        public void GetSettings()
        {
            var settingsHelper = Locator.ServiceProvider.GetService<ISettingsHelper>();
            Option.Token = settingsHelper?.ReadLocalSetting(Id + "." + QGeoSettings.Token, "");
            var language = settingsHelper?.ReadLocalSetting<string>(QGeoSettings.Language.ToString(), null);
            if (language is null) return;
            if (language.Contains("-") && !language.Contains("zh"))
            {
                Option.Language = language.Remove(language.IndexOf("-", StringComparison.Ordinal));
            }
            else
            {
                Option.Language = language;
            }
        }
        public async Task<List<GeolocationBase>> GetCitiesGeolocationByName(string name)
        {
            var result = await RequestAsync(new GeolocationApi<QGeolocationResponse>(), new QGeolocationRequestByName{Name = name});
            return result.Locations?.ConvertAll(p => (GeolocationBase)p.MapToQGeolocation());
        }
        public async Task<List<GeolocationBase>> GetCitiesGeolocationByLocation(double lat,double lon)
        {
            var result = await RequestAsync(new GeolocationApi<QGeolocationResponse>(), new QGeolocationRequestByLocation {Lat = lat,Lon = lon });
            return result.Locations.ConvertAll(p => (GeolocationBase)p.MapToQGeolocation());
        }
        public async Task<TResponse> RequestAsync<TResponse>(
            GeolocationApi<TResponse> contract, IQGeolocationRequest request)
        {
            var handler = new QGeoApiHandler();
            return await handler.RequestAsync(contract, request, Option);
        }
    }
    public enum QGeoSettings
    {
        Token,
        Language
    }
}