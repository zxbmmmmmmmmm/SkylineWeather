using FluentWeather.Uwp.Helpers.Update;
using FluentWeather.Uwp.Helpers;
using FluentWeather.Uwp.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
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
using CommunityToolkit.WinUI.Helpers;
using Microsoft.AppCenter.Analytics;
using System.Security.Cryptography.X509Certificates;

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
            Analytics.TrackEvent("UpdateManualChecked",new Dictionary<string, string> { { "CurrentVersion", versionString } });
            try
            {
                var info = await UpdateHelper.CheckUpdateAsync("zxbmmmmmmmmm", "FluentWeather", new Version(versionString));
                var viewAction = new Action(() =>
                {
                    Launcher.LaunchUriAsync(new Uri(info.HtmlUrl));
                });
                if (info.IsExistNewVersion)
                {
                    InfoBarHelper.Info("更新可用", info.TagName, action: viewAction, buttonContent: "查看");
                }
                else
                {
                    InfoBarHelper.Success("应用为最新版本", versionString);
                }
            }
            catch (Exception ex)
            {
                InfoBarHelper.Error("检查更新失败", "请检查当前网络或尝试开启代理");
                Common.LogManager.GetLogger("UpdateHelper").Error("检查更新失败", ex);
            }
        }
    }
}
