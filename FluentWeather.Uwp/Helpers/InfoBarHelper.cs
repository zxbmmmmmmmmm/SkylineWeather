using System.Threading.Tasks;
using Windows.System;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using System;
using MetroLog.WinRT;
using FluentWeather.Uwp.Shared;

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
        infoBar.IsOpen = false;
        _container?.Children.Remove(infoBar);
    }
    public static void Error(string title, string message, int delay = 5000,bool isClosable = true)
    {
        AddToContainer(InfoBarSeverity.Error, title, message, delay, isClosable);
    }
    public static void Info(string title, string message, int delay = 5000, bool isClosable = true ,string buttonContent = null, Action action = null)
    {
        DispatcherQueue.GetForCurrentThread().TryEnqueue(async () =>
        {
            var infoBar = new InfoBar
            {
                Severity = InfoBarSeverity.Informational,
                Title = title,
                Message = message,
                IsOpen = true,
                IsClosable = isClosable,           
            };
            if (action is not null)
            {
                var btn = new Button
                {
                    Content = buttonContent,
                    HorizontalAlignment = HorizontalAlignment.Right,
                };
                btn.Click += (_, _) =>
                {
                    try
                    {
                        action();
                    }
                    catch (Exception ex)
                    {
                        Common.LogManager.GetLogger(("InfoBarHelper")).Error($"{title} - {message} - {buttonContent}", ex);
                    }
                };
                infoBar.ActionButton = btn;
            }
            _container.Children.Add(infoBar);
            if (delay > 0)
            {

                await Task.Delay(delay);
                infoBar.IsOpen = false;
            }
        });
    }
    public static void Success(string title, string message, int delay = 5000, bool isClosable = true)
    {
        AddToContainer(InfoBarSeverity.Success, title, message, delay, isClosable);
    }
    public static void Warning(string title, string message, int delay = 5000, bool isClosable = true)
    {
        AddToContainer(InfoBarSeverity.Warning, title, message, delay, isClosable);
    }
    private static void AddToContainer(InfoBarSeverity severity,string title,string message,int delay, bool isClosable)
    {
        DispatcherQueue.GetForCurrentThread().TryEnqueue(async () =>
        {
            var infoBar = new InfoBar
            {
                Severity = severity,
                Title = title,
                Message = message,
                IsOpen = true,
                IsClosable = isClosable
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