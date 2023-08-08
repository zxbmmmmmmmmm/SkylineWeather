using FluentWeather.Uwp.Controls.Popups;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace FluentWeather.Uwp.Behaviors;

public class ButtonOpenContentPopupBehavior:Behavior<ButtonBase>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.Click += ButtonClicked;
    }
    
    public FrameworkElement Content
    {
        get => (FrameworkElement)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public static readonly DependencyProperty ContentProperty =
        DependencyProperty.Register(nameof(Content), typeof(FrameworkElement), typeof(ButtonOpenContentPopupBehavior), new PropertyMetadata(null));

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.Click -= ButtonClicked;
    }
    private void ButtonClicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
    {
        new ContentPopup().Show(Content);
    }
}