using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Media.SpeechSynthesis;
using Windows.Storage.Streams;

namespace FluentWeather.Uwp.Helpers;

public static class TTSHelper
{
    public static MediaPlayer MediaPlayer;
    public static bool IsPlaying => MediaPlayer is null ? false : MediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Playing;

    public static async void Speech(string text)
    {
        MediaPlayer ??= new();
        if (MediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Playing)
        {
            MediaPlayer.Pause();
            return;
        }
        using var stream = await new SpeechSynthesizer().SynthesizeTextToStreamAsync(text);
        var streamRef = RandomAccessStreamReference.CreateFromStream(stream);
        MediaPlayer.Source = MediaSource.CreateFromStreamReference(streamRef, stream.ContentType);
        MediaPlayer.Play();
    }

}