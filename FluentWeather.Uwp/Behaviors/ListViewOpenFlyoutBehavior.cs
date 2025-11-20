using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace FluentWeather.Uwp.Behaviors;

public class ListViewOpenFlyoutBehavior : Behavior<ListViewBase>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.ItemClick += ListItemClicked;
    }

    private void ListItemClicked(object sender, ItemClickEventArgs e)
    {
        var container = AssociatedObject.ContainerFromItem(e.ClickedItem);

        var item = container as SelectorItem;
        var content = item?.ContentTemplateRoot as FrameworkElement;
        if (content is null && e.ClickedItem is FrameworkElement element)
        {
            FlyoutBase.ShowAttachedFlyout(element);
        }
        else
        {
            FlyoutBase.ShowAttachedFlyout(content);
        }
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.ItemClick -= ListItemClicked;
    }
}