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


            await PushDaily(lon,lat);
            await PushWarnings(116.3, 39.9);


            
            deferral.Complete();
        }
        private async Task PushWarnings(double lon, double lat)
        {
            var warnings = await QWeatherProvider.QWeatherProvider.Instance.GetWeatherWarnings(lon, lat);
            foreach (var warning in warnings)
            {
                var toast = new ToastContentBuilder()
                    .AddText(warning.Title)
                    .AddText(warning.Description)
                    .AddAttributionText(warning.Sender);
                toast.Show();
            }
        }
        private async Task PushDaily(double lon,double lat)
        {
            var daily = await QWeatherProvider.QWeatherProvider.Instance.GetDailyForecasts(lon, lat);
            var dailyToast = new ToastContentBuilder();
            ToastContentBuilder dailyBuilder = new ToastContentBuilder();
            dailyBuilder.AddText($"{daily[0].Description}  最高{((ITemperatureRange)daily[0]).MaxTemperature}°,最低{((ITemperatureRange)daily[0]).MinTemperature}°");
            var adapGroup = new AdaptiveGroup();
            foreach (var item in daily)
            {
                adapGroup.Children.Add(GenerateSubgroup(GetWeek(((ITime)item).Time), $"ms-appx:///Assets/Weather/Day/{GetImageName(item.WeatherType)}", ((ITemperatureRange)item).MaxTemperature, ((ITemperatureRange)item).MinTemperature));
            }
            dailyBuilder.AddVisualChild(adapGroup);
            dailyBuilder.Show();
        }

        private string GetWeek(DateTime date)
        {
            if (date.Day == DateTime.Today.Day)
                return "今天";
            if (date.Day == DateTime.Today.Day + 1)
                return "明天";

            return CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(date.DayOfWeek).Replace("星期", "周");
        }
        private string GetImageName(WeatherType weatherType)
        {
            switch (weatherType)
            {
                case WeatherType.Clear:
                    return "Clear.png";
                case WeatherType.Hail:
                    return "Hail.png";
                case WeatherType.PartlyCloudy:
                    return "PartlyCloudy.png";
                case WeatherType.HeavyRain:
                case WeatherType.LightRain:
                    return "Rain.png";
                case WeatherType.Cloudy:
                case WeatherType.VeryCloudy:
                    return "Cloudy.png";
                case WeatherType.Fog:
                    return "Fog.png";
                case WeatherType.HeavyShowers:
                case WeatherType.LightShowers:
                    return "Showers.png";
                case WeatherType.LightSnow:
                case WeatherType.HeavySnow:
                case WeatherType.LightSnowShowers:
                case WeatherType.HeavySnowShowers:
                    return "Snow.png";
                case WeatherType.LightSleet:
                case WeatherType.LightSleetShowers:
                    return "Sleet.png";
                case WeatherType.ThunderyHeavyRain:
                case WeatherType.ThunderyShowers:
                case WeatherType.ThunderySnowShowers:
                    return "Thundery.png";
                default:
                    return "PartlyCloudy.png";
            }
        }
        private static AdaptiveSubgroup GenerateSubgroup(string day, string img, int tempHi, int tempLo)
        {
            return new AdaptiveSubgroup()
            {
                HintWeight = 1,
                

                Children =
                {
                    // Day
                    new AdaptiveText()
                    {
                        Text = day,
                        HintAlign = AdaptiveTextAlign.Center,
                    },

                    // Image
                    new AdaptiveImage()
                    {
                        Source = img,
                        HintRemoveMargin = false,
                        
                    },

                    // High temp
                    new AdaptiveText()
                    {
                        Text = tempHi + "°",
                        HintAlign = AdaptiveTextAlign.Center
                    },

                    // Low temp
                    new AdaptiveText()
                    {
                        Text = tempLo + "°",
                        HintAlign = AdaptiveTextAlign.Center,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    }

                }
            };
        }

    }
}