using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.UI.Xaml;

namespace FluentWeather.Uwp.Helpers;

public static class Extensions
{
    extension<T>(T dictionary) where T : ResourceDictionary
    {
        public T Merge(ResourceDictionary d)
        {
            dictionary.MergedDictionaries.Add(d);
            return dictionary;
        }

        public T Merge(string source)
        {
            dictionary.MergedDictionaries.Add(new() { Source = new Uri(source) });
            return dictionary;
        }
        public T MergeMUXC(ControlsResourcesVersion version)
        {
            dictionary.MergedDictionaries.Add(new XamlControlsResources { ControlsResourcesVersion = version });
            return dictionary;
        }
    }

    extension<T>(IEnumerable<T> enumerable)
    {
        public ObservableCollection<T> ToObservableCollection()
        {
            return new ObservableCollection<T>(enumerable);
        }
    }

    extension(StorageFolder folder)
    {
        public async Task<StorageFile> GetOrCreateFileAsync(string name)
        {
            var item = await folder.TryGetItemAsync(name) as StorageFile;
            item ??= await folder.CreateFileAsync(name);
            return item;
        }

        public async Task<StorageFolder> GetOrCreateFolderAsync(string name)
        {
            var item = await folder.TryGetItemAsync(name) as StorageFolder;
            item ??= await folder.CreateFolderAsync(name);
            return item;
        }
    }

    extension<TKey, TValue>(IDictionary<TKey, TValue> dict)
    {
        public TValue GetOrCreate(TKey key)
        {
            if (!dict.TryGetValue(key, out TValue val))
            {
                val = default;
                dict.Add(key, val);
            }

            return val;
        }
    }
}
internal static class ResourceExtensions
{
    private static readonly ResourceLoader ResLoader = ResourceLoader.GetForCurrentView();
    private static readonly ResourceLoader IndependentResLoader = ResourceLoader.GetForViewIndependentUse();

    extension(string resourceKey)
    {
        public string GetLocalized()
        {
            return ResLoader.GetString(resourceKey);
        }

        public string GetLocalizedIndependent()
        {
            return IndependentResLoader.GetString(resourceKey);
        }
    }
}