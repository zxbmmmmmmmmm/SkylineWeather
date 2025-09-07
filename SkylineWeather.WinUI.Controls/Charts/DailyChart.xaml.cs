using CommunityToolkit.WinUI;
using LanguageExt;
using LanguageExt.ClassInstances;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Models.Weather;
using SkylineWeather.WinUI.Controls.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using DataPoint = (SkylineWeather.Abstractions.Models.Weather.DailyWeather Data, System.Numerics.Vector2 Point);


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SkylineWeather.WinUI.Controls.Charts
{
    public sealed partial class DailyChart : UserControl
    {
        private readonly Dictionary<WeatherCode, CanvasBitmap> _iconCache = new();
        public DailyChart()
        {
            InitializeComponent();
            RenderCanvas.CreateResources += OnRenderCanvasCreateResources;
            RenderCanvas.Draw += OnRenderCanvasDraw;
            FontSize = 24;
        }

        [GeneratedDependencyProperty]
        public partial IReadOnlyList<DailyWeather> Data { get; set; }

        [GeneratedDependencyProperty(DefaultValue = 3f)]
        public partial float LineStrokeWidth { get; set; }

        [GeneratedDependencyProperty(DefaultValueCallback = nameof(TextColorDefaultValue))]
        public partial Color TextColor { get; set; }
        private static object TextColorDefaultValue() => Colors.White;

        [GeneratedDependencyProperty(DefaultValueCallback = nameof(LineColorDefaultValue))]
        public partial Color LineColor { get; set; }
        private static object LineColorDefaultValue() => Colors.White;

        [GeneratedDependencyProperty(DefaultValueCallback = nameof(FillColorDefaultValue))]
        public partial Color FillColor { get; set; }
        private static object FillColorDefaultValue() => Color.FromArgb(64, 255, 255, 255);

        [GeneratedDependencyProperty]
        public partial Func<WeatherCode, string> IconMapper { get; set; }

        partial void OnPropertyChanged(DependencyPropertyChangedEventArgs e) => RenderCanvas.Invalidate();
        //private void OnRenderCanvasPointerWheelChanged(object sender, PointerRoutedEventArgs e) => e.Handled = true;
        private void OnRenderCanvasCreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args)
        {

        }
        partial void OnDataChanged(IReadOnlyList<DailyWeather> newValue)
        {
            ReloadIconsAsync();
        }
        partial void OnIconMapperChanged(Func<WeatherCode, string> newValue)
        {
            ReloadIconsAsync();
        }

        private async void ReloadIconsAsync()
        {
            if (Data is null || IconMapper is null) return;
            var tasks = Data
                .Select(p => p.WeatherCode).Distinct()
                .Where(p => !_iconCache.ContainsKey(p))
                .ToList()
                .ConvertAll(p => CanvasBitmap.LoadAsync(RenderCanvas, IconMapper(p)).AsTask());
            await Task.WhenAll(tasks);
        }

        private void OnRenderCanvasDraw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (Data == null || !Data.Any()) return;

            var max = Data.Max(p => p.HighTemperature.DegreesCelsius);
            var min = Data.Min(p => p.LowTemperature.DegreesCelsius);
            var gap = max - min;
            var padding = (ThicknessF)Padding;
            var combinedPadding = padding;
            var height = (float)sender.ActualHeight - padding.Top - padding.Bottom;
            var width = (float)sender.ActualWidth - padding.Left - padding.Right;
            var innerHeight = (float)sender.ActualHeight - combinedPadding.Top - combinedPadding.Bottom;
            var innerWidth = (float)sender.ActualWidth - combinedPadding.Left - combinedPadding.Right;
            var count = Data.Count;
            var data = Data;
            args.DrawingSession.Blend = CanvasBlend.Copy;

            // 1.生成点位
            var highPoints = GetPoints(p => p.HighTemperature);
            var lowPoints = GetPoints(p => p.LowTemperature);
            if (highPoints.Count < 2) return;

            var highData = new DrawingData(highPoints, combinedPadding.Left, combinedPadding.Top, innerHeight, innerWidth, innerWidth / count);
            var lowData = new DrawingData(lowPoints, combinedPadding.Left, combinedPadding.Top, innerHeight, innerWidth, innerWidth / count);
            // 2.绘制折线
            DrawLine(sender, args.DrawingSession, highData);
            DrawLine(sender, args.DrawingSession, lowData);

            // 3.绘制点位
            DrawPoints(args.DrawingSession, highPoints, p => p.HighTemperature, -24);
            DrawPoints(args.DrawingSession, lowPoints, p => p.LowTemperature, 24);

            return;

            List<DataPoint> GetPoints(Func<DailyWeather, Temperature> selector)
            {
                var points = new List<DataPoint>(count);
                var horizontalSpacing = innerWidth / (count - 1); // count - 1为间隔
                for (var i = 0; i < count; i++)
                {
                    var item = data.ElementAt(i);
                    var percent = gap == 0 ? 0.5 : (selector(item).DegreesCelsius - min) / gap;
                    var y = innerHeight + combinedPadding.Top - (percent * innerHeight);
                    var x = combinedPadding.Left + i * horizontalSpacing;
                    points.Add((item, new Vector2(x, (float)y)));
                }
                return points;
            }
        }



        private void DrawPoints(
            CanvasDrawingSession session,
            IEnumerable<DataPoint> dataPoints,
            Func<DailyWeather, Temperature> selector,
            float textOffset)
        {
            var fontSize = (float)FontSize;
            var textColor = TextColor;
            foreach (var dataPoint in dataPoints)
            {
                session.DrawCenteredTextLayout(
                    new CanvasTextLayout(session, selector(dataPoint.Data).Value.ToString("0°"),
                        new CanvasTextFormat { FontSize = fontSize }, 100, 20),
                    dataPoint.Point.X, dataPoint.Point.Y + textOffset, textColor); 
                session.FillCircle(dataPoint.Point, 8, Colors.White);
                session.FillCircle(dataPoint.Point, 4, Colors.Transparent);
            }
        }

        private void DrawLine(ICanvasResourceCreator canvasResourceCreator, CanvasDrawingSession session, DrawingData data)
        {
            using (var path = new CanvasPathBuilder(canvasResourceCreator))
            {
                path.BeginFigure(data.DataPoints[0].Point);
                for (var i = 0; i < data.DataPoints.Count; i++)
                {
                    path.AddLine(data.DataPoints[i].Point);
                }
                path.EndFigure(CanvasFigureLoop.Open);
                using var geometry = CanvasGeometry.CreatePath(path);
                session.DrawGeometry(geometry, LineColor, LineStrokeWidth);
            }
        }

        private record DrawingData(List<DataPoint> DataPoints, float X, float Y, float Height, float Width, float spacing);

    }
}
