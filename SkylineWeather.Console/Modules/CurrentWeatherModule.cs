using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Provider.Interfaces;
using SkylineWeather.SDK;
using Spectre.Console;
using UnitsNet;

namespace SkylineWeather.Console.Modules;

public class CurrentWeatherModule(
    ICurrentWeatherProvider provider,
    Func<string, Task> backFunc,
    CancellationToken cancellationToken) : IFeatureModule
{
    private readonly ICurrentWeatherProvider _provider = provider;
    private readonly CancellationToken _cancellationToken = cancellationToken;
    private readonly Func<string, Task> _backFunc = backFunc;
    public async Task RunAsync()
    {
        var settings = Program.AppHost.Services.GetService<CommonSettings>();
        Location location;

        if (settings is not null)
        {
            location = settings.DefaultGeolocation!.Location;
        }
        else
        {
            var latitude = AnsiConsole.Ask<double>("纬度: ");
            var longitude = AnsiConsole.Ask<double>("经度: ");
            location = new Location(latitude, longitude);
        }
        var result = await _provider.GetCurrentWeatherAsync(location);
        result.IfSucc(current =>
        {

            AnsiConsole.WriteLine($"经度:{location.Longitude},纬度:{location.Latitude}");

            AnsiConsole.WriteLine($"数据提供商:{((Abstractions.Provider.ProviderBase)_provider).Name}");

            var table = new Table().HideHeaders();
            table.ShowRowSeparators = true;
            table.AddColumn("名称", c => c.NoWrap().LeftAligned());
            table.AddColumn("数据");

            table.AddRow("天气类型", current.WeatherCode.ToString());
            table.AddRow("温度", current.Temperature.ToString());
            AnsiConsole.Write(table);
        });




        if (AnsiConsole.Confirm("是否返回？"))
        {
            await _backFunc(string.Empty).ConfigureAwait(false);
        }
    }
}