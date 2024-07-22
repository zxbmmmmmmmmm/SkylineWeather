using CommunityToolkit.Mvvm.ComponentModel;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentWeather.Uwp.Controls.Settings;

[ObservableObject]
public sealed partial class QWeatherSettingSection : UserControl
{
    public QWeatherSettingSection()
    {
        this.InitializeComponent();
        //var settingsHelper = Locator.ServiceProvider.GetService<ISettingsHelper>();
        //var settings = Locator.ServiceProvider.GetService<ISetting>();
        //Token = settingsHelper.ReadLocalSetting(settings.Id + "." + nameof(Token),"");
        //PropertyChanged += OnPropertyChanged; 
    }

    //private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    //{
    //    var settingsHelper = Locator.ServiceProvider.GetService<ISettingsHelper>();
    //    var settings = Locator.ServiceProvider.GetService<ISetting>();
    //    //Token = settingsHelper.ReadLocalSetting(settings.Id + "." + nameof(Token), "");

    //    switch (e.PropertyName)
    //    {
    //        case nameof(Token):
    //            settingsHelper.WriteLocalSetting(settings.Id + "." + nameof(Token), Token);
    //            break;
    //    }
    //}

    //[ObservableProperty]
    //private string _token;


}

