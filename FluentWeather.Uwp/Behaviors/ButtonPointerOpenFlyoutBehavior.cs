using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace FluentWeather.Uwp.Behaviors;

public class ButtonPointerOpenFlyoutBehavior:Behavior<Button>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.PointerEntered += OnPointerEntered;
        AssociatedObject.PointerExited += OnPointerExited;
    }

    private void OnPointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        AssociatedObject?.Flyout?.Hide();
    }

    private void OnPointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
    {

        AssociatedObject?.Flyout?.ShowAt(AssociatedObject);
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.PointerEntered -= OnPointerEntered;
        AssociatedObject.PointerExited -= OnPointerExited;
    }
}