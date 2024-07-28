using FluentWeather.Abstraction.Interfaces.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;
using System;
using Windows.Storage;
using Windows.UI.Shell;
using AdaptiveCards;
using Windows.Media.Protection.PlayReady;
using Microsoft.Toolkit.Uwp.Helpers;
using FluentWeather.Abstraction.Models;
using System.Text.Json.Serialization;
using Windows.Security.Cryptography.Core;
using FluentWeather.Uwp.Shared.Helpers.ValueConverters;
using Windows.UI.WebUI;
using System.Globalization;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;

namespace FluentWeather.Uwp.Shared.Helpers;

public static class StartMenuCompanionHelper
{
    public static async Task UpdateStartMenuCompanionAsync(this AdaptiveCard adaptiveCard, string fileName = "StartMenuCompanion.json")
    {
        try
        {
            StorageFile file = await StorageFileHelper.WriteTextToLocalFileAsync(adaptiveCard.ToJson(), fileName).ConfigureAwait(false);
            FileInfo info = new(file.Path);
            FileSecurity security = info.GetAccessControl();
            // Add Shell Experience Capability SID
            security.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier("S-1-15-3-1024-3167453650-624722384-889205278-321484983-714554697-3592933102-807660695-1632717421"), FileSystemRights.ReadAndExecute, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow));
            info.SetAccessControl(security);
        }
        catch (Exception ex)
        {
        }
    }


    public static async Task<AdaptiveCard> CreateCompanionCard(WeatherCardData data)
    {
        var card = new AdaptiveCard("1.1")
        {
            Speak = "预报",
            Body =
            [
                new AdaptiveTextBlock
                {
                    Text = data.Location.Name,
                    HorizontalAlignment = AdaptiveHorizontalAlignment.Center,
                    IsSubtle = true,
                    Size = AdaptiveTextSize.Small
                },
                new AdaptiveTextBlock
                {
                    Spacing = AdaptiveSpacing.None,
                    Text = DateTime.Now.ToString(),
                    HorizontalAlignment = AdaptiveHorizontalAlignment.Center,
                    IsSubtle = true,
                    Size = AdaptiveTextSize.Small
                },

                new AdaptiveImage
                {
                    Url = await new Uri("ms-appx:///Assets/Weather/" + data.Current.WeatherType.GetWeatherIconName()).ToBase64Url(),
                    HorizontalAlignment = AdaptiveHorizontalAlignment.Center,
                    PixelHeight = 64,
                    PixelWidth = 64,
                    Spacing = AdaptiveSpacing.None,

                },

                new AdaptiveTextBlock
                {
                    Text = $"{data.Current.Temperature.ConvertTemperatureUnit()}° {data.Current.Description}",
                    HorizontalAlignment = AdaptiveHorizontalAlignment.Center,
                    Size = AdaptiveTextSize.Large,
                    Spacing = AdaptiveSpacing.None,
                    Weight = AdaptiveTextWeight.Bolder,
                },

                new AdaptiveColumnSet
                {
                    Spacing = AdaptiveSpacing.Medium,
                    Columns =
                    [
                        new AdaptiveColumn
                        {
                            Width = AdaptiveColumnWidth.Stretch,
                            Items =
                            [
                                new AdaptiveTextBlock
                                {
                                    Text = data.Current.WindDirectionDescription.Replace("偏",""),
                                    Size = AdaptiveTextSize.Small,
                                    HorizontalAlignment= AdaptiveHorizontalAlignment.Center,
                                    IsSubtle= true,
                                },
                                new AdaptiveTextBlock
                                {
                                    HorizontalAlignment= AdaptiveHorizontalAlignment.Center,
                                    Text = $"{data.Current.WindScale} 级",
                                    Spacing= AdaptiveSpacing.None,
                                }
                            ]
                        },

                        new AdaptiveColumn
                        {
                            Width = AdaptiveColumnWidth.Stretch,
                            Items =
                            [
                                new AdaptiveTextBlock
                                {
                                    Text = "空气质量",
                                    Size = AdaptiveTextSize.Small,
                                    HorizontalAlignment= AdaptiveHorizontalAlignment.Center,
                                    IsSubtle= true,
                                },
                                new AdaptiveTextBlock
                                {
                                    HorizontalAlignment= AdaptiveHorizontalAlignment.Center,
                                    Text = "45 优",
                                    Spacing= AdaptiveSpacing.None,
                                }
                            ]
                        }
                        ]
                },
                await CreateDailyColumnSet(data.Daily[0]),
                await CreateDailyColumnSet(data.Daily[1]),
                await CreateDailyColumnSet(data.Daily[2]),
                await CreateDailyColumnSet(data.Daily[3]),
                await CreateDailyColumnSet(data.Daily[4]),
                await CreateDailyColumnSet(data.Daily[5]),
                await CreateDailyColumnSet(data.Daily[6]),
                new AdaptiveColumnSet(),
                new AdaptiveColumnSet{
                    Spacing = AdaptiveSpacing.Medium,
                    Columns = [
                        new AdaptiveColumn
                    {
                        Width = AdaptiveColumnWidth.Stretch,
                        Items =
                        [
                            new AdaptiveTextBlock
                            {
                                HorizontalAlignment = AdaptiveHorizontalAlignment.Center,
                                Color = AdaptiveTextColor.Accent,
                                Text = "查看更多",
                            }
                        ],

                        SelectAction = new AdaptiveOpenUrlAction { Title = "查看更多", Url = new Uri("weather://")},

                    },],

                }
                ],

        };
        return card;
    }



    public static async Task<AdaptiveColumnSet> CreateDailyColumnSet(WeatherDailyBase daily)
    {
        var set = new AdaptiveColumnSet
        {
            Spacing = AdaptiveSpacing.Large,
            Columns =
            [
                new AdaptiveColumn
                {
                    Width = "8px"
                },
                new AdaptiveColumn
                {
                    Width = AdaptiveColumnWidth.Auto,
                    Items =
                    [
                        new AdaptiveImage
                        {
                            Url = await new Uri("ms-appx:///Assets/Weather/Resized/32/" + daily.WeatherType.GetWeatherIconName()).ToBase64Url(),
                            PixelWidth = 32,
                            PixelHeight = 32,
                        }
                    ]
                },
                new AdaptiveColumn
                {
                    Width = AdaptiveColumnWidth.Stretch,
                    Items =
                    [
                        new AdaptiveTextBlock
                        {
                            Text = $"{daily.Time.GetWeek2()} {daily.Description}",
                            IsSubtle= true,
                        },
                        new AdaptiveTextBlock
                        {
                            Spacing = AdaptiveSpacing.None,
                            Text = $"{daily.MinTemperature.ConvertTemperatureUnit()}° - {daily.MaxTemperature.ConvertTemperatureUnit()}° {daily.WindDirectionDescription.GetShortWindDirectionDescription()} {daily.WindScale}级",
                        }
                    ]
                },
            ]
        };
        return set;
    }
    public static async Task<AdaptiveCard> CreateWidgetCard(WeatherCardData data)
    {
        var card = new AdaptiveCard("1.1")
        {
            Speak = "预报",
           
            Body =
            [
                new AdaptiveColumnSet()
                {
                    Columns =
                    [
                        new AdaptiveColumn()
                        {
                            Width = AdaptiveColumnWidth.Auto,
                            Items =
                            [
                                new AdaptiveImage
                                {
                                    Url = new Uri(data.Daily[0].WeatherType.GetBase64String()),
                                    Size = AdaptiveImageSize.Auto,
                                    AltText = data.Daily[0].Description,

                                    HorizontalAlignment = AdaptiveHorizontalAlignment.Left
                                }
                            ]
                        },
                        new AdaptiveColumn()
                        {
                            Width = AdaptiveColumnWidth.Auto,
                            Items =
                            [
                                new AdaptiveTextBlock
                                {
                                    Text = $"{data.Daily[0].MinTemperature.ConvertTemperatureUnit()}° - {data.Daily[0].MaxTemperature.ConvertTemperatureUnit()}°",
                                    Size= AdaptiveTextSize.Large,
                                    Spacing = AdaptiveSpacing.None,
                                    Weight = AdaptiveTextWeight.Bolder,
                                    HorizontalAlignment= AdaptiveHorizontalAlignment.Left
                                },
                                new AdaptiveTextBlock
                                {
                                    Text = data.Daily[0].Description,
                                    Spacing = AdaptiveSpacing.None,
                                    Weight = AdaptiveTextWeight.Bolder,
                                }
                            ]
                        }
                    ]
                },
                new AdaptiveColumnSet
                {
                    Columns=
                    [
                        await GetDailyForecastColumn(data.Daily[1]),
                        await GetDailyForecastColumn(data.Daily[2]),
                        await GetDailyForecastColumn(data.Daily[3]),
                        await GetDailyForecastColumn(data.Daily[4]),
                        await GetDailyForecastColumn(data.Daily[5]),
                    ]
                },
                new AdaptiveColumnSet
                {
                    Columns =
                    [
                        new AdaptiveColumn
                        {
                            Width = AdaptiveColumnWidth.Stretch,
                            Items =
                            [
                                new AdaptiveTextBlock
                                {
                                    Text = "丽水市,莲都区",
                                    Size = AdaptiveTextSize.Small,
                                },
                                new AdaptiveTextBlock
                                {
                                    Text = "更新于 10分钟前",
                                    Size = AdaptiveTextSize.Small,
                                    Spacing = AdaptiveSpacing.None,
                                    IsSubtle = true
                                }
                            ]
                        },
                        //new AdaptiveColumn
                        //{
                        //    Width= AdaptiveColumnWidth.Stretch,
                        //    Items =
                        //    [
                        //        new AdaptiveActionSet{
                        //            Actions =
                        //            [
                        //                new AdaptiveOpenUrlAction
                        //                {
                        //                    Title = "查看更多",
                        //                    Url=new Uri("weather://"),
                        //                },
                        //            ]
                        //        }
                        //    ]
                        //}
                    ]
                }
            ]
        };

        return card;
    }
    public static async Task<AdaptiveColumn> GetDailyForecastColumn(WeatherDailyBase daily)
    {
        var column = new AdaptiveColumn
        {
            Width = AdaptiveColumnWidth.Stretch,
            Items =
            [
                new AdaptiveTextBlock
                {
                    Text = daily.Time.GetWeek2(),
                    IsSubtle = true,
                    Spacing = AdaptiveSpacing.None,
                    HorizontalAlignment = AdaptiveHorizontalAlignment.Center,
                },
                new AdaptiveImage
                {
                    Url = await new Uri("/Assets/Resized/32/" + daily.WeatherType.GetWeatherIconName()).ToBase64Url(),
                    Size = AdaptiveImageSize.Auto,
                    Spacing = AdaptiveSpacing.None,
                    HorizontalAlignment = AdaptiveHorizontalAlignment.Center,
                },
                new AdaptiveTextBlock
                {
                    Text = daily.MaxTemperature.ConvertTemperatureUnit().ToString(),
                    HorizontalAlignment = AdaptiveHorizontalAlignment.Center,
                    Spacing = AdaptiveSpacing.None,

                },
                new AdaptiveTextBlock
                {
                    Text = daily.MinTemperature.ConvertTemperatureUnit().ToString(),
                    HorizontalAlignment = AdaptiveHorizontalAlignment.Center,
                    Spacing = AdaptiveSpacing.None,
                    IsSubtle = true,
                }
            ]
        };
        return column;
    }

}
    public sealed class WeatherCardData 
{
    [JsonPropertyName("daily")]
    public List<WeatherDailyBase> Daily { get; set; }

    [JsonPropertyName("current")]
    public WeatherNowBase Current { get; set; }

    [JsonPropertyName("location")]
    public GeolocationBase Location { get; set; }
}
