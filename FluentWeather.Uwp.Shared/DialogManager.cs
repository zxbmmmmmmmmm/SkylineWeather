using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace FluentWeather.Uwp.Shared;

public static class DialogManager
{
    public static ContentDialog ActiveDialog;

    private static TaskCompletionSource<bool> _dialogAwaiter = new TaskCompletionSource<bool>();

    public static async Task<ContentDialogResult> OpenDialogAsync(ContentDialog dialog, DialogShowingOption option = DialogShowingOption.AbortPrevious)
    {
        return await OpenDialog(dialog, option);
    }
    private static async Task<ContentDialogResult> OpenDialog(ContentDialog dialog, DialogShowingOption option)
    {
        TaskCompletionSource<bool> currentAwaiter = _dialogAwaiter;
        TaskCompletionSource<bool> nextAwaiter = new TaskCompletionSource<bool>();
        _dialogAwaiter = nextAwaiter;

        if (ActiveDialog != null)
        {
            switch (option)
            {
                case DialogShowingOption.AwaitPrevious:
                    await currentAwaiter.Task;
                    break;
                case DialogShowingOption.AbortPrevious:
                    //ActiveDialog.IsAborted = true;
                    ActiveDialog.Hide();
                    break;
                case DialogShowingOption.ShowIfNoActive:
                    return ContentDialogResult.None;
            }
        }
        ActiveDialog = dialog;
        var result = await dialog?.ShowAsync();
        nextAwaiter.SetResult(true);
        ActiveDialog = null;
        return result;
    }
}
public enum DialogShowingOption
{
    AwaitPrevious,
    AbortPrevious,
    ShowIfNoActive
}