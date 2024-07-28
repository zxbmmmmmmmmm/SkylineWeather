﻿using FluentWeather.Abstraction.Models;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using static FluentWeather.Uwp.Shared.Common;
using FluentWeather.Uwp.QWeatherProvider;
using FluentWeather.Abstraction.Interfaces.WeatherProvider;
using Windows.ApplicationModel.Resources;
using TileHelper = FluentWeather.Uwp.Shared.Helpers.TileHelper;
using FluentWeather.Uwp.Shared.Helpers;

namespace FluentWeather.Tasks
{
    public sealed class NotifyTask :IBackgroundTask
    {
        private IWeatherWarningProvider _warningProvider;
        private IDailyForecastProvider _dailyForecastProvider;
        private ICurrentWeatherProvider _currentWeatherProvider;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            LogManager.GetLogger(nameof(NotifyTask)).Info("NotifyTask Started");

            if(Settings.ProviderConfig is Uwp.Shared.ProviderConfig.QWeather)
            {
                var provider = new QWeatherProvider(Settings.QWeatherToken, Settings.QWeatherDomain, null, Settings.QWeatherPublicId);
                _warningProvider = provider;
                _currentWeatherProvider = provider;
                _dailyForecastProvider = provider;
            }
            else
            {
                var provider = new OpenMeteoProvider.OpenMeteoProvider();
                _dailyForecastProvider = provider;
                _currentWeatherProvider = provider;
            }
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
            if (_warningProvider is null) return;
            var settingContainer = ApplicationData.Current.LocalSettings;
            var isWarningNotificationEnabled = Settings.IsWarningNotificationEnabled;
            if (!isWarningNotificationEnabled) return;
            var warnings = await _warningProvider.GetWeatherWarnings(lon, lat);
            settingContainer.Values["PushedWarnings"] ??= JsonSerializer.Serialize(new Dictionary<string,DateTime>());
            var pushed = JsonSerializer.Deserialize<Dictionary<string, DateTime>>((string)settingContainer.Values["PushedWarnings"]);
            foreach (var warning in warnings)
            {               
                if (!Settings.NotificationsDebugMode&&pushed.ContainsKey(warning.Id)) continue; //未被推送过
                if (Settings.IgnoreWarningWords != "" && Regex.IsMatch(warning.Title,Settings.IgnoreWarningWords)) continue;//匹配屏蔽词
                var builder = new ToastContentBuilder()
                    .AddText(warning.Title)
                    .AddText(warning.Description)
                    .AddAttributionText(warning.Sender);
                builder.Show(toast =>
                {
                    toast.ExpirationTime = DateTime.Now.AddHours(16);
                });
                if (Settings.NotificationsDebugMode) continue;
                pushed.Add(warning.Id,warning.PublishTime);
            }
            if (Settings.IsDailyNotificationTileEnabled)
            {

                TileHelper.UpdateWarningTile(warnings);
            }
            TileHelper.UpdateBadge(warnings.Count);
            settingContainer.Values["PushedWarnings"] = JsonSerializer.Serialize(pushed);
        }
        private async Task PushDaily(double lon, double lat)
        {
            var isTileAvailable = true;
            var isPushTodayAvailable = true;
            var isPushTomorrowAvailable = true;
            if (!Settings.NotificationsDebugMode)
            {
                if (_dailyForecastProvider is null) return;
                isPushTodayAvailable = Settings.IsDailyNotificationEnabled && Settings.LastPushedTime != DateTime.Now.Date.DayOfYear;
                isPushTomorrowAvailable = Settings.IsTomorrowNotificationEnabled && Settings.LastPushedTimeTomorrow != DateTime.Now.Date.DayOfYear;
                isTileAvailable = Settings.IsDailyNotificationTileEnabled && Settings.LastPushedTime != DateTime.Now.Date.DayOfYear;
                if (!isPushTodayAvailable && !isPushTomorrowAvailable && !isTileAvailable) return;
            }


            var daily = await _dailyForecastProvider.GetDailyForecasts(lon, lat);
            var current = await _currentWeatherProvider.GetCurrentWeather(lon, lat);
            var info = new WeatherCardData { Current = current, Daily = daily ,Location = Settings.DefaultGeolocation};
            var card = await StartMenuCompanionHelper.CreateCompanionCard(info);
            await card.UpdateStartMenuCompanionAsync();
            if (isTileAvailable)
            {
                TileHelper.UpdateForecastTile(daily);
                LogManager.GetLogger(nameof(NotifyTask)).Info("Tile Updated");
            }
            if (DateTime.Now.Hour < 18)
            {
                if (!isPushTodayAvailable) return;
                PushToday(daily);
                Settings.LastPushedTime = DateTime.Now.Date.DayOfYear;
            }
            else
            {
                if (!isPushTomorrowAvailable) return;
                PushTomorrow(daily);
                Settings.LastPushedTimeTomorrow = DateTime.Now.Date.DayOfYear;
            }
        }

        private void PushToday(List<WeatherDailyBase> data)
        {
            var trimmed = (data.Count >= 7) ? data.GetRange(0, 7) : data;
            //var group = new AdaptiveGroup();
            //TileHelper.GetGroupChildren(group, trimmed);
            var largeGroup = new AdaptiveGroup();
            foreach (var item in trimmed)
            {
                largeGroup.Children.Add(TileHelper.GenerateTileSubgroup(TileHelper.GetWeek(item.Time), $"Assets/Weather/Resized/32/{AssetsHelper.GetWeatherIconName(item.WeatherType)}", item.MaxTemperature, item.MinTemperature));
            }
            var builder = new ToastContentBuilder()
                .AddHeroImage(new Uri("ms-appx:///Assets/Backgrounds/" + AssetsHelper.GetBackgroundImageName(data[0].WeatherType) +".png"))
                .AddAttributionText(ResourceLoader.GetForViewIndependentUse().GetString("ToadyWeather"))
                .AddText($"{trimmed[0].Description}  {ResourceLoader.GetForViewIndependentUse().GetString("HighestTemperature")}{(trimmed[0]).MaxTemperature}°,{ResourceLoader.GetForViewIndependentUse().GetString("LowestTemperature")}{(trimmed[0]).MinTemperature}°")
                .AddVisualChild(largeGroup);
            builder.Show(toast =>
            {
                toast.ExpirationTime = DateTime.Now.AddHours(12);
            });
            LogManager.GetLogger(nameof(NotifyTask)).Info("Notification Pushed(Today)");
        }
        private void PushTomorrow(List<WeatherDailyBase> data)
        {
            var trimmed = (data.Count >= 7) ? data.GetRange(1, 6) : data;
            var largeGroup = new AdaptiveGroup();
            foreach (var item in trimmed)
            {
                largeGroup.Children.Add(TileHelper.GenerateSubgroup(TileHelper.GetWeek(item.Time), $"Assets/Weather/Resized/32/{AssetsHelper.GetWeatherIconName(item.WeatherType)}", item.MaxTemperature, item.MinTemperature));
            }
            var builder = new ToastContentBuilder()
                .AddHeroImage(new Uri("ms-appx:///Assets/Backgrounds/" + AssetsHelper.GetBackgroundImageName(data[0].WeatherType) + ".png"))
                .AddAttributionText(ResourceLoader.GetForViewIndependentUse().GetString("TomorrowWeather"))
                .AddText($"{trimmed[0].Description}  {ResourceLoader.GetForViewIndependentUse().GetString("HighestTemperature")}{(trimmed[0]).MaxTemperature}°,{ResourceLoader.GetForViewIndependentUse().GetString("LowestTemperature")}{(trimmed[0]).MinTemperature}°")
                .AddVisualChild(largeGroup);
            builder.Show(toast =>
            {
                toast.ExpirationTime = DateTime.Now.AddHours(12);
            });
               LogManager.GetLogger(nameof(NotifyTask)).Info("Notification Pushed(Tomorrow)");

        }

    }


}