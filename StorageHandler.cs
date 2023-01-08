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
    
    private static string _clientSecret = string.Empty;
    
    public static string ClientSecret
    {
        get { return _clientSecret; }
        set
        {
            _clientSecret = value;
            OnDataChange?.Invoke("clientSecret", value);
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

    private static bool _isUsingCollaborationHost = false;

    public static bool IsUsingCollaborationHost
    {
        get { return _isUsingCollaborationHost; }
        set
        {
            _isUsingCollaborationHost = value;
            OnDataChange?.Invoke("isUsingCollaborationHost", value ? "1" : "0");
        }
    }
    
    private static string _collaborationHostAddress = string.Empty;

    public static string CollaborationHostAddress
    {
        get { return _collaborationHostAddress; }
        set
        {
            _collaborationHostAddress = value;
            OnDataChange?.Invoke("collaborationHostAddress", value);
        }
    }
    
    private static string _collaborationSession = string.Empty;
    
    public static string CollaborationSession
    {
        get { return _collaborationSession; }
        set
        {
            _collaborationSession = value;
            OnDataChange?.Invoke("collaborationSession", value);
        }
    }
    
    public static event DataChange OnDataChange;
}