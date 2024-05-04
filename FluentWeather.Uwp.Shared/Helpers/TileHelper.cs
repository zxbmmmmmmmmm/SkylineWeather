using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.ApplicationModel.Resources;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using FluentWeather.Abstraction.Models;
using Microsoft.Toolkit.Uwp.Notifications;

namespace FluentWeather.Uwp.Shared.Helpers
{
    public static class TileHelper
    {

        public static void UpdateBadge(int value)
        {
            var badgeXml =
                BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);
            var badgeElement = badgeXml.SelectSingleNode("/badge") as XmlElement;
            badgeElement!.SetAttribute("value", value.ToString());
            var badge = new BadgeNotification(badgeXml);
            var badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
            badgeUpdater.Update(badge);
        }

        public static void UpdateForecastTile(List<WeatherDailyBase> data)
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Update(new TileNotification(GenerateForecastTileContent(data).GetXml()) { Tag = "forecast" });
        }
        public static void UpdateWarningTile(List<WeatherWarningBase> data)
        {
            if (data.Count == 0) return;
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Update(new TileNotification(GenerateWarningsTileContent(data).GetXml()) { Tag = "warnings" });
        }

        public static TileContent GenerateWarningsTileContent(List<WeatherWarningBase> warnings)
        {
            TileContentBuilder builder = new TileContentBuilder();

            //var description = string.Format("{0}条预警", warnings.Count.ToString());
            builder.AddTile(TileSize.Medium)
                .SetTextStacking(TileTextStacking.Top)
                .SetDisplayName("ms-resource:Warning", TileSize.Medium)
                .SetBranding(TileBranding.Auto, TileSize.Medium);

            // Wide Tile
            builder.AddTile(TileSize.Wide)
                //.AddText(description, size: TileSize.Wide, hintWrap: true, hintStyle: AdaptiveTextStyle.CaptionSubtle, hintMaxLines: 2)
                .SetTextStacking(TileTextStacking.Center)
                .SetDisplayName("ms-resource:Warning", TileSize.Wide)
                .SetBranding(TileBranding.Auto, TileSize.Wide);

            // Large tile
            builder.AddTile(TileSize.Large)
                .SetTextStacking(TileTextStacking.Top)
                .SetBranding(TileBranding.Name)
                .SetDisplayName("ms-resource:Warning", TileSize.Large);

            foreach (var item in warnings)
            {
                builder.AddText(item.ShortTitle, size: TileSize.Medium, hintStyle: AdaptiveTextStyle.Caption, hintWrap: true, hintMaxLines: 2);
                builder.AddText(item.ShortTitle, size: TileSize.Wide, hintStyle: AdaptiveTextStyle.Body, hintWrap: true, hintMaxLines: 2);
                if(warnings.Count == 1)
                {
                    builder.AddText(item.Description, size: TileSize.Wide, hintStyle: AdaptiveTextStyle.CaptionSubtle, hintWrap: true, hintMaxLines: 3);
                }   
                builder.AddText(item.ShortTitle, size: TileSize.Large, hintStyle: AdaptiveTextStyle.Base);
                if(warnings.Count == 1)
                {
                    builder.AddText(item.Description, size: TileSize.Large, hintStyle: AdaptiveTextStyle.CaptionSubtle, hintWrap: true, hintMaxLines: 10);
                }
                else
                {
                    builder.AddText(item.Description, size: TileSize.Large, hintStyle: AdaptiveTextStyle.CaptionSubtle, hintWrap: true, hintMaxLines: 3);
                }
                builder.AddText("", size: TileSize.Large, hintStyle: AdaptiveTextStyle.CaptionSubtle);
            }
            return builder.Content;
        }
        public static TileContent GenerateForecastTileContent(List<WeatherDailyBase> daily)
        {
            TileContentBuilder builder = new TileContentBuilder();

            // Small Tile
            builder.AddTile(TileSize.Small)
                .AddAdaptiveTileVisualChild(new AdaptiveImage { Source = "Assets/Weather/Resized/24/" + AssetsHelper.GetWeatherIconName(daily[0].WeatherType), HintAlign = AdaptiveImageAlign.Center })
                .SetTextStacking(TileTextStacking.Center);


            //var mediumGroup = new AdaptiveGroup();
            //GetGroupChildren(mediumGroup, daily.GetRange(0, 2));

            //// Medium Tile
            //builder.AddTile(TileSize.Medium)
            //    .AddAdaptiveTileVisualChild(mediumGroup, TileSize.Medium);
            builder.AddTile(TileSize.Medium)
                .AddText("")
                .AddAdaptiveTileVisualChild(new AdaptiveImage { Source = "Assets/Weather/Resized/32/" + AssetsHelper.GetWeatherIconName(daily[0].WeatherType), HintAlign = AdaptiveImageAlign.Center })
                .SetTextStacking(TileTextStacking.Center)
                .SetBranding(TileBranding.Name)
                .SetDisplayName(daily[0].Description, TileSize.Medium);

            // Wide Tile
            builder.AddTile(TileSize.Wide)
                .AddText($"{daily[0].Description}", TileSize.Wide, AdaptiveTextStyle.Title)
                .AddText($"{daily[0].MaxTemperature}° / {daily[0].MinTemperature}° {daily[0].WindDirectionDescription} {daily[0].WindScale}{ResourceLoader.GetForViewIndependentUse().GetString("Level")}", TileSize.Wide, AdaptiveTextStyle.Body)
                .AddText($"{ResourceLoader.GetForViewIndependentUse().GetString("Humidity")}:{daily[0].Humidity}% {ResourceLoader.GetForViewIndependentUse().GetString("Pressure")}:{daily[0].Pressure}hPa", TileSize.Wide, AdaptiveTextStyle.CaptionSubtle)
                .SetBranding(TileBranding.Auto, TileSize.Wide)
                .SetTextStacking(TileTextStacking.Center);

            // Large tile
            builder.AddTile(TileSize.Large, GenerateLargeTileContent(daily))

                .SetBranding(TileBranding.Name)
                .SetTextStacking(TileTextStacking.Center);

            if (!Common.Settings.TransparentTiles)
            {
                builder.SetBackgroundImage(new Uri("Assets/Backgrounds/" + AssetsHelper.GetBackgroundImageName(daily[0].WeatherType) + ".png", UriKind.Relative), TileSize.Large, hintOverlay: 40)
                    .SetBackgroundImage(new Uri("Assets/Backgrounds/" + AssetsHelper.GetBackgroundImageName(daily[0].WeatherType) + ".png", UriKind.Relative), TileSize.Wide, hintOverlay: 40);

            }

            // Set the base URI for the images, so we don't redundantly specify the entire path
            //builder.Content.Visual.BaseUri = new Uri("Assets/", UriKind.Relative);

            builder.Content.Visual.LockDetailedStatus1 = daily[0].Description;
            builder.Content.Visual.LockDetailedStatus2 = $"{daily[0].MaxTemperature}° / {daily[0].MinTemperature}° {daily[0].WindDirectionDescription} {daily[0].WindScale}{ResourceLoader.GetForViewIndependentUse().GetString("Level")}";


            return builder.Content;
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
                group.Children.Add(GenerateSubgroup(GetWeek(item.Time), "ms-appx:///Assets/Weather/Resized/32" + AssetsHelper.GetWeatherIconName(item.WeatherType), item.MaxTemperature, item.MinTemperature));
            }
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
                                HintWeight = 31,
                                Children =
                                {
                                    new AdaptiveImage() { Source = $"Assets/Weather/{AssetsHelper.GetWeatherIconName(daily[0].WeatherType)}" }
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
                                        Text = (daily[0]).MaxTemperature + "° / " +daily[0].MinTemperature + "° " +(daily[0]).WindDirectionDescription  + (daily[0]).WindScale + ResourceLoader.GetForViewIndependentUse().GetString("Level")
                                    },
                                },
                                HintTextStacking = AdaptiveSubgroupTextStacking.Center,
                            }
                        }
                    },

                    // For spacing
                    new AdaptiveText(),


                }
            };
            var largeGroup = new AdaptiveGroup();
            foreach (var item in daily.GetRange(1,5))
            {                
                largeGroup.Children.Add(GenerateTileSubgroup(GetWeek(item.Time), $"Assets/Weather/Resized/32/{AssetsHelper.GetWeatherIconName(item.WeatherType)}", item.MaxTemperature, item.MinTemperature));
            }
            content.Children.Add(largeGroup);
            return content;
        }


        public static string GetWeek(DateTime date)
        {
            if (date.Day == DateTime.Today.Day)
                return "ms-resource:Today";
            if (date.Day == DateTime.Today.Day + 1)
                return "ms-resource:Tomorrow";

            return CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(date.DayOfWeek).Replace("星期", "周");
        }

    }
}
