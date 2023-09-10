using Microsoft.UI.Xaml.Controls;
using System;
using Windows.UI.Xaml;

namespace FluentWeather.Uwp.Helpers;

public static class Extensions
{
    public static T Merge<T>(this T dictionary, ResourceDictionary d) where T : ResourceDictionary
    {
        dictionary.MergedDictionaries.Add(d);
        return dictionary;
    }
    public static T Merge<T>(this T dictionary, string source) where T : ResourceDictionary
    {
        dictionary.MergedDictionaries.Add(new() { Source = new Uri(source) });
        return dictionary;
    }
    public static T MergeMUXC<T>(this T dictionary, ControlsResourcesVersion version) where T : ResourceDictionary
    {
        dictionary.MergedDictionaries.Add(new XamlControlsResources { ControlsResourcesVersion = version });
        return dictionary;
    }
}