using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
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
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
    {
        return new ObservableCollection<T>(enumerable);
    }
    public static async Task<StorageFile> GetOrCreateFileAsync(this StorageFolder folder,string name)
    {
        var item = await folder.TryGetItemAsync(name) as StorageFile;
        item ??= await folder.CreateFileAsync(name);
        return item;
    }
    public static async Task<StorageFolder> GetOrCreateFolderAsync(this StorageFolder folder, string name)
    {
        var item = await folder.TryGetItemAsync(name) as StorageFolder;
        item ??= await folder.CreateFolderAsync(name);
        return item;
    }
    public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
    {
        if (!dict.TryGetValue(key, out TValue val))
        {
            val = default;
            dict.Add(key, val);
        }

        return val;
    }
}