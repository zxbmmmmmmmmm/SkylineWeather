using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using Windows.UI.Notifications;
using static FluentWeather.Abstraction.Models.WeatherCode;

namespace FluentWeather.Uwp.Shared
{
    public sealed class TileHelper
    {
        public static void UpdateTiles(List<WeatherDailyBase> data)
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.Update(new TileNotification(GenerateTileContent(data).GetXml()));
        }
        public static AdaptiveSubgroup GenerateSubgroup(string day, string img, int tempHi, int tempLo)
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
                        HintAlign = AdaptiveImageAlign.Center,
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

        public static AdaptiveSubgroup GenerateTileSubgroup(string day, string img, int tempHi, int tempLo)
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
                        HintRemoveMargin = true,
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

        public static void GetGroupChildren(AdaptiveGroup group, List<WeatherDailyBase> daily)
        {
            foreach (var item in daily)
            {
                group.Children.Add(GenerateSubgroup(GetWeek(((ITime)item).Time), "Assets/Weather/Resized/32/" + GetImageName(item.WeatherType), ((ITemperatureRange)item).MaxTemperature, ((ITemperatureRange)item).MinTemperature));
            }
        }
        public static void GetGroupChildrenForTile(AdaptiveGroup group, List<WeatherDailyBase> daily)
        {
            foreach (var item in daily)
            {
                group.Children.Add(GenerateTileSubgroup(GetWeek(((ITime)item).Time), GetImageName(item.WeatherType), ((ITemperatureRange)item).MaxTemperature, ((ITemperatureRange)item).MinTemperature));
            }
        }
        public static TileContent GenerateTileContent(List<WeatherDailyBase> daily)
        {
            TileContentBuilder builder = new TileContentBuilder();

            // Small Tile
            builder.AddTile(TileSize.Small)
                .AddAdaptiveTileVisualChild(new AdaptiveImage { Source = "Resized/24/" + GetImageName(daily[0].WeatherType), HintAlign = AdaptiveImageAlign.Center })
                .SetTextStacking(TileTextStacking.Center);


            //var mediumGroup = new AdaptiveGroup();
            //GetGroupChildren(mediumGroup, daily.GetRange(0, 2));

            //// Medium Tile
            //builder.AddTile(TileSize.Medium)
            //    .AddAdaptiveTileVisualChild(mediumGroup, TileSize.Medium);

            builder.AddTile(TileSize.Medium)
                .AddText("")
                .AddAdaptiveTileVisualChild(new AdaptiveImage { Source = "Resized/32/" + GetImageName(daily[0].WeatherType), HintAlign = AdaptiveImageAlign.Center })
                .SetTextStacking(TileTextStacking.Center)
                .SetBranding(TileBranding.Name)
                .SetDisplayName(daily[0].Description, TileSize.Medium);

            var wideGroup = new AdaptiveGroup();
            GetGroupChildrenForTile(wideGroup, daily.GetRange(0, 5));

            // Wide Tile
            builder.AddTile(TileSize.Wide)
                .AddAdaptiveTileVisualChild(wideGroup, TileSize.Wide)
                .SetBranding(TileBranding.None, TileSize.Wide)
                .SetTextStacking(TileTextStacking.Center);

            // Large tile
            builder.AddTile(TileSize.Large, GenerateLargeTileContent(daily))
                .SetBranding(TileBranding.Name)
                .SetTextStacking(TileTextStacking.Center);


            // Set the base URI for the images, so we don't redundantly specify the entire path
            builder.Content.Visual.BaseUri = new Uri("Assets/Weather/", UriKind.Relative);

            builder.Content.Visual.LockDetailedStatus1 = daily[0].Description;
            builder.Content.Visual.LockDetailedStatus2 = $"{((ITemperatureRange)daily[0]).MinTemperature}° / {((ITemperatureRange)daily[0]).MaxTemperature}° {((IWind)daily[0]).WindDirectionDescription} {((IWind)daily[0]).WindScale}级";


            return builder.Content;
        }
        public static TileBindingContentAdaptive GenerateLargeTileContent(List<WeatherDailyBase> daily)
        {
            var content = new TileBindingContentAdaptive()
            {
                Children =
                {
            new AdaptiveGroup()
            {
                Children =
                {
                    new AdaptiveSubgroup()
                    {
                        HintWeight = 30,
                        Children =
                        {
                            new AdaptiveImage() { Source = $"{GetImageName(daily[0].WeatherType)}" }
                        }
                    },

                    new AdaptiveSubgroup()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = daily[0].Description,
                                HintStyle = AdaptiveTextStyle.Subtitle

                            },

                            new AdaptiveText()
                            {
                                Text = ((ITemperatureRange)daily[0]).MaxTemperature + "° / " +((ITemperatureRange)daily[0]).MinTemperature + "°"
                            },


                            new AdaptiveText()
                            {
                                Text = ((IWind)daily[0]).WindDirectionDescription  + ((IWind)daily[0]).WindSpeed + "km/h",
                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                            }
                        }
                    }
                }
            },

            // For spacing
            new AdaptiveText(),


            }
            };
            var largeGroup = new AdaptiveGroup();
            foreach (var item in daily.GetRange(1, 5))
            {
                largeGroup.Children.Add(GenerateLargeSubgroup(GetWeek(((ITime)item).Time), $"{GetImageName(item.WeatherType)}", ((ITemperatureRange)item).MaxTemperature, ((ITemperatureRange)item).MinTemperature));
            }
            content.Children.Add(largeGroup);
            return content;
        }
        public static AdaptiveSubgroup GenerateLargeSubgroup(string day, string image, int high, int low)
        {
            // Generate the normal subgroup
            var subgroup = GenerateTileSubgroup(day, image, high, low);

            // Allow there to be padding around the image
            (subgroup.Children[1] as AdaptiveImage).HintRemoveMargin = null;

            return subgroup;
        }
        public static string GetWeek(DateTime date)
        {
            if (date.Day == DateTime.Today.Day)
                return "今天";
            if (date.Day == DateTime.Today.Day + 1)
                return "明天";

            return CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(date.DayOfWeek).Replace("星期", "周");
        }
        public static string GetImageName(WeatherCode weatherType)
        {
            return weatherType switch
            {
                SlightHail or ModerateOrHeavyHail => "BlowingHail.png",
                HeavyRain => "HeavyRain.png",
                ModerateRain => "LightRain.png",
                SlightRain => "LightRain.png",
                ModerateRainShowers => "LightRain.png",
                PartlyCloudy => "Cloudy.png",
                Overcast => "VeryCloudy.png",
                SlightSnowFall => "LightSnow.png",
                HeavySnowFall => "HeavySnow.png",
                Fog => "Fog.png",
                LightFreezingRain or HeavyFreezingRain => "FreezingRain.png",
                SlightSleet or ModerateOrHeavySleet => "RainSnow.png",
                ThunderstormWithHeavyHail or SlightOrModerateThunderstorm or ThunderstormWithSlightHail or HeavyThunderStorm => "Thundery.png",
                Clear => "SunnyDay.png",
                Haze => "HazeSmokeDay.png",
                MainlyClear => "MostlySunnyDay.png",
                ViolentRainShowers or ViolentRainShowers or SlightRainShowers => "RainShowersDay.png",
                SlightSnowShowers or HeavySnowShowers => "SnowShowersDay.png",
                _ => "PartlyCloudy.png",
            };
        }

    }
}
