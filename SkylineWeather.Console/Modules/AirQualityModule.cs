using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Models.AirQuality;
using SkylineWeather.Abstractions.Provider.Interfaces;
using SkylineWeather.DataAnalyzer.Models;
using SkylineWeather.SDK;
using Spectre.Console;

namespace SkylineWeather.Console.Modules;

public class AirQualityModule(
    IAirQualityProvider provider,
    Func<string, Task> backFunc,
    CancellationToken cancellationToken) : IFeatureModule
{
    private readonly IAirQualityProvider _provider = provider;
    private readonly CancellationToken _cancellationToken = cancellationToken;
    private readonly Func<string, Task> _backFunc = backFunc;
    public async Task RunAsync()
    {
        var config = Program.AppHost.Services.GetRequiredService<IConfiguration>();
        var settings = config.Get<CommonSettings>();
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
        var result = await _provider.GetCurrentAirQualityAsync(location, cancellationToken);

        result.IfSucc(airQuality =>
        {
            AnsiConsole.WriteLine($"经度:{location.Longitude},纬度:{location.Latitude}");

            AnsiConsole.WriteLine($"数据提供商:{((Abstractions.Provider.ProviderBase)_provider).Name}");

            var table = new Table()
                .Title("污染物")
                .AddColumn("项目")
                .AddColumn("数值")
                .AddRow("PM2.5", airQuality.PM25?.ToString() ?? "N/A")
                .AddRow("PM10", airQuality.PM10?.ToString() ?? "N/A")
                .AddRow("二氧化硫 (SO2)", airQuality.SO2?.ToString() ?? "N/A")
                .AddRow("二氧化氮 (NO2)", airQuality.NO2?.ToString() ?? "N/A")
                .AddRow("臭氧 (O3)", airQuality.O3?.ToString() ?? "N/A")
                .AddRow("一氧化碳 (CO)", airQuality.CO?.ToString() ?? "N/A");
            AnsiConsole.Write(table);

            // 1. 收集所有 AQI 结果
            var aqiResults = new List<(string Name, Aqi Aqi)>();


            var services = Program.AppHost.Services.GetServices<IAqiAnalyzer>();
            foreach (var service in services)
            {
                var aqi = service.CalculateAqi(airQuality);
                aqiResults.Add((aqi.Standard.ToString(), aqi));
            }
            if (airQuality.Aqi is not null)
            {
                aqiResults.Add(($"数据源 ({airQuality.Aqi.Standard})", airQuality.Aqi));
            }

            // 2. 创建转置后的表格
            var aqiTable = new Table().Title("AQI 对比");

            // 3. 添加列标题（项目 + 各个计算标准）
            aqiTable.AddColumn("项目");
            foreach (var (name, _) in aqiResults)
            {
                aqiTable.AddColumn(new TableColumn(name).Centered());
            }

            // 4. 添加行数据
            aqiTable.AddRow(["AQI" ,..aqiResults.Select(r => r.Aqi.Value.ToString(CultureInfo.InvariantCulture)).ToArray()]);
            aqiTable.AddRow(["PM2.5", .. aqiResults.Select(r => r.Aqi.SubAqis.PM25.ToString() ?? "N/A").ToArray()]);
            aqiTable.AddRow(["PM10", .. aqiResults.Select(r => r.Aqi.SubAqis.PM10.ToString() ?? "N/A").ToArray()]);
            aqiTable.AddRow(["SO2", .. aqiResults.Select(r => r.Aqi.SubAqis.SO2.ToString() ?? "N/A").ToArray()]);
            aqiTable.AddRow(["NO2", .. aqiResults.Select(r => r.Aqi.SubAqis.NO2.ToString() ?? "N/A").ToArray()]);
            aqiTable.AddRow(["O3", .. aqiResults.Select(r => r.Aqi.SubAqis.O3.ToString() ?? "N/A").ToArray()]);
            aqiTable.AddRow(["CO", .. aqiResults.Select(r => r.Aqi.SubAqis.CO.ToString() ?? "N/A").ToArray()]);

            AnsiConsole.Write(aqiTable);
        });



        if (AnsiConsole.Confirm("是否返回？"))
        {
            await _backFunc(string.Empty).ConfigureAwait(false);
        }
    }
}