using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentWeather.Abstraction.Models.Exceptions;
using FluentWeather.Uwp.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
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

namespace FluentWeather.Uwp.QWeatherProvider.Views;
[ObservableObject]
public sealed partial class SetTokenDialog : ContentDialog
{
    [ObservableProperty]
    private string key;
    [ObservableProperty]
    private string publicId;

    public SetTokenDialog()
    {
        this.InitializeComponent();
    }
    [RelayCommand]
    private async Task Confirm()
    {
        try
        {
            var domain = await CheckKey(Key);
            Common.Settings.QWeatherToken = Key;
            Common.Settings.QWeatherDomain = domain;
            Common.Settings.QWeatherPublicId = PublicId;
            Common.Settings.OOBECompleted = true;
            Hide();
            await CoreApplication.RequestRestartAsync(string.Empty);
        }
        catch(HttpResponseException e)
        {
            ErrorTextblock.Visibility = Visibility.Visible;
            if(PublicId != "")
            {
                ErrorTextblock.Text = $"认证失败({(int)e.Code}-{e.Code})" + Environment.NewLine + "请检查Public Id与KEY是否对应";
            }
            else
            {
                ErrorTextblock.Text = $"此KEY不可用，请重试({(int)e.Code}-{e.Code})";
            }
        }

    }
    private async Task<string> CheckKey(string token)
    {
        var client = new QWeatherProvider(token, "api.qweather.com",null,PublicId);
        try
        {
            await client.GetCurrentWeather(116.39,39.9);
        }
        catch
        {
            client.SetDomain("devapi.qweather.com");
            await client.GetCurrentWeather(116.39, 39.9);
            return "devapi.qweather.com";
        }
        return "api.qweather.com";
    }

    private async void KeyBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        var con = Clipboard.GetContent();
        if (!con.Contains(StandardDataFormats.Text)) return;
        var str = await con.GetTextAsync();
        Key = str;
    }

    private async void PublicIdBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        var con = Clipboard.GetContent();
        if (!con.Contains(StandardDataFormats.Text)) return;
        var str = await con.GetTextAsync();
        PublicId = str;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Hide();
    }
}