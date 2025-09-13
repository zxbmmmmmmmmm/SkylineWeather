using Microsoft.Extensions.DependencyInjection;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Provider.Interfaces;
using SkylineWeather.SDK;
using Spectre.Console;

namespace SkylineWeather.Console.Modules;

public class PrecipitationModule(
    IPrecipitationProvider provider,
    Func<string, Task> backFunc,
    CancellationToken cancellationToken) : IFeatureModule
{
    private readonly IPrecipitationProvider _provider = provider;
    private readonly CancellationToken _cancellationToken = cancellationToken;
    private readonly Func<string, Task> _backFunc = backFunc;
    public async Task RunAsync()
    {
        var settings = Program.AppHost.Services.GetService<CommonSettings>();
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
        var result = await _provider.GetPrecipitationAsync(location);
        result.IfSucc(precip =>
        {

            AnsiConsole.WriteLine($"经度:{location.Longitude},纬度:{location.Latitude}");

            AnsiConsole.WriteLine($"数据提供商:{((Abstractions.Provider.ProviderBase)_provider).Name}");

            var table = new Table();
            table.AddColumn("时间");
            table.AddColumn("类型");
            table.AddColumn("降水量(mm)");
            foreach (var daily in precip)
            {
                table.AddRow(
                    daily.Time.ToString("MM/dd HH:MM"),
                    Markup.Escape(daily.Type.ToString()),
                    Markup.Escape(daily.Amount.ToString("0.0")));
            }
            AnsiConsole.Write(table);
        });




        if (AnsiConsole.Confirm("是否返回？"))
        {
            await _backFunc(string.Empty).ConfigureAwait(false);
        }
    }
}