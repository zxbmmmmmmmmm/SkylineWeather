﻿using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using FluentWeather.Uwp.Shared;

namespace FluentWeather.Uwp.Behaviors;

public class ButtonOpenContentDialogBehavior:Behavior<ButtonBase>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.Click += ButtonClicked;
    }

    public Type DialogType
    {
        get => (Type)GetValue(DialogTypeProperty);
        set => SetValue(DialogTypeProperty, value);
    }

    public static readonly DependencyProperty DialogTypeProperty =
        DependencyProperty.Register(nameof(DialogType), typeof(Type), typeof(ListViewOpenContentDialogBehavior), new PropertyMetadata(typeof(Page)));

    private async void ButtonClicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
    {
        var dialog = Activator.CreateInstance(DialogType) as ContentDialog;
        await DialogManager.OpenDialogAsync(dialog);
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.Click -= ButtonClicked;
    }
}