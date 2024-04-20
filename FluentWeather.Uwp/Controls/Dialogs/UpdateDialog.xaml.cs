using FluentWeather.Uwp.Helpers.Update;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace FluentWeather.Uwp.Controls.Dialogs;

public sealed partial class UpdateDialog : ContentDialog
{
    public UpdateDialog(UpdateInfo updateInfo)
    {
        this.InitializeComponent();
        this.UpdateInfo = updateInfo;
    }

    public UpdateInfo UpdateInfo
    {
        get => (UpdateInfo)GetValue(UpdateInfoProperty);
        set => SetValue(UpdateInfoProperty, value);
    }

    // Using a DependencyProperty as the backing store for UpdateInfo.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty UpdateInfoProperty =
        DependencyProperty.Register(nameof(UpdateInfo), typeof(UpdateInfo), typeof(UpdateDialog), new PropertyMetadata(default));

    private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        Launcher.LaunchUriAsync(new Uri("https://wwxk.lanzouj.com/b02x4kddg"));
    }

    private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        Launcher.LaunchUriAsync(new Uri(UpdateInfo.HtmlUrl));
    }

    private void MarkdownTextBlock_LinkClicked(object sender, Microsoft.Toolkit.Uwp.UI.Controls.LinkClickedEventArgs e)
    {
        Launcher.LaunchUriAsync(new Uri(e.Link));
    }
}