using FluentWeather.Uwp.Pages;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Behaviors;
using Microsoft.Xaml.Interactivity;
using System;
using Windows.UI.Xaml;

namespace FluentWeather.Uwp.Behaviors;

public class AnimateBackgroundAction : DependencyObject, IAction
{

    /// <summary>
    /// Gets or sets the linked <see cref="AnimationSet"/> instance to invoke.
    /// </summary>
    public AnimationSet Animation
    {
        get => (AnimationSet)GetValue(AnimationProperty);
        set => SetValue(AnimationProperty, value);
    }

    /// <summary>
    /// Identifies the <seealso cref="Animation"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty AnimationProperty = DependencyProperty.Register(
        nameof(Animation),
        typeof(AnimationSet),
        typeof(AnimateBackgroundAction),
        new PropertyMetadata(null));


    /// <inheritdoc/>
    public object Execute(object sender, object parameter)
    {
        if (Animation is null)
        {
            ThrowArgumentNullException();
        }

        Animation.Start(RootPage.Instance.BackgroundImage);

        return null!;

        static void ThrowArgumentNullException() => throw new ArgumentNullException(nameof(Animation));
    }
}