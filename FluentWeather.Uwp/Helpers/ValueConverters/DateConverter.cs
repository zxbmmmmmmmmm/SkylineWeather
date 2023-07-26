using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace FluentWeather.Uwp.Helpers.ValueConverters;

public class DateConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not DateTime date)
            return value;
        if (date.Day == DateTime.Today.Day)
            return "今天";
        if (date.Day == DateTime.Today.Day + 1)
            return "明天";
        
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

        return date.Month + "月" + date.Day + "日";
    }



    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}