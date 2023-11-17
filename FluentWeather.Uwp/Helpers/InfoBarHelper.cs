using System.Threading.Tasks;
using Windows.System;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls;

namespace FluentWeather.Uwp.Helpers;

internal static class InfoBarHelper
{
    private static StackPanel _container;
    public static void Initialize(StackPanel container)
    {
        _container = container;
    }
    public static void ShowInfoBar(InfoBar infoBar)
    {
        _container?.Children.Add(infoBar);
    }
    public static void RemoveInfoBar(InfoBar infoBar)
    {
        _container?.Children.Remove(infoBar);
    }
    public static void Error(string title, string message, int delay = 5000)
    {
        AddToContainer(InfoBarSeverity.Error, title, message, delay);
    }
    public static void Info(string title, string message, int delay = 5000)
    {
        AddToContainer(InfoBarSeverity.Informational, title, message, delay);
    }
    public static void Success(string title, string message, int delay = 5000)
    {
        AddToContainer(InfoBarSeverity.Success, title, message, delay);
    }
    public static void Warning(string title, string message, int delay = 5000)
    {
        AddToContainer(InfoBarSeverity.Warning, title, message, delay);
    }
    private static void AddToContainer(InfoBarSeverity severity,string title,string message,int delay)
    {
        DispatcherQueue.GetForCurrentThread().TryEnqueue(async () =>
        {
            var infoBar = new InfoBar
            {
                Severity = severity,
                Title = title,
                Message = message,
                IsOpen = true,
            };
            _container.Children.Add(infoBar);
            if (delay > 0)
            {

                await Task.Delay(delay);
                infoBar.IsOpen = false;
            }
        });
    }
}