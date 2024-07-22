using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Interfaces.WeatherProvider;
using FluentWeather.Abstraction.Models;
using FluentWeather.DIContainer;
using FluentWeather.Uwp.Helpers.Analytics;
using FluentWeather.Uwp.Shared;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using Windows.Devices.Geolocation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace FluentWeather.Uwp.Controls.Dialogs;

[ObservableObject]
public sealed partial class TyphoonDialog : ContentDialog
{
    public TyphoonDialog()
    {
        this.InitializeComponent();
        GetTyphoons();
        ShowWarningLines();
        TyphoonMap.MapServiceToken = Constants.BingMapsKey;
        DataContext = this;
        Locator.ServiceProvider.GetService<AppAnalyticsService>()?.TrackTyphoonDialogOpened();
    }
    public const string MapStyleSheetJson = "{\"version\":\"1.*\",\"settings\":{},\"elements\":{\"transportation\":{\"visible\":false},\"road\":{\"labelVisible\":false}}}";
    [ObservableProperty]
    private ObservableCollection<TyphoonTrackBase> _tracks = new();
    [ObservableProperty]
    private ObservableCollection<TyphoonBase> _typhoons = new();
    [ObservableProperty]
    private TyphoonBase _selected;
    public async void GetTyphoons()
    {
        if(Common.Settings.QWeatherDomain is "devapi.qweather.com")
        {
            ServiceWarning.Visibility = Visibility.Visible;
            return;
        }
        var provider = Locator.ServiceProvider.GetService<ITyphoonProvider>();
        var data = await provider.GetActiveTyphoons();
        if(data.Count is 0)
        {
            NoTyphoonInfo.Visibility = Visibility.Visible;
            return;
        }
        data.ForEach(p =>
        {
            Typhoons.Add(p);
            SetMap(p);
        });
        var maxValue = Typhoons.Max(p => p.Now.WindSpeed);
        var max = Typhoons?.Where(p => p.Now.WindSpeed == maxValue).FirstOrDefault();
        SegmentedControl.SelectedItem = max;

        TyphoonMap.ZoomLevel = 6;
    }
    public void SetMap(TyphoonBase typ)
    {
        TyphoonMap.StyleSheet = MapStyleSheet.Combine(new List<MapStyleSheet>
        {
            MapStyleSheet.AerialWithOverlay(),
            MapStyleSheet.ParseFromJson(MapStyleSheetJson)
        });
        var basic = new BasicGeoposition { Latitude = typ.Now.Latitude, Longitude = typ.Now.Longitude };
        var typhoonCenter = new Geopoint(basic);

        typ.History.ForEach(Tracks.Add);
        Tracks.Add(typ.Now);
        typ.Forecast.ForEach(Tracks.Add);

        ShowTrackRoute(typ);
        ShowForecastRoute(typ);
        ShowTyphoon(typ);
    }
    public void MoveToTyphoonCenter(TyphoonBase typhoon)
    {
        var basic = new BasicGeoposition { Latitude = typhoon.Now.Latitude, Longitude = typhoon.Now.Longitude };
        var typhoonCenter = new Geopoint(basic);
        TyphoonMap.Center = typhoonCenter;
    }
    public void ShowTyphoon(TyphoonBase typhoon)
    {
        var now = new BasicGeoposition { Latitude = typhoon.Now.Latitude, Longitude = typhoon.Now.Longitude };
            
        if (((IWindRadius)typhoon.Now).WindRadius7 is { } radius7)
            TyphoonMap.MapElements.Add(GetCircleMapPolygon(now, GetRadius(radius7), Color.FromArgb(64,0,128,255), Color.FromArgb(100, 0, 128, 255)));

        if (((IWindRadius)typhoon.Now).WindRadius10 is { } radius10)
            TyphoonMap.MapElements.Add(GetCircleMapPolygon(now, GetRadius(radius10), Color.FromArgb(32, 255, 255, 0), Colors.Yellow));

        if (((IWindRadius)typhoon.Now).WindRadius12 is { } radius12)
            TyphoonMap.MapElements.Add(GetCircleMapPolygon(now, GetRadius(radius12), Color.FromArgb(16, 255, 0, 0), Colors.Red));
    }
    public double GetRadius(WindRadius radius)
    {
        return (double)(radius.SouthEast + radius.SouthWest + radius.NorthEast + radius.NorthWest) / 4 * 1000;
    }
    public void ShowTrackRoute(TyphoonBase typhoon)
    {
        var endLocation = new BasicGeoposition() { Latitude = typhoon.Now.Latitude, Longitude = typhoon.Now.Longitude };
        var path = new List<BasicGeoposition>();
        foreach (var item in typhoon.History)
        {
            var point = new BasicGeoposition{ Latitude = item.Latitude, Longitude = item.Longitude };
            path.Add(point);
        }
        path.Add(endLocation);
        var  line = new MapPolyline()
        {
            Path = new Geopath(path),
            StrokeThickness = 2,
            StrokeColor = Color.FromArgb(160, 255, 255, 0),
        };
        TyphoonMap.MapElements.Add(line);
    }
    public void ShowForecastRoute(TyphoonBase typhoon)
    {
        var path = new List<BasicGeoposition>
        {
            new ()
            {
                Latitude = typhoon.Now.Latitude, Longitude = typhoon.Now.Longitude
            }
        };
        foreach (var item in typhoon.Forecast)
        {
            var point = new BasicGeoposition{ Latitude = item.Latitude, Longitude = item.Longitude };
            path.Add(point);
        }
        var line = new MapPolyline()
        {
            Path = new Geopath(path),
            StrokeThickness = 3,
            StrokeColor = Color.FromArgb(200, 255, 0, 0),
            StrokeDashed = true
        };
        TyphoonMap.MapElements.Add(line);
    }

    public MapPolygon GetCircleMapPolygon(BasicGeoposition originalLocation, double radius,Color fill,Color stroke)
    {
        MapPolygon retVal = new MapPolygon();

        List<BasicGeoposition> locations = new List<BasicGeoposition>();
        double latitude = originalLocation.Latitude * Math.PI / 180.0;
        double longitude = originalLocation.Longitude * Math.PI / 180.0;
        // double x = radius / 3956; // Miles
        double x = radius / 6371000; // Meters 
        for (int i = 0; i <= 360; i += 3) // <-- you can modify this incremental to adjust the polygon.
        {
            double aRads = i * Math.PI / 180.0;
            double latRadians = Math.Asin(Math.Sin(latitude) * Math.Cos(x) + Math.Cos(latitude) * Math.Sin(x) * Math.Cos(aRads));
            double lngRadians = longitude + Math.Atan2(Math.Sin(aRads) * Math.Sin(x) * Math.Cos(latitude), Math.Cos(x) - Math.Sin(latitude) * Math.Sin(latRadians));

            BasicGeoposition loc = new BasicGeoposition() { Latitude = 180.0 * latRadians / Math.PI, Longitude = 180.0 * lngRadians / Math.PI };
            locations.Add(loc);
        }

        retVal.Path = new Geopath(locations);
        retVal.FillColor = fill;
        retVal.StrokeColor =stroke;
        retVal.StrokeThickness = 2;

        return retVal;
    }

    public void ShowWarningLines()
    {
        var locations24 = new List<BasicGeoposition>
        {
            new (){ Longitude=105,Latitude = 0 },
            new (){ Longitude=113,Latitude = 4.5 },
            new (){ Longitude=119,Latitude = 11 },
            new (){ Longitude=119,Latitude = 18 },
            new (){ Longitude=127,Latitude = 22 },
            new (){ Longitude=127,Latitude = 34 },
        };
        var locations48 = new List<BasicGeoposition>
        {
            new (){ Longitude=105,Latitude = 0 },
            new (){ Longitude=120,Latitude = 0 },
            new (){ Longitude=132,Latitude = 15 },
            new (){ Longitude=132,Latitude = 34 },
        };
        var line24 = new MapPolyline()
        {
            Path = new Geopath(locations24),
            StrokeThickness = 2,
            StrokeColor = Color.FromArgb(150, 255, 255, 0),
        }; var line48 = new MapPolyline()
        {
            Path = new Geopath(locations48),
            StrokeThickness = 2,
            StrokeDashed = true,
            StrokeColor = Color.FromArgb(120, 255, 255, 0),
        };
        var text24 = new TextBlock { TextWrapping = TextWrapping.Wrap,  Text = "24小时警戒线" ,Foreground = new SolidColorBrush(Color.FromArgb(150, 255, 255, 0)) };
        var text48 = new TextBlock { TextWrapping = TextWrapping.Wrap, Text = "48小时警戒线", Foreground = new SolidColorBrush(Color.FromArgb(120, 255, 255, 0)) };

        TyphoonMap.MapElements.Add(line24);
        TyphoonMap.MapElements.Add(line48);
    }
    private readonly Geopoint _point24 = new Geopoint(new BasicGeoposition { Longitude = 127, Latitude = 34 });
    private readonly Geopoint _point48 = new Geopoint(new BasicGeoposition { Longitude = 132, Latitude = 34 });

    [RelayCommand]
    public void Close()
    {
        Hide();
    }

    private void SegmentedControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var items = e.AddedItems;
        MoveToTyphoonCenter(items[0] as TyphoonBase);
    }
    private static Geopoint GetGeoPoint(double lat, double lon)
    {
        return new Geopoint(new BasicGeoposition { Latitude = lat, Longitude = lon });
    }
    private static string TyphoonTypeToDescription(TyphoonType type)
    {
        return type switch
        {
            TyphoonType.TD => "热带气压",
            TyphoonType.TS => "热带风暴",
            TyphoonType.STS => "强热带风暴",
            TyphoonType.TY => "台风",
            TyphoonType.STY => "强台风",
            TyphoonType.SuperTY => "超强台风",
            _ => "未知强度"
        };
    }
}