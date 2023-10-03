using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentWeather.Uwp.Pages;
using FluentWeather.Uwp.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
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

[ObservableObject]
public sealed partial class AboutDialog : ContentDialog
{
    public AboutDialog()
    {
        this.InitializeComponent();
        Common.LogManager.GetLogger("Application").Info("打开对话框:关于");
    }
    [RelayCommand]
    public void Close()
    {
        Hide();
    }
    private string AppVersion => string.Format("{0}.{1}.{2}.{3}",
        Package.Current.Id.Version.Major,
        Package.Current.Id.Version.Minor,
        Package.Current.Id.Version.Build,
        Package.Current.Id.Version.Revision);
    

    private void AboutImage_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
    {
        DevCheckBox.Visibility = Visibility.Visible;
    }

    private void DevButton_Click(object sender, RoutedEventArgs e)
    {
        this.Hide();
        var frame = Window.Current.Content as Frame;
        if (frame is null) return;
        frame.Content = new TestPage();
    }
}