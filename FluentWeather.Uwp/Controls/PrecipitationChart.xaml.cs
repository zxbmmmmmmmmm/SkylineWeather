using FluentWeather.Abstraction.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentWeather.Uwp.Controls
{
    public sealed partial class PrecipitationChart : UserControl
    {
        public PrecipitationChart()
        {
            this.InitializeComponent();
        }

        public List<PrecipitationItemBase> Precipitations
        {
            get => (List<PrecipitationItemBase>)GetValue(PrecipitationsProperty);
            set => SetValue(PrecipitationsProperty, value);
        }
        public static readonly DependencyProperty PrecipitationsProperty =
            DependencyProperty.Register(nameof(Precipitations), typeof(List<PrecipitationItemBase>), typeof(PrecipitationChart), new PropertyMetadata(default));
    }
}
