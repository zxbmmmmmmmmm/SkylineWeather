using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FluentWeather.Abstraction.Models;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentWeather.Uwp.Controls
{
    public sealed partial class HourlyDataChart : UserControl
    {
        public HourlyDataChart()
        {
            this.InitializeComponent();
        }

        public List<WeatherHourlyBase> HourlyForecasts
        {
            get => ((List<WeatherHourlyBase>)GetValue(HourlyForecastsProperty));
            set => SetValue(HourlyForecastsProperty, value);
        }


        public static readonly DependencyProperty HourlyForecastsProperty =
            DependencyProperty.Register(nameof(HourlyForecasts), typeof(List<WeatherHourlyBase>), typeof(HourlyDataChart), new PropertyMetadata(default));

        public string PropertyName
        {
            get => (string)GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register(nameof(PropertyName), typeof(string), typeof(HourlyDataChart), new PropertyMetadata(default));


        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(HourlyDataChart), new PropertyMetadata(default, OnPropertyChanged));

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(HourlyDataChart), new PropertyMetadata(default, OnPropertyChanged));

        public double MajorStep
        {
            get => (double)GetValue(MajorStepProperty);
            set => SetValue(MajorStepProperty, value);
        }

        public static readonly DependencyProperty MajorStepProperty =
            DependencyProperty.Register(nameof(MajorStep), typeof(double), typeof(HourlyDataChart), new PropertyMetadata(default, OnPropertyChanged));

        public DataTemplate TrackInfoTemplate
        {
            get => (DataTemplate)GetValue(TrackInfoTemplateProperty);
            set => SetValue(TrackInfoTemplateProperty, value);
        }

        public static readonly DependencyProperty TrackInfoTemplateProperty =
            DependencyProperty.Register(nameof(TrackInfoTemplateProperty), typeof(DataTemplate), typeof(HourlyDataChart), new PropertyMetadata(default));

        public DataTemplate IntersectionTemplate
        {
            get => (DataTemplate)GetValue(IntersectionTemplateProperty);
            set => SetValue(IntersectionTemplateProperty, value);
        }

        public static readonly DependencyProperty IntersectionTemplateProperty =
            DependencyProperty.Register(nameof(IntersectionTemplateProperty), typeof(DataTemplate), typeof(HourlyDataChart), new PropertyMetadata(default));

        public string LabelFormat
        {
            get => (string)GetValue(LabelFormatProperty);
            set => SetValue(LabelFormatProperty, value);
        }

        public static readonly DependencyProperty LabelFormatProperty =
            DependencyProperty.Register(nameof(LabelFormat), typeof(string), typeof(HourlyTemperatureChart), new PropertyMetadata(default));



        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = (HourlyDataChart)d;
            //chart.Bindings.Update();
            if (e.NewValue is default(double)) return;
            if(e.Property == MinimumProperty)
            {               
                chart.VerticalLinearAxis.Minimum = (double)e.NewValue;
            }
            if (e.Property == MaximumProperty)
            {
                chart.VerticalLinearAxis.Maximum = (double)e.NewValue;
            }
            if (e.Property == MajorStepProperty)
            {
                chart.VerticalLinearAxis.MajorStep = (double)e.NewValue;
            }
        }
    }
}
