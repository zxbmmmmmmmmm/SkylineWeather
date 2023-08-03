using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Interfaces.WeatherProvider;
using FluentWeather.Abstraction.Models;
using FluentWeather.DIContainer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace FluentWeather.Uwp.Controls.Dialogs
{
    [ObservableObject]
    public sealed partial class TyphoonDialog : ContentDialog
    {
        public TyphoonDialog()
        {
            this.InitializeComponent();
            GetTyphoons();
        }
        [ObservableProperty]
        private ObservableCollection<TyphoonTrackBase> tracks = new();
        [ObservableProperty]
        private ObservableCollection<TyphoonBase> typhoons = new();
        public async void GetTyphoons()
        {
            var provider = Locator.ServiceProvider.GetService<ITyphoonProvider>();
            var data = await provider.GetActiveTyphoons();
            data.ForEach(p =>
            {
                Typhoons.Add(p);
                SetMap(p);
            });
        }
        public void SetMap(TyphoonBase typ)
        {
            var basic = new BasicGeoposition { Latitude = typ.Now.Latitude, Longitude = typ.Now.Longitude };
            var typhoonCenter = new Geopoint(basic);

            typ.History.ForEach(Tracks.Add);
            Tracks.Add(typ.Now);
            typ.Forecast.ForEach(Tracks.Add);

            TyphoonMap.Center = typhoonCenter;
            TyphoonMap.ZoomLevel = 5;
            ShowTrackRoute(typ);
            ShowForecastRoute(typ);
            ShowTyphoon(typ);
        }
        public void ShowTyphoon(TyphoonBase typhoon)
        {
            var now = new BasicGeoposition { Latitude = typhoon.Now.Latitude, Longitude = typhoon.Now.Longitude };
            
            if (((IWindRadius)typhoon.Now).WindRadius7 is { } radius7)
                TyphoonMap.MapElements.Add(GetCircleMapPolygon(now, GetRadius(radius7), Color.FromArgb(64,0,0,255), Colors.Blue));

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
                StrokeColor = Colors.Yellow,
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
                StrokeColor = Colors.Red,
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
            for (int i = 0; i <= 360; i += 10) // <-- you can modify this incremental to adjust the polygon.
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

        [RelayCommand]
        public void Close()
        {
            Hide();
        }
    }
}
