namespace SpotiSharpBackend;

public delegate void DataChange();

public static class StorageHandler
{
    private static string _clientId = string.Empty;
    
    public static string ClientId
    {
        get { return _clientId; }
        set
        {
            _clientId = value;
            OnDataChange?.Invoke();
        }
    }
    
    private static string _refreshToken = string.Empty;

    public static string RefreshToken
    {
        get { return _refreshToken; }
        set
        {
            _refreshToken = value;
            OnDataChange?.Invoke();
        }
    }
    
    public static event DataChange OnDataChange;
}