using Windows.UI.Xaml;

namespace FluentWeather.Uwp.Themes;

public sealed partial class Generic:ResourceDictionary
{
    public Generic()
    {
        this.InitializeComponent();
    }
    public static Visibility GetPrecipVisibility(int? precip)
    {
        return (precip >= 10) ? Visibility.Visible : Visibility.Collapsed;
    }

}