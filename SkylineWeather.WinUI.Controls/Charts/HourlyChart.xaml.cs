using CommunityToolkit.WinUI;
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;
using UnitsNet.Units;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DataPoint = (SkylineWeather.Abstractions.Models.Weather.HourlyWeather Data,System.Numerics.Vector2 Point);

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SkylineWeather.WinUI.Controls.Charts
{
    public sealed partial class HourlyChart : UserControl
    {

        private readonly ThicknessF InnerPadding = new(12, 12, 12, 72);
        private readonly Dictionary<WeatherCode, CanvasBitmap> _iconCache = new();

        public HourlyChart()
        {
            InitializeComponent();
            RenderCanvas.CreateResources += OnRenderCanvasCreateResources;
            RenderCanvas.Draw += OnRenderCanvasDraw;
            //RenderCanvas.PointerWheelChanged += OnRenderCanvasPointerWheelChanged;
        }


        [GeneratedDependencyProperty]
        public partial IReadOnlyList<HourlyWeather> Data { get; set; }

        [GeneratedDependencyProperty(DefaultValue = 3f)]
        public partial float LineStrokeWidth { get; set; }

        [GeneratedDependencyProperty(DefaultValue = 0.5f)]
        public partial float GridStrokeWidth { get; set; }


        [GeneratedDependencyProperty(DefaultValueCallback = nameof(HorizontalGridLineColorDefaultValue))]
        public partial Color HorizontalGridLineColor { get; set; }       
        private static object HorizontalGridLineColorDefaultValue() => Color.FromArgb(32, 255, 255, 255);

        [GeneratedDependencyProperty(DefaultValueCallback = nameof(HorizontalGridLineDashStyleDefaultValue))]
        public partial float[] HorizontalGridLineDashStyle { get; set; }
        private static object HorizontalGridLineDashStyleDefaultValue() => new float[] { 8, 8 };


        [GeneratedDependencyProperty(DefaultValueCallback = nameof(VerticalGridLineColorDefaultValue))]
        public partial Color VerticalGridLineColor { get; set; }
        private static object VerticalGridLineColorDefaultValue() => Color.FromArgb(96, 255, 255, 255);

        [GeneratedDependencyProperty(DefaultValueCallback = nameof(VerticalGridLineDashStyleDefaultValue))]
        public partial float[] VerticalGridLineDashStyle { get; set; }
        private static object VerticalGridLineDashStyleDefaultValue() => new float[] { 6, 4, 2 ,4 };


        [GeneratedDependencyProperty(DefaultValueCallback = nameof(TextColorDefaultValue))]
        public partial Color TextColor { get; set; }
        private static object TextColorDefaultValue() => Color.FromArgb(128, 255, 255, 255);

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
        partial void OnDataChanged(IReadOnlyList<HourlyWeather> newValue)
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

            var max = Data.Max(p => p.Temperature.DegreesCelsius);
            var min = Data.Min(p => p.Temperature.DegreesCelsius);
            var gap = max - min;
            var padding = new ThicknessF
            {
                Bottom = (float)Padding.Bottom,
                Left = (float)Padding.Left,
                Right = (float)Padding.Right,
                Top = (float)Padding.Top
            };
            var combinedPadding = InnerPadding + padding;
            var height = (float)sender.ActualHeight - padding.Top - padding.Bottom;
            var width = (float)sender.ActualWidth - padding.Left - padding.Right;
            var innerHeight = (float)sender.ActualHeight - combinedPadding.Top - combinedPadding.Bottom;
            var innerWidth = (float)sender.ActualWidth - combinedPadding.Left - combinedPadding.Right;
            var count = Data.Count;

            // 1.生成点位
            var points = new List<DataPoint>(count);
            var horizontalSpacing = innerWidth / (count - 1); // count - 1为间隔
            for (var i = 0; i < count; i++)
            {
                var item = Data.ElementAt(i);
                var percent = gap == 0 ? 0.5 : (item.Temperature.DegreesCelsius - min) / gap;
                var y = innerHeight + combinedPadding.Top - (percent * innerHeight);
                var x = combinedPadding.Left + i * horizontalSpacing;
                points.Add((item,new Vector2(x, (float)y)));
            }
            if (points.Count < 2) return;
            var drawingData = new DrawingData(points, combinedPadding.Left, combinedPadding.Top, innerHeight, innerWidth, innerWidth/count);

            // 2.绘制贝塞尔曲线
            DrawLine(sender, args.DrawingSession, drawingData);

            // 3.绘制点位
            DrawPoints(args.DrawingSession, drawingData.DataPoints);

            // 4.绘制坐标轴
            DrawHorizontalAxis(sender, args.DrawingSession, drawingData);
            DrawGrid(sender, args.DrawingSession, points);
        }

        private void DrawPoints(CanvasDrawingSession session, IEnumerable<DataPoint> dataPoints)
        {
            var fontSize = (float)FontSize;
            var textColor = TextColor;
            foreach (var dataPoint in dataPoints)
            {
                //session.FillCircle(dataPoint.Point, 4, Colors.Black);
                //session.DrawCircle(dataPoint.Point, 4, Colors.White, 3);
                session.DrawCenteredTextLayout(
                    new CanvasTextLayout(session, dataPoint.Data.Temperature.DegreesCelsius.ToString("0.0"), 
                        new CanvasTextFormat { FontSize = fontSize }, 100, 20), 
                    dataPoint.Point.X, dataPoint.Point.Y - 16, textColor);
            }
        }

        private void DrawLine(ICanvasResourceCreator canvasResourceCreator,CanvasDrawingSession session, DrawingData data)
        {
            var (controlPoints1, controlPoints2) = GetControlPoints(data.DataPoints.Select(p => p.Point).ToList());
            using (var path = new CanvasPathBuilder(canvasResourceCreator))
            {
                using var fillPath = new CanvasPathBuilder(canvasResourceCreator);
                fillPath.BeginFigure(data.DataPoints[0].Point);
                path.BeginFigure(data.DataPoints[0].Point);

                for (var i = 0; i < data.DataPoints.Count - 1; i++)
                {
                    path.AddCubicBezier(controlPoints1[i], controlPoints2[i], data.DataPoints[i + 1].Point);
                    fillPath.AddCubicBezier(controlPoints1[i], controlPoints2[i], data.DataPoints[i + 1].Point);
                }
                path.EndFigure(CanvasFigureLoop.Open);
                fillPath.AddLine(data.X + data.Width, data.Y + data.Height + InnerPadding.Bottom);
                fillPath.AddLine(data.X, data.Y + data.Height + InnerPadding.Bottom);
                fillPath.EndFigure(CanvasFigureLoop.Closed);

                using var geometry = CanvasGeometry.CreatePath(path);
                using var fillGeometry = CanvasGeometry.CreatePath(fillPath);

                using var brush = new CanvasLinearGradientBrush(canvasResourceCreator, [
                    new CanvasGradientStop { Position = 0.0f, Color = FillColor },
                    new CanvasGradientStop { Position = 1.0f, Color = Colors.Transparent }
                ]);

                brush.StartPoint = new Vector2(data.X, data.Y);
                brush.EndPoint = new Vector2(data.X, data.Y + data.Height + InnerPadding.Bottom);
                
                session.DrawGeometry(geometry, LineColor, LineStrokeWidth);
                session.FillGeometry(fillGeometry, brush);
            }
        }

        private void DrawHorizontalAxis(
            ICanvasResourceCreator canvasResourceCreator,
            CanvasDrawingSession session, DrawingData data)
        {
            var dataPoints = data.DataPoints;
            var last = dataPoints[0];
            var fontSize =(float) FontSize;
            var textColor = TextColor;
            using var brush = new CanvasLinearGradientBrush(canvasResourceCreator, [
                new CanvasGradientStop { Position = 0.0f, Color = VerticalGridLineColor },
                new CanvasGradientStop { Position = 1.0f, Color = Colors.Transparent }
            ]);
            var style = new CanvasStrokeStyle { CustomDashStyle = VerticalGridLineDashStyle };

            for (var i = 0; i < dataPoints.Count; i++)
            {
                if (i == 0 || i == dataPoints.Count -1 || last.Data.WeatherCode != dataPoints[i].Data.WeatherCode)
                {
                    if(i != 0)
                    {
                        var spacing = dataPoints[i].Point.X - last.Point.X;
                        // 绘制天气图标（无图标时绘制文字）
                        if (_iconCache.TryGetValue(last.Data.WeatherCode, out var icon))
                        {
                            session.DrawImage(icon, new Rect(
                                last.Point.X - spacing / 2, data.Y + data.Height,
                                spacing, spacing), 
                                new Rect(0,0,icon.SizeInPixels.Width, icon.SizeInPixels.Height));
                        }
                        else
                        {
                            var weatherTextLayout =
                                new CanvasTextLayout(
                                    canvasResourceCreator, last.Data.WeatherCode.ToString(),
                                    new CanvasTextFormat { HorizontalAlignment = CanvasHorizontalAlignment.Center },
                                    spacing, 1000);
                            session.DrawTextLayout(weatherTextLayout, last.Point.X, data.Y + data.Height, textColor);
                        }
                    }

                    last = dataPoints[i];

                    var axisPoint = new Vector2(dataPoints[i].Point.X, data.Y + data.Height + InnerPadding.Bottom);
                    brush.StartPoint = dataPoints[i].Point;
                    brush.EndPoint = axisPoint;
                    var textLayout = new CanvasTextLayout(canvasResourceCreator, last.Data.Time.Hour.ToString(), new CanvasTextFormat { FontSize = fontSize }, 50, 50);
                    session.DrawCenteredTextLayout(textLayout, axisPoint.X, axisPoint.Y, textColor);
                    session.DrawLine(dataPoints[i].Point, axisPoint, brush,1.5f , style);
                }
            }
        }

        private void DrawGrid(ICanvasResourceCreator canvasResourceCreator, 
            CanvasDrawingSession session,
            IReadOnlyList<DataPoint> dataPoints)
        {
            var highest = dataPoints.MinBy(p => p.Point.Y);// Y坐标越大温度越低
            var lowest = dataPoints.MaxBy(p => p.Point.Y);
            var gap = (int)highest.Data.Temperature.DegreesCelsius - (int)lowest.Data.Temperature.DegreesCelsius;
            var bottomY = lowest.Point.Y;
            var topY = highest.Point.Y;
            var height = bottomY - topY;
            var spacing = height / gap;
            var leftX = dataPoints[0].Point.X;
            var rightX = dataPoints[^1].Point.X;
            var style = new CanvasStrokeStyle { CustomDashStyle = [8,8] };
            var gridStrokeWidth = GridStrokeWidth;
            var gridLineColor = HorizontalGridLineColor;

            for (var i = 0; i <= gap; i++)
            {
                var y = topY + spacing * i;
                session.DrawLine(leftX, y, rightX, y, gridLineColor, gridStrokeWidth, style);
            }
        }

        private static (Vector2[] p1, Vector2[] p2) GetControlPoints(IList<Vector2> knots)
        {
            int n = knots.Count - 1;
            var p1 = new Vector2[n];
            var p2 = new Vector2[n];

            // RHS vector
            var a = new float[n];
            var b = new float[n];
            var c = new float[n];
            var r = new Vector2[n];

            // Left most segment
            a[0] = 0;
            b[0] = 2;
            c[0] = 1;
            r[0] = knots[0] + 2 * knots[1];

            // Internal segments
            for (int i = 1; i < n - 1; i++)
            {
                a[i] = 1;
                b[i] = 4;
                c[i] = 1;
                r[i] = 4 * knots[i] + 2 * knots[i + 1];
            }

            // Right most segment
            a[n - 1] = 2;
            b[n - 1] = 7;
            c[n - 1] = 0;
            r[n - 1] = 8 * knots[n - 1] + knots[n];

            // Solve Ax=b with the Thomas algorithm (from Wikipedia)
            for (int i = 1; i < n; i++)
            {
                var m = a[i] / b[i - 1];
                b[i] -= m * c[i - 1];
                r[i] -= m * r[i - 1];
            }

            p1[n - 1] = r[n - 1] / b[n - 1];
            for (var i = n - 2; i >= 0; --i)
                p1[i] = (r[i] - c[i] * p1[i + 1]) / b[i];

            for (var i = 0; i < n - 1; i++)
                p2[i] = 2 * knots[i + 1] - p1[i + 1];

            p2[n - 1] = 0.5f * (knots[n] + p1[n - 1]);

            return (p1, p2);
        }

        private record DrawingData(List<DataPoint> DataPoints,float X,float Y, float Height, float Width, float spacing);

        private struct ThicknessF
        {
            private float _Left;
            private float _Top;
            private float _Right;
            private float _Bottom;

            public ThicknessF(float uniformLength)
            {
                this._Left = this._Top = this._Right = this._Bottom = uniformLength;
            }

            public ThicknessF(float left, float top, float right, float bottom)
            {
                this._Left = left;
                this._Top = top;
                this._Right = right;
                this._Bottom = bottom;
            }

            public float Left
            {
                get => this._Left;
                set => this._Left = value;
            }

            public float Top
            {
                get => this._Top;
                set => this._Top = value;
            }

            public float Right
            {
                get => this._Right;
                set => this._Right = value;
            }

            public float Bottom
            {
                get => this._Bottom;
                set => this._Bottom = value;
            }

            public override string ToString() => this.ToString(CultureInfo.InvariantCulture);

            internal string ToString(CultureInfo cultureInfo)
            {
                char numericListSeparator = TokenizerHelper.GetNumericListSeparator((IFormatProvider)cultureInfo);
                StringBuilder stringBuilder = new StringBuilder(64 /*0x40*/);
                stringBuilder.Append(this.InternalToString(this._Left, cultureInfo));
                stringBuilder.Append(numericListSeparator);
                stringBuilder.Append(this.InternalToString(this._Top, cultureInfo));
                stringBuilder.Append(numericListSeparator);
                stringBuilder.Append(this.InternalToString(this._Right, cultureInfo));
                stringBuilder.Append(numericListSeparator);
                stringBuilder.Append(this.InternalToString(this._Bottom, cultureInfo));
                return stringBuilder.ToString();
            }

            internal string InternalToString(float l, CultureInfo cultureInfo)
            {
                return float.IsNaN(l) ? "Auto" : Convert.ToString(l, (IFormatProvider)cultureInfo);
            }

            public override bool Equals(object obj) => obj is ThicknessF thickness && this == thickness;

            public bool Equals(ThicknessF thickness) => this == thickness;

            public override int GetHashCode()
            {
                return this._Left.GetHashCode() ^ this._Top.GetHashCode() ^ this._Right.GetHashCode() ^ this._Bottom.GetHashCode();
            }

            public static bool operator ==(ThicknessF t1, ThicknessF t2)
            {
                return t1._Left == t2._Left && t1._Top == t2._Top && t1._Right == t2._Right && t1._Bottom == t2._Bottom;
            }

            public static ThicknessF operator +(ThicknessF t1, ThicknessF t2)
            {
                return new ThicknessF(t1._Left + t2._Left, t1._Top + t2._Top, t1._Right + t2._Right, t1._Bottom + t2._Bottom);
            }

            public static bool operator !=(ThicknessF t1, ThicknessF t2) => !(t1 == t2);
        }
    }
}


public static class CanvasExtensions
{
    public static void DrawCenteredTextLayout(this CanvasDrawingSession session, CanvasTextLayout textLayout, float x, float y, Color color)
    {
        var textHeight = (float)textLayout.DrawBounds.Height;
        var textWidth = (float)textLayout.DrawBounds.Width;
        session.DrawTextLayout(textLayout, new Vector2(x - textWidth / 2, y - textHeight), color);
    }
}

internal static class SampleData
{
    public static List<HourlyWeather> GetHourlyWeather()
    {
        var data = new List<HourlyWeather>();
        var now = DateTimeOffset.Now;

        for (int i = 0; i < 24; i++)
        {
            var time = now.AddHours(i);
            var weather = new HourlyWeather
            {
                Time = time,
                Temperature = new Temperature((int)(15 + 5 * Math.Sin(i * Math.PI / 12) + Random.Shared.Next(2)), TemperatureUnit.DegreeCelsius),
                WeatherCode = GetWeatherCodeForHour(i),
                Wind = new Wind
                {
                    Speed = new Speed(10 + i % 5, SpeedUnit.KilometerPerHour),
                    Angle = new Angle(i * 15, AngleUnit.Degree),
                    Direction = (WindDirection)(i % 16)
                },
                CloudAmount = (i % 24 < 6 || i % 24 > 18) ? 20.0 : 60.0, // 夜间云少，白天云多
                Humidity = 50 + 10 * Math.Cos(i * Math.PI / 12),
                Visibility = new Length(20 - i % 10, LengthUnit.Kilometer),
                Pressure = new Pressure(1010 + i % 5, PressureUnit.Hectopascal)
            };
            data.Add(weather);
        }
        return data;
    }

    private static WeatherCode GetWeatherCodeForHour(int hour)
    {
        // 模拟天气变化
        if (hour > 18 || hour < 6)
        {
            return hour % 5 == 0 ? WeatherCode.PartlyCloudy : WeatherCode.Clear;
        }
        else if (hour > 12 && hour < 18)
        {
            return hour % 6 == 0 ? WeatherCode.LightDrizzle : WeatherCode.PartlyCloudy;
        }
        else
        {
            return WeatherCode.Clear;
        }
    }
}