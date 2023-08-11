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

namespace FluentWeather.Uwp.Controls.Popups
{
    public sealed partial class ContentPopup : UserControl
    {
        private Popup _popup;
        public ContentPopup()
        {
            this.InitializeComponent();
            _popup = new Popup
            {
                Child = this,
                ShouldConstrainToRootBounds = true,
            };
        }
        public void Show(Type pageType,object parameter= null,string title = "")
        {
            this.Width = Window.Current.Bounds.Width;
            this.Height = Window.Current.Bounds.Height;
            Window.Current.SizeChanged += WindowSizeChanged;
            _popup.IsOpen = true;
            TitleTextBlock.Text = title;
            BackButton.Click += BackButton_Click;
            ContentFrame.Navigate(pageType, parameter);
        }

        private void WindowSizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            this.Width = Window.Current.Bounds.Width;
            this.Height = Window.Current.Bounds.Height;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _popup.IsOpen = false;
            _popup.Child = null;
            _popup = null;
            Window.Current.SizeChanged -= WindowSizeChanged;
            BackButton.Click -= BackButton_Click;
        }
    }
}
