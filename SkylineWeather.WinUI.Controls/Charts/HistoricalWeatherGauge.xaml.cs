using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using CommunityToolkit.WinUI;
using SkylineWeather.Abstractions.Models.Weather;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Text;
using SkylineWeather.WinUI.Controls.Helpers;
using Microsoft.UI;
using UnitsNet;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SkylineWeather.WinUI.Controls.Charts;

public sealed partial class HistoricalWeatherGauge : UserControl
{
    [GeneratedDependencyProperty]
    public partial HistoricalWeather Historical { get; set; }

    [GeneratedDependencyProperty]
    public partial DailyWeather Today { get; set; }

    [GeneratedDependencyProperty]
    public partial CurrentWeather Current { get; set; }

    [GeneratedDependencyProperty(DefaultValue = 8f)]
    public partial float GaugeHeight { get; set; }

    private readonly ThicknessF InnerPadding = new(16, 0, 16, 0);


    public HistoricalWeatherGauge()
    {
        InitializeComponent();
        RenderCanvas.CreateResources += OnRenderCanvasCreateResources;
        RenderCanvas.Draw += OnRenderCanvasDraw;
    }

    private void OnRenderCanvasDraw(CanvasControl sender, CanvasDrawEventArgs args)
    {
        args.DrawingSession.Blend = Microsoft.Graphics.Canvas.CanvasBlend.Copy;
        var historical = Historical;
        var today = Today;
        var current = Current;
        if (historical is null)
            return;
        var session = args.DrawingSession;
        var padding = (ThicknessF)Padding;
        var combinedPadding = padding + InnerPadding;
        var gaugeHeight = GaugeHeight;
        var innerHeight = (float)sender.ActualHeight - combinedPadding.Top - combinedPadding.Bottom;
        var innerWidth = (float)sender.ActualWidth - combinedPadding.Left - combinedPadding.Right;
        // 绘制指示条
        using var brush = new CanvasLinearGradientBrush(sender, [
            new CanvasGradientStop { Position = 0.0f, Color = Colors.Blue },
            new CanvasGradientStop { Position = 1.0f, Color = Colors.Red }
        ]);
        brush.StartPoint = new System.Numerics.Vector2(0, 0);
        brush.EndPoint = new System.Numerics.Vector2(innerWidth, 0);
        var y = combinedPadding.Top + innerHeight / 2 - gaugeHeight / 2;
        session.FillRoundedRectangle(new Rect(combinedPadding.Left, y, innerWidth, gaugeHeight), 4, 4, brush);


        List<RenderPoint> renderPoints = 
        [
            new(TemperatureType.Highest,historical.HighestTemperature,historical.HighestTemperatureDate),
            new(TemperatureType.Lowest,historical.LowestTemperature,historical.LowestTemperatureDate),
            new(TemperatureType.AverageHigh,historical.AverageHighTemperature,null),
            new(TemperatureType.AverageLow,historical.AverageLowTemperature,null),
        ];

        if (today is not null)
        {
            renderPoints.Add(new RenderPoint(TemperatureType.TodayHigh,today.HighTemperature, DateOnly.FromDateTime(DateTime.Today)));
            renderPoints.Add(new RenderPoint(TemperatureType.TodayLow,today.LowTemperature, DateOnly.FromDateTime(DateTime.Today)));
        }

        if (current is not null)
        {
            renderPoints.Add(new RenderPoint(TemperatureType.Current,current.Temperature, DateOnly.FromDateTime(DateTime.Today)));
        }

        renderPoints = renderPoints.OrderBy(p => p.Temperature).ToList();
        var max = renderPoints[0];
        var min = renderPoints[^1];
        var gap = max.Temperature.DegreesCelsius - min.Temperature.DegreesCelsius;

        foreach(var point in renderPoints)
        {
            var percent = (point.Temperature.DegreesCelsius - min.Temperature.DegreesCelsius) / gap;
            var x = combinedPadding.Left + (float)(percent * innerWidth);
            session.FillCircle(x, y + 4, 8, Colors.White);
            session.FillCircle(x, y + 4, 4, Colors.Transparent);
            var textLayout = new CanvasTextLayout(session, point.Type.ToString(),new CanvasTextFormat(), 500, 20);
            session.DrawCenteredTextLayout(textLayout, x, y - 24, Colors.White);
        }
    }

    private void OnRenderCanvasCreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args)
    {
            
    }

    private record RenderPoint(
        TemperatureType Type,
        Temperature Temperature,
        DateOnly? Date);

    private enum TemperatureType 
    {
        Highest,
        Lowest,
        AverageHigh,
        AverageLow,
        TodayHigh,
        TodayLow,
        Current
    }


    private enum TextPosition
    {
        Top,
        Bottom
    }
}