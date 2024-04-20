using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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