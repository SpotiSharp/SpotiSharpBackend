namespace SpotiSharpBackend;

public delegate void OpenBrowser(Uri uri);

public static class MauiConnector
{
    public static event OpenBrowser OnOpenBrowser;

    public static void TriggerBrowerOpen(Uri uri)
    {
        OnOpenBrowser?.Invoke(uri);
    }
}