using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Provider.Interfaces;
using Spectre.Console;

namespace SkylineWeather.Console.Modules;

public class HourlyWeatherModule(
    IHourlyWeatherProvider provider,
    Func<string, Task> backFunc,
    CancellationToken cancellationToken) : IFeatureModule
{
    private readonly IHourlyWeatherProvider _provider = provider;
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
        var result = await _provider.GetHourlyWeatherAsync(location);

        result.IfSucc(forecasts =>
        {
            AnsiConsole.WriteLine($"经度:{location.Longitude},纬度:{location.Latitude}");

            AnsiConsole.WriteLine($"数据提供商:{((Abstractions.Provider.ProviderBase)_provider).Name}");

            var dailyTable = new Table();
            dailyTable.AddColumn("时间");
            dailyTable.AddColumn("天气类型");
            dailyTable.AddColumn("温度");
            foreach (var daily in forecasts)
            {
                dailyTable.AddRow(
                    daily.Time.ToString("MM/dd HH:MM"),
                    Markup.Escape(daily.WeatherCode.ToString()),
                    Markup.Escape(daily.Temperature.ToString("0.0")));
            }
            AnsiConsole.Write(dailyTable);
        });



        if (AnsiConsole.Confirm("是否返回？"))
        {
            await _backFunc(string.Empty).ConfigureAwait(false);
        }
    }
}