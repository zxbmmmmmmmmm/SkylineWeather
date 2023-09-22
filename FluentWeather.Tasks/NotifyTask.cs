using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Notifications;
using static FluentWeather.Uwp.Shared.TileHelper;
using static FluentWeather.Uwp.Shared.Common;

namespace FluentWeather.Tasks
{
    public sealed class NotifyTask :IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            
            var settingContainer = ApplicationData.Current.LocalSettings;
            var token = settingContainer.Values["qWeather.Token"].ToString();
            var provider = new QWeatherProvider.QWeatherProvider(token,Settings.QWeatherDomain);
            var lat = Settings.Latitude;
            var lon = Settings.Longitude;
            if(lat is -1 || lon is -1)
            {
                deferral.Complete();
                return;
            }

            await PushDaily(lon,lat);
            await PushWarnings(lon, lat);


            deferral.Complete();
        }
        private async Task PushWarnings(double lon, double lat)
        {
            var settingContainer = ApplicationData.Current.LocalSettings;
            var isWarningNotificationEnabled = Settings.IsWarningNotificationEnabled;
            if (!isWarningNotificationEnabled) return;

            var warnings = await QWeatherProvider.QWeatherProvider.Instance.GetWeatherWarnings(lon, lat);
            settingContainer.Values["PushedWarnings"] ??= JsonSerializer.Serialize(new Dictionary<string,DateTime>());
            var pushed = JsonSerializer.Deserialize<Dictionary<string, DateTime>>((string)settingContainer.Values["PushedWarnings"]);
            foreach (var warning in warnings)
            {
                if (!pushed.ContainsKey(warning.Id))//未被推送过
                {
                    var toast = new ToastContentBuilder()
                        .AddText(warning.Title)
                        .AddText(warning.Description)
                        .AddAttributionText(warning.Sender);
                    toast.Show();
                    pushed.Add(warning.Id,warning.PublishTime);
                }
            }
            settingContainer.Values["PushedWarnings"] = JsonSerializer.Serialize(pushed);
        }
        private async Task PushDaily(double lon, double lat)
        {
            var isPushTodayAvailable = Settings.IsDailyNotificationEnabled && Settings.LastPushedTime != DateTime.Now.Date.DayOfYear;
            var isPushTomorrowAvailable = Settings.IsTomorrowNotificationEnabled && Settings.LastPushedTimeTomorrow != DateTime.Now.Date.DayOfYear;
            var isTileAvailable = Settings.IsDailyNotificationTileEnabled && Settings.LastPushedTime != DateTime.Now.Date.DayOfYear;
            if (!isPushTodayAvailable && !isPushTomorrowAvailable && !isTileAvailable) return;

            var data = await QWeatherProvider.QWeatherProvider.Instance.GetDailyForecasts(lon, lat);
            if (isTileAvailable)
            {
                UpdateTiles(data);
            }
            if (DateTime.Now.Hour < 18)
            {
                if (!isPushTodayAvailable) return;
                PushToday(data);
                Settings.LastPushedTime = DateTime.Now.Date.DayOfYear;
            }
            else
            {
                if (!isPushTomorrowAvailable) return;
                PushTomorrow(data);
                Settings.LastPushedTimeTomorrow = DateTime.Now.Date.DayOfYear;
            }
        }
        private void PushTomorrow(List<WeatherBase> data)
        {
            var trimmed = (data.Count >= 7) ? data.GetRange(0, 7) : data;
            var builder = new ToastContentBuilder()
                .AddText("今日天气")
                .AddAttributionText($"{trimmed[0].Description}  最高{((ITemperatureRange)trimmed[0]).MaxTemperature}°,最低{((ITemperatureRange)trimmed[0]).MinTemperature}°");
            builder.Show();
        }
        private void PushToday(List<WeatherBase> data)
        {
            var trimmed = (data.Count >= 7) ? data.GetRange(1, 7) : data;
            var builder = new ToastContentBuilder()
                .AddText("明日天气")
                .AddAttributionText($"{trimmed[0].Description}  最高{((ITemperatureRange)trimmed[0]).MaxTemperature}°,最低{((ITemperatureRange)trimmed[0]).MinTemperature}°");
            builder.Show();
        }
        private void UpdateTiles(List<WeatherBase> data)
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.Update(new TileNotification(GenerateTileContent(data).GetXml()));
        }
    }
}