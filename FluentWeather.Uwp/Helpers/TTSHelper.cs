using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Media.SpeechSynthesis;
using Windows.Storage.Streams;
using Windows.System.Display;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace FluentWeather.Uwp.Helpers;

public static class TTSHelper
{
    public static readonly MediaPlayer MediaPlayer = new ()
    {
        AutoPlay = false,
    };
    public static bool IsPlaying => MediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Playing;

    public static async void Speech(string text)
    {
        if(MediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Playing)
        {
            MediaPlayer.Pause();
            return;
        }
        var stream = await new SpeechSynthesizer().SynthesizeTextToStreamAsync(text);
        var streamRef = RandomAccessStreamReference.CreateFromStream(stream);
        MediaPlayer.Source = MediaSource.CreateFromStreamReference(streamRef, stream.ContentType);
        MediaPlayer.Play();
    }

}