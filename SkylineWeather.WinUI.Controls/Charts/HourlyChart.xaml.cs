using CommunityToolkit.WinUI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
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
using UnitsNet;
using UnitsNet.Units;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SkylineWeather.WinUI.Controls.Charts
{
    public sealed partial class HourlyChart : UserControl
    {
        public HourlyChart()
        {
            InitializeComponent();
            RenderCanvas.Draw += OnRenderCanvasDraw;
            Data = SampleData.GetHourlyWeather();
        }

        private readonly ThicknessF InnerPadding = new(24,12,12,72);

        private void OnRenderCanvasDraw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            if (Data == null || !Data.Any()) return;

            var max = Data.Max(p => p.Temperature.DegreesCelsius);
            var min = Data.Min(p => p.Temperature.DegreesCelsius);
            var gap = max - min;
            var innerPadding = InnerPadding;
            var padding = new ThicknessF
            {
                Bottom = (float)Padding.Bottom,
                Left = (float)Padding.Left,
                Right = (float)Padding.Right,
                Top = (float)Padding.Top
            };
            var combinedPadding = innerPadding + padding;
            var height = (float)sender.ActualHeight - padding.Top - padding.Bottom;
            var width = (float)sender.ActualWidth - padding.Left - padding.Right;
            var innerHeight = (float)sender.ActualHeight - combinedPadding.Top - combinedPadding.Bottom;
            var innerWidth = (float)sender.ActualWidth - combinedPadding.Left - combinedPadding.Right;
            var count = Data.Count();

            // 1.生成点位
            var points = new List<Vector2>(count);
            var horizontalSpacing = innerWidth / (count - 1); // count - 1为间隔
            for (var i = 0; i < count; i++)
            {
                var item = Data.ElementAt(i);
                var percent = gap == 0 ? 0.5 : (item.Temperature.DegreesCelsius - min) / gap;
                var y = innerHeight + combinedPadding.Top - (percent * innerHeight);
                var x = combinedPadding.Left + i * horizontalSpacing;
                points.Add(new Vector2(x, (float)y));
            }
            
            if (points.Count < 2) return;


            // 2.绘制贝塞尔曲线
            DrawLine(sender, args.DrawingSession, points, combinedPadding.Left, combinedPadding.Top, innerHeight, innerWidth);

            // 3.绘制点位
            foreach (var point in points)
            {
                args.DrawingSession.FillCircle(point, 4, Colors.Black);
                args.DrawingSession.DrawCircle(point, 4, Colors.White, 3);
            }

            // 4.绘制坐标轴
            DrawAxis(sender, args.DrawingSession, Data, combinedPadding.Left, height + padding.Top, innerWidth);
            DrawWeatherAxis(sender, args.DrawingSession, Data, combinedPadding.Left, height + padding.Top - 36, innerWidth, 16);
            DrawGrid(sender, args.DrawingSession, points, (int)max, (int)min);
        }


        private void DrawLine(ICanvasResourceCreator canvasResourceCreator,CanvasDrawingSession session, IList<Vector2> points, float x, float y, float height, float width)
        {
            var (controlPoints1, controlPoints2) = GetControlPoints(points);
            using (var path = new CanvasPathBuilder(canvasResourceCreator))
            {
                using var fillPath = new CanvasPathBuilder(canvasResourceCreator);
                fillPath.BeginFigure(points[0]);
                path.BeginFigure(points[0]);

                for (var i = 0; i < points.Count - 1; i++)
                {
                    path.AddCubicBezier(controlPoints1[i], controlPoints2[i], points[i + 1]);
                    fillPath.AddCubicBezier(controlPoints1[i], controlPoints2[i], points[i + 1]);
                }
                path.EndFigure(CanvasFigureLoop.Open);
                fillPath.AddLine(x + width, y + height + InnerPadding.Bottom);
                fillPath.AddLine(x, y + height + InnerPadding.Bottom);
                fillPath.EndFigure(CanvasFigureLoop.Closed);

                using var geometry = CanvasGeometry.CreatePath(path);
                using var fillGeometry = CanvasGeometry.CreatePath(fillPath);

                using var brush = new CanvasLinearGradientBrush(canvasResourceCreator, [
                    new CanvasGradientStop { Position = 0.0f, Color = Color.FromArgb(64,255,255,255) },
                    new CanvasGradientStop { Position = 1.0f, Color = Colors.Transparent }
                ]);

                brush.StartPoint = new Vector2(x, y);
                brush.EndPoint = new Vector2(x, y + height + InnerPadding.Bottom);
                
                session.DrawGeometry(geometry, Colors.White, StrokeWidth);
                session.FillGeometry(fillGeometry, brush);
            }
        }

        private void DrawAxis(ICanvasResourceCreator canvasResourceCreator, CanvasDrawingSession session, IReadOnlyList<HourlyWeather> data, float x, float y, float width)
        {
            //session.DrawLine(new Vector2(x, y), new Vector2(x + width, y), Colors.White);
            var spacing = width / (data.Count - 1);
            var lastWeather = data[0].WeatherCode;
            for (var i = 0; i < data.Count; i++)
            {
                if (i == 0 || i == data.Count - 1 || lastWeather != data[i].WeatherCode)
                {
                    var text = data[i].Time.Hour.ToString();
                    var textLayout = new CanvasTextLayout(canvasResourceCreator, text, new CanvasTextFormat { FontSize = 14 }, spacing, float.MaxValue);
                    var regions = textLayout.GetCharacterRegions(0, text.Length);
                    var textHeight = (float)regions[0].LayoutBounds.Height;
                    var textWidth = (float)regions.Sum(p => p.LayoutBounds.Width);
                    session.DrawTextLayout(textLayout, new Vector2(x - textWidth / 2, y - textHeight), Color.FromArgb(128, 255, 255, 255));
                }
                lastWeather = data[i].WeatherCode;
                x += spacing;
            }
        }

        private void DrawWeatherAxis(ICanvasResourceCreator canvasResourceCreator, CanvasDrawingSession session, IReadOnlyList<HourlyWeather> data, float x, float y, float width, float height)
        {
            var spacing = width / (data.Count - 1);
            var lastWeather = data[0].WeatherCode;
            var lastDrawnPosition = x;
            for (var i = 0; i < data.Count; i++)
            {
                if (i == 0 || i == data.Count - 1 || lastWeather != data[i].WeatherCode)
                {
                    if (i != 0)
                    {
                        //TODO: 修改为天气图标
                        var weatherWidth = x - lastDrawnPosition;
                        var text = data[i - 1].WeatherCode.ToString();
                        var textLayout = new CanvasTextLayout(canvasResourceCreator, text, new CanvasTextFormat { FontSize = 14 }, spacing, float.MaxValue);
                        var regions = textLayout.GetCharacterRegions(0, text.Length);
                        var textHeight = (float)regions[0].LayoutBounds.Height;
                        var textWidth = (float)regions.Sum(p => p.LayoutBounds.Width);
                        session.DrawTextLayout(textLayout, lastDrawnPosition + (weatherWidth / 2) - (textWidth / 2), y, Color.FromArgb(128, 255, 255, 255));
                    }
                    session.DrawLine(x, y, x, y + height, Colors.White);
                    lastDrawnPosition = x;

                }
                lastWeather = data[i].WeatherCode;
                x += spacing;
            }
        }

        private void DrawGrid(ICanvasResourceCreator canvasResourceCreator, 
            CanvasDrawingSession session,
            IReadOnlyList<Vector2> points,
            int max,int min)
        {
            var gap = max - min;
            var bottomY = points.Max(p => p.Y);
            var topY = points.Min(p => p.Y);
            var height = bottomY - topY;
            var spacing = height / gap;
            var leftX = points[0].X;
            var rightX = points[^1].X;
            var style = new CanvasStrokeStyle { CustomDashStyle = [8,8] };
            for (var i = 0; i <= gap; i++)
            {
                session.DrawLine(leftX, topY + spacing * i, rightX, topY + spacing*i, Color.FromArgb(32,255,255,255),0.5f , style);
            }
        }

        private (Vector2[] p1, Vector2[] p2) GetControlPoints(IList<Vector2> knots)
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


        [GeneratedDependencyProperty]
        public partial IReadOnlyList<HourlyWeather> Data { get; set; }

        [GeneratedDependencyProperty(DefaultValue = 3f)]
        public partial float StrokeWidth { get; set; }

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

public static class SampleData
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