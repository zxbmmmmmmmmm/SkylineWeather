using LanguageExt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Provider.Interfaces;
using SkylineWeather.Abstractions.Services;
using Spectre.Console;

namespace SkylineWeather.Console.Modules;

public class GeolocationModule(
    IGeolocationProvider provider,
    Func<string, Task> backFunc,
    CancellationToken cancellationToken) : IFeatureModule
{
    private readonly IGeolocationProvider _provider = provider;
    private readonly CancellationToken _cancellationToken = cancellationToken;
    private readonly Func<string, Task> _backFunc = backFunc;
    private ISettingsService _settings = Program.AppHost.Services.GetRequiredService<IConfiguration>().Get<FileSettingsService>();
    public async Task RunAsync()
    {
        var features = Enum.GetValues<GeolocationFeatureType>();
        var featureType = AnsiConsole.Prompt(
            new SelectionPrompt<GeolocationFeatureType>()
                .Title("请选择功能")
                .PageSize(10)
                .MoreChoicesText("更多")
                .AddChoices(features)
                .UseConverter(FeatureToString));

        var geolocations = featureType switch
        {
            GeolocationFeatureType.Search => await Search(),
            GeolocationFeatureType.ReverseSearch => await ReverseSearch(),
            GeolocationFeatureType.Geolocation => await Geolocation(),
            _ => throw new NotSupportedException(),
        };
        AnsiConsole.WriteLine($"数据提供商:{((Abstractions.Provider.ProviderBase)_provider).Name}");
        PrintGeolocations(geolocations);
        SetDefaultLocation(geolocations);

        if (AnsiConsole.Confirm("是否返回？"))
        {
            await _backFunc(string.Empty).ConfigureAwait(false);
        }
    }
    private void SetDefaultLocation(IEnumerable<Geolocation> geolocations)
    {
        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<Geolocation>()
                .Title("设置默认位置")
                .PageSize(10)
                .MoreChoicesText("更多")
                .AddChoices(geolocations)
                .UseConverter(p => p.Name));
        _settings.DefaultGeolocation = selected;
    }
    private async Task<List<Geolocation>> Geolocation()
    {
        throw new NotImplementedException();
    }
    private async Task<List<Geolocation>> Search()
    {
        var name = AnsiConsole.Ask<string>("名称: ");
        var result = await _provider.GetGeolocationsAsync(name);
        var geolocations = new List<Geolocation>();
        result.IfSucc(geo =>
        {
            geolocations = geo;
        });
        return geolocations ?? throw new ResultIsNullException();
    }
    private async Task<List<Geolocation>> ReverseSearch()
    {
        var latitude = AnsiConsole.Ask<double>("纬度: ");
        var longitude = AnsiConsole.Ask<double>("经度: ");
        var location = new Location(latitude, longitude);
        var result = await _provider.GetGeolocationsAsync(location);
        var geolocations = new List<Geolocation>();
        result.IfSucc(geo =>
        {
            geolocations = geo;
        }); 
        return geolocations ?? throw new ResultIsNullException();
    }
    private static string FeatureToString(GeolocationFeatureType feature)
    {
        return feature switch
        {
            GeolocationFeatureType.Geolocation => "定位",
            GeolocationFeatureType.Search => "搜索(名称)",
            GeolocationFeatureType.ReverseSearch => "反向搜索(经纬度)",
            _ => throw new NotSupportedException(),
        };
    }
    private void PrintGeolocations(IEnumerable<Geolocation> geolocations)
    {
        var table = new Table();
        table.AddColumn("名称");
        table.AddColumn("经度");
        table.AddColumn("纬度");
        table.AddColumn("Adm1");
        table.AddColumn("Adm2");
        foreach (var daily in geolocations)
        {
            table.AddRow(
                daily.Name,
                Markup.Escape(daily.Location.Longitude.ToString()),
                Markup.Escape(daily.Location.Latitude.ToString()),
                Markup.Escape(daily.AdmDistrict ?? ""),
                Markup.Escape(daily.AdmDistrict2 ?? ""));
        }
        AnsiConsole.Write(table);
    }
}
public enum GeolocationFeatureType
{
    Search,
    ReverseSearch,
    Geolocation,
}