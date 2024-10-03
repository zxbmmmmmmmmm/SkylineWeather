using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Provider.Interfaces;
using Spectre.Console;

namespace SkylineWeather.Console.Modules;

public class AlertModule(
    IAlertProvider provider,
    Func<string, Task> backFunc,
    CancellationToken cancellationToken) : IFeatureModule
{
    private readonly IAlertProvider _provider = provider;
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
        var result = await _provider.GetAlertsAsync(location);

        result.IfSucc(alerts =>
        {
            AnsiConsole.WriteLine($"经度:{location.Longitude},纬度:{location.Latitude}");

            AnsiConsole.WriteLine($"数据提供商:{((Abstractions.Provider.ProviderBase)_provider).Name}");

            if (alerts.Count is 0)
            {
                AnsiConsole.WriteLine("无预警");
            }
            else
            {
                foreach (var alert in alerts)
                {
                    var table = new Table
                    {
                        Title = new TableTitle(alert.Title),
                        ShowHeaders = false,
                        ShowRowSeparators = true,
                    };

                    table.AddColumn("名称");
                    table.AddColumn("内容");
                    table.AddRow("内容", alert.Description);
                    if (alert.Sender is not null)
                        table.AddRow("发布者", alert.Sender);
                    if (alert.PublishTime is not null)
                        table.AddRow("发布时间", alert.PublishTime.Value.ToString());
                    if (alert.StartTime is not null)
                        table.AddRow("开始时间", alert.StartTime.Value.ToString());
                    if (alert.EndTime is not null)
                        table.AddRow("结束时间", alert.EndTime.Value.ToString());


                    AnsiConsole.Write(table);
                    AnsiConsole.WriteLine();
                }

            }

        });



        if (AnsiConsole.Confirm("是否返回？"))
        {
            await _backFunc(string.Empty).ConfigureAwait(false);
        }
    }
}