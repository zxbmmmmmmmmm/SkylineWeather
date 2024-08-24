using FluentWeather.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace FluentWeather.Uwp.Shared.Helpers.ValueConverters
{
    public class ListViewSelectionParameterConverter : IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // cast value to whatever EventArgs class you are expecting here
            var args = (SelectionChangedEventArgs)value;
            // return what you need from the args
            return (GeolocationBase)args.AddedItems.FirstOrDefault();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
