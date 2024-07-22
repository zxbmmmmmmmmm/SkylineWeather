using FluentWeather.Uwp.Controls.Popups;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace FluentWeather.Uwp.Behaviors;

public class ButtonOpenContentPopupBehavior:Behavior<ButtonBase>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.Click += ButtonClicked;
    }
    
    public Type PageType
    {
        get => (Type)GetValue(PageTypeProperty);
        set => SetValue(PageTypeProperty, value);
    }

    // Using a DependencyProperty as the backing store for PageType.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty PageTypeProperty =
        DependencyProperty.Register(nameof(PageType), typeof(Type), typeof(ButtonOpenContentPopupBehavior), new PropertyMetadata(typeof(Page)));


    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.Click -= ButtonClicked;
    }
    private void ButtonClicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
    {
        new ContentPopup().Show(PageType);
    }
}