namespace SpotiSharpBackend;

public delegate void DataChange(string key, string value);

public static class StorageHandler
{
    private static string _clientId = string.Empty;
    
    public static string ClientId
    {
        get { return _clientId; }
        set
        {
            _clientId = value;
            OnDataChange?.Invoke("clientId", value);
        }
    }
    
    private static string _refreshToken = string.Empty;

    public static string RefreshToken
    {
        get { return _refreshToken; }
        set
        {
            _refreshToken = value;
            OnDataChange?.Invoke("refreshToken", value);
        }
    }
    
    public static event DataChange OnDataChange;
}