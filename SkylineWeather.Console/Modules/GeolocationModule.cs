using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Provider.Interfaces;
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
    public async Task RunAsync()
    {
        var config = Program.AppHost.Services.GetRequiredService<IConfiguration>();
        var settings = config.Get<FileSettingsService>();
        Location location;

        if (settings is not null)
        {
            location = settings.DefaultGeolocation.Location;
        }
        else
        {
            var latitude = AnsiConsole.Ask<double>("纬度: ");
            var longitude = AnsiConsole.Ask<double>("经度: ");
            location = new Location(latitude, longitude);
        }
        var result = await _provider.GetGeolocationsAsync(location);
        result.IfSucc(geolocations =>
        {

            AnsiConsole.WriteLine($"数据提供商:{((Abstractions.Provider.ProviderBase)_provider).Name}");
            PrintGeolocations(geolocations);

        });
        var name = AnsiConsole.Ask<string>("名称: ");
        var result1 = await _provider.GetGeolocationsAsync(name);
        result1.IfSucc(geolocations =>
        {
            AnsiConsole.WriteLine($"数据提供商:{((Abstractions.Provider.ProviderBase)_provider).Name}");
            PrintGeolocations(geolocations);
        });


        if (AnsiConsole.Confirm("是否返回？"))
        {
            await _backFunc(string.Empty).ConfigureAwait(false);
        }
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