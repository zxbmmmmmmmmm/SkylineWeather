using System.Globalization;
using Windows.UI.Xaml;

namespace FluentWeather.Uwp.Themes;

public sealed partial class Generic:ResourceDictionary
{
    public Generic()
    {
        this.InitializeComponent();
        var isEnglish = CultureInfo.CurrentCulture.Name.Contains("en");
        this.Add("WeatherDescriptionFontSize", isEnglish ? 16 : 18);
        this.Add("HourlyWeatherDescriptionVisibility", isEnglish ? Visibility.Collapsed : Visibility.Visible);
    }
    public static Visibility GetPrecipVisibility(int? precip)
    {
        return (precip >= 10) ? Visibility.Visible : Visibility.Collapsed;
    }

}