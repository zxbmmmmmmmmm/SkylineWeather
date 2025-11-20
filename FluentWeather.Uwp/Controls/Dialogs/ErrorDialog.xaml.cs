using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace FluentWeather.Uwp.Controls.Dialogs;

public sealed partial class ErrorDialog : ContentDialog
{


    public IList<Exception> Exceptions
    {
        get => (IList<Exception>)GetValue(ExceptionsProperty);
        set => SetValue(ExceptionsProperty, value);
    }

    public static readonly DependencyProperty ExceptionsProperty =
        DependencyProperty.Register(nameof(Exceptions), typeof(IList<Exception>), typeof(ErrorDialog), new PropertyMetadata(null));


    public ErrorDialog()
    {
        this.InitializeComponent();
    }

}