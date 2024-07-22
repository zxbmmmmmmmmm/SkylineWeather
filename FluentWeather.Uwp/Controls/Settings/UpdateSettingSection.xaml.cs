﻿using FluentWeather.Uwp.Helpers.Update;
using FluentWeather.Uwp.Helpers;
using FluentWeather.Uwp.Shared;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using CommunityToolkit.WinUI.Helpers;
using FluentWeather.Uwp.Controls.Dialogs;
using Windows.ApplicationModel.Resources;
using FluentWeather.DIContainer;
using FluentWeather.Uwp.Helpers.Analytics;
using Microsoft.Extensions.DependencyInjection;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentWeather.Uwp.Controls.Settings
{
    public sealed partial class UpdateSettingSection : UserControl
    {
        public UpdateSettingSection()
        {
            this.InitializeComponent();
        }

        private async void CheckUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var versionString = Package.Current.Id.Version.ToFormattedString();
            var res = ResourceLoader.GetForCurrentView();
            try
            {
                var info = await UpdateHelper.CheckUpdateAsync("zxbmmmmmmmmm", "FluentWeather", new Version(versionString));
                Locator.ServiceProvider.GetService<AppAnalyticsService>()?.TrackUpdateManualChecked();
                var viewAction = new Action(async () =>
                { 
                    DialogManager.OpenDialogAsync(new UpdateDialog(info));
                });
                if (info.IsExistNewVersion)
                {
                    InfoBarHelper.Info(res.GetString("UpdateAvailable"), info.TagName, action: viewAction, buttonContent: res.GetString("ViewUpdate"));
                }
                else
                {
                    InfoBarHelper.Success(res.GetString("LatestVersion"), versionString, action: viewAction, buttonContent: res.GetString("ViewUpdate"));
                }
            }
            catch (Exception ex)
            {
                InfoBarHelper.Error(res.GetString("LatestVersion"), res.GetString("UpdateCheckFailedDescription"));
                Common.LogManager.GetLogger("UpdateHelper").Error("UpdateFailed", ex);
            }
        }
    }
}
