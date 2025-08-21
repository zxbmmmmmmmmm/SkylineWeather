using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SkylineWeather.WinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task GetSampleData()
        {
            var provider = new OpenMeteoProvider.OpenMeteoProvider();
            var data = await provider.GetHourlyWeatherAsync(new Abstractions.Models.Location(28, 119));
            data.IfSucc(result =>
            {
                Chart.Data = result.Take(24).ToList();
            });
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await GetSampleData();
        }

        private async void Button_Loaded(object sender, RoutedEventArgs e)
        {
            await GetSampleData();
        }
    }
}
