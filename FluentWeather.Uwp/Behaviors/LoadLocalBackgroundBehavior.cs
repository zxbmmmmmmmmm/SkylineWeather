using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using FluentWeather.Abstraction.Models;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using Windows.ApplicationModel;
using FluentWeather.Uwp.Helpers;

namespace FluentWeather.Uwp.Behaviors;

public class LoadLocalBackgroundBehavior:Behavior<ImageEx>
{
    public static LoadLocalBackgroundBehavior Instance;
    public LoadLocalBackgroundBehavior()
    {
        Instance = this;
    }
    public WeatherCode WeatherType
    {
        get => (WeatherCode)GetValue(WeatherTypeProperty);
        set => SetValue(WeatherTypeProperty, value);
    }
    public static readonly DependencyProperty WeatherTypeProperty =
        DependencyProperty.Register(nameof(WeatherType), typeof(WeatherCode), 
            typeof(LoadLocalBackgroundBehavior), 
            new PropertyMetadata(WeatherCode.Unknown,WeatherTypeUpdated));
    private static void WeatherTypeUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var b = d as LoadLocalBackgroundBehavior;
        if (b is null) return;
        if (b._weatherType == b.WeatherType) return;//天气类型相同时不更新
        b.LoadImage();
        b._weatherType = b.WeatherType;
    }

    private WeatherCode _weatherType = WeatherCode.Unknown;
    public async void LoadImage()
    {
        var folder = await ApplicationData.Current.LocalFolder.GetOrCreateFolderAsync("Backgrounds");
        var assetsFolder = await Package.Current.InstalledLocation.GetOrCreateFolderAsync("Assets");
        assetsFolder = await assetsFolder.GetOrCreateFolderAsync("Backgrounds");

        var image = await GetImage(folder, WeatherType.ToString());
        image ??= await GetImage(folder, GetImageName(WeatherType));
        image ??= await GetImage(assetsFolder, GetImageName(WeatherType));
        AssociatedObject.Source = image;
    }

    public string GetImageName(WeatherCode weather)
    {
        var code = (int)weather;
        if (code is 0)
            return "Clear";
        if (code is 1 or 2)
            return "PartlyCloudy";
        if (code is 3)
            return "Overcast";
        if (50 <= code && code <= 69 || (80<=code && code <= 82))
            return "Rain";
        if (40 <= code && code <= 49)
            return "Fog";
        if (70 <= code && code <= 79)
            return "Snow";

        return "All";
    }
    
    private async Task<BitmapImage> GetImage(StorageFolder folder, string name)
    {
        var item = await folder.TryGetItemAsync(name + ".png");
        if (item is not StorageFile file) return null;
        //using var ir = await file.OpenAsync(FileAccessMode.Read);
        var image = new BitmapImage(new Uri(file.Path));
        //await image.SetSourceAsync(ir);
        return image;
    }
}