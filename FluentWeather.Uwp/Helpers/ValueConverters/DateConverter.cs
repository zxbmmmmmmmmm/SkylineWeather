using System;
using System.Globalization;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.UI.Xaml.Data;

namespace FluentWeather.Uwp.Helpers.ValueConverters;

public sealed class DateConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not DateTime date)
            return value;
        if (date.Day == DateTime.Today.Day)
            return ResourceLoader.GetForCurrentView().GetString("Today");
        if (date.Day == DateTime.Today.Day + 1)
            return ResourceLoader.GetForCurrentView().GetString("Tomorrow");

        return CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(date.DayOfWeek).Replace("星期","周");
    }

     

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

public class ShortDateConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not DateTime date)
            return value;

        if (ApplicationLanguages.Languages[0].Contains("zh"))
            return date.Month + "月" + date.Day + "日";
        return date.Month + "/" + date.Day;
    }



    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}