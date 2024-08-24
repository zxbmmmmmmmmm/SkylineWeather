using FluentWeather.Abstraction.Models;
using FluentWeather.DIContainer;
using FluentWeather.Uwp.Helpers.Analytics;
using FluentWeather.Uwp.Shared;
using Microsoft.Extensions.DependencyInjection;
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

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentWeather.Uwp.Controls
{
    public sealed partial class AnnouncementItem : UserControl
    {
        public Announcement Announcement
        {
            get => (Announcement)GetValue(AnnouncementProperty);
            set => SetValue(AnnouncementProperty, value);
        }

        public static readonly DependencyProperty AnnouncementProperty =
            DependencyProperty.Register(nameof(Announcement), typeof(Announcement), typeof(Announcement), new PropertyMetadata(default));
        public AnnouncementItem()
        {
            this.InitializeComponent();       
            this.Loaded += AnnouncementItem_Loaded;
            this.Unloaded += AnnouncementItem_Unloaded;
        }

        private bool _isViewed;

        private void AnnouncementItem_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= AnnouncementItem_Loaded;
            this.Unloaded -= AnnouncementItem_Unloaded;
            this.MainInfoBar.Closed -= MainInfoBar_Closed;
            this.ActionButton.Click -= ActionButton_Click;
        }

        private void AnnouncementItem_Loaded(object sender, RoutedEventArgs e)
        {
            this.MainInfoBar.Closed += MainInfoBar_Closed;
            this.ActionButton.Click += ActionButton_Click;
            _isViewed = Common.Settings.ClosedAnnouncements.Contains(Announcement.Name);
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            _isViewed = true;

            var service = Locator.ServiceProvider.GetService<AppAnalyticsService>();
            service.TrackAnnouncementViewed(Announcement.Name);

            var viewed = Common.Settings.ViewedAnnouncements;
            viewed.Add(Announcement.Name);
            Common.Settings.ViewedAnnouncements = viewed;

            if (Announcement.CloseWhenView)
            {
                MainInfoBar.IsOpen = false;
                var closed = Common.Settings.ClosedAnnouncements;
                closed.Add(Announcement.Name);
                Common.Settings.ClosedAnnouncements = closed;
            }
        }

        private void MainInfoBar_Closed(Microsoft.UI.Xaml.Controls.InfoBar sender, Microsoft.UI.Xaml.Controls.InfoBarClosedEventArgs args)
        {
            var service = Locator.ServiceProvider.GetService<AppAnalyticsService>();
            service.TrackAnnouncementClosed(Announcement.Name,_isViewed);

            var closed = Common.Settings.ClosedAnnouncements;
            closed.Add(Announcement.Name);
            Common.Settings.ClosedAnnouncements = closed;
        }
    }
}
