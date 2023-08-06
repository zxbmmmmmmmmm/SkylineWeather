using System;
using Windows.ApplicationModel.Background;
using Microsoft.Toolkit.Uwp.Notifications;
using FluentWeather.DIContainer;
using FluentWeather.Abstraction.Interfaces.WeatherProvider;
using Microsoft.Extensions.DependencyInjection;
using FluentWeather.Abstraction.Interfaces.Helpers;
using Windows.Storage;
using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;
using Windows.UI.Notifications;
using System.Linq;
using System.Text.Json;
using static FluentWeather.Tasks.NotifyTask;
using static FluentWeather.Uwp.Shared.TileHelper;


namespace FluentWeather.Tasks
{
    public sealed class NotifyTask :IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            //
            // Call asynchronous method(s) using the await keyword.
            //
            //TODO:推送通知
            var settingContainer = ApplicationData.Current.LocalSettings;
            var token = settingContainer.Values["qWeather.Token"].ToString();
            var provider = new QWeatherProvider.QWeatherProvider(token);
            var lat = (double)settingContainer.Values["Latitude"];
            var lon = (double)settingContainer.Values["Longitude"];
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
            var isWarningNotificationEnabled = (bool)settingContainer.Values["IsWarningNotificationEnabled"];
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
        private async Task PushDaily(double lon,double lat)
        {
            var settingContainer = ApplicationData.Current.LocalSettings;
            settingContainer.Values["LastPushedTime"] ??= DateTime.Now.ToLongDateString();

            var isDailyNotificationTileEnabled = (bool)settingContainer.Values["IsDailyNotificationTileEnabled"];
            var isDailyNotificationEnabled = (bool)settingContainer.Values["IsDailyNotificationEnabled"];
            if (!isDailyNotificationEnabled && !isDailyNotificationTileEnabled) return;

            if ((string)settingContainer.Values["LastPushedTime"] == DateTime.Now.ToLongDateString())
                return;

            var daily = await QWeatherProvider.QWeatherProvider.Instance.GetDailyForecasts(lon, lat);
            //推送通知
            var dailyBuilder = new ToastContentBuilder();
            dailyBuilder.AddText($"{daily[0].Description}  最高{((ITemperatureRange)daily[0]).MaxTemperature}°,最低{((ITemperatureRange)daily[0]).MinTemperature}°");
            var group = new AdaptiveGroup();
            GetGroupChildren(group, daily);
            dailyBuilder.AddVisualChild(group);
            dailyBuilder.Content.Visual.BaseUri = new Uri("Assets/Weather/Day/", UriKind.Relative);
            if(isDailyNotificationEnabled)
                dailyBuilder.Show();
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            if(isDailyNotificationTileEnabled)
                updater.Update(new TileNotification(GenerateTileContent(daily).GetXml()));
            
        }





    }
}