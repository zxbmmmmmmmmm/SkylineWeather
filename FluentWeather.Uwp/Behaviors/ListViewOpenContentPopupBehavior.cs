using FluentWeather.Uwp.Controls.Popups;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FluentWeather.Uwp.Behaviors;

public class ListViewOpenContentPopupBehavior : Behavior<ListViewBase>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.ItemClick += ListItemClicked;
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
        AssociatedObject.ItemClick -= ListItemClicked;
    }
    private void ListItemClicked(object sender, ItemClickEventArgs e)
    {
        new ContentPopup().Show(PageType);
    }

}