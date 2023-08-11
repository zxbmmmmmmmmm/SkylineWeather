using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace FluentWeather.Uwp.Behaviors;

public class ButtonCloseContentDialogBehavior:Behavior<ButtonBase>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.Click += ButtonClicked;
    }

    private void ButtonClicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
    {
        var dialog = AssociatedObject.FindParent<ContentDialog>();
        dialog?.Hide();
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.Click -= ButtonClicked;

    }

}