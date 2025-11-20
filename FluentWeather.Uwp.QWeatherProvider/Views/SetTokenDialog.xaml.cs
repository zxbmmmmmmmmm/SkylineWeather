using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentWeather.Abstraction.Models.Exceptions;
using FluentWeather.Uwp.Shared;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace FluentWeather.Uwp.QWeatherProvider.Views;

[ObservableObject]
public sealed partial class SetTokenDialog : ContentDialog
{
    [ObservableProperty]
    private partial string Key { get; set; }

    [ObservableProperty]
    private partial string PublicId { get; set; }

    [ObservableProperty]
    private partial string Domain { get; set; }
    public SetTokenDialog()
    {
        this.InitializeComponent();
    }
    [RelayCommand]
    private async Task Confirm()
    {
        try
        {
            await CheckKey(Key);
            Common.Settings.QWeatherToken = Key;
            Common.Settings.QWeatherDomain = Domain;
            Common.Settings.QWeatherPublicId = "";
            Common.Settings.OOBECompleted = true;
            Hide();
            await CoreApplication.RequestRestartAsync(string.Empty);
        }
        catch (Exception e)
        {
            ErrorTextblock.Visibility = Visibility.Visible;
            ErrorTextblock.Text = e.Message;
        }

    }
    private async Task CheckKey(string token)
    {
        var client = new QWeatherProvider(token, Domain, null, PublicId);
        await client.GetCurrentWeather(116.39, 39.9);
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

    private async void DomainBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        var con = Clipboard.GetContent();
        if (!con.Contains(StandardDataFormats.Text)) return;
        var str = await con.GetTextAsync();
        Domain = str;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Hide();
    }
}