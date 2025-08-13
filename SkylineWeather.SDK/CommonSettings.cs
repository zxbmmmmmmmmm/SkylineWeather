using CommunityToolkit.Mvvm.ComponentModel;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using SkylineWeather.Abstractions.Models.AirQuality;

namespace SkylineWeather.SDK;

public partial class CommonSettings : ObservableObject
{
    [ObservableProperty]
    public partial Geolocation? DefaultGeolocation { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<Geolocation> SavedCities { get; set; } = [];

    [ObservableProperty]
    public partial Dictionary<string, Dictionary<string, object>> ProviderConfigurations { get; set; } = [];

    [ObservableProperty]
    public partial Dictionary<string, string> ProviderMappings { get; set; } = [];

    [ObservableProperty]
    public partial AqiStandard AqiStandard { get; set; } = AqiStandard.Usa;
}
