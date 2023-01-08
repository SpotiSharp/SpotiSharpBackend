using SpotifyAPI.Web;

namespace SpotiSharpBackend;

public delegate void AuthenticationComplete();

public static class Authentication
{
    private static string _verifier;

    private static PKCETokenResponse _initialResponse;

    private static string _clientId;
    
    private static string _clientSecret;

    private static SpotifyClient _spotifyClient;
    public static SpotifyClient? SpotifyClient { get; private set; }

    public static event AuthenticationComplete OnAuthenticate;
    
    static Authentication()
    {
        _clientId = StorageHandler.ClientId;
        string refreshToken = StorageHandler.RefreshToken;
        if (_clientId != string.Empty && refreshToken != string.Empty) RefreshAuthentication(refreshToken);
    }

    public static void Authenticate(string clientId = "")
    {
        if (clientId != string.Empty) _clientId = clientId;
        if (_spotifyClient == null)
        {
            NewAuthentication();
        }
        else
        {
            RefreshAuthentication();
        }
    }
    
    public static void UserLessAuthenticate()
    {
        _clientId = StorageHandler.ClientId;
        _clientSecret = StorageHandler.ClientSecret;
        
        var config = SpotifyClientConfig
            .CreateDefault()
            .WithAuthenticator(new ClientCredentialsAuthenticator(_clientId, _clientSecret));
        try
        {
            SpotifyClient = new SpotifyClient(config);
            OnAuthenticate?.Invoke();
        }
        catch (APIException) { }
    }

    private static void NewAuthentication()
    {
        // Generates a secure random verifier of length 100 and its challenge
        (_verifier, string challenge) = PKCEUtil.GenerateCodes();

        var loginRequest = new LoginRequest(
            new Uri("http://127.0.0.1:5000/callback"),
            _clientId,
            LoginRequest.ResponseType.Code
        )
        {
            CodeChallengeMethod = "S256",
            CodeChallenge = challenge,
            Scope = new[] {
                Scopes.Streaming, 
                Scopes.AppRemoteControl, 
                Scopes.PlaylistModifyPrivate, 
                Scopes.PlaylistModifyPublic, 
                Scopes.PlaylistReadCollaborative, 
                Scopes.PlaylistReadPrivate, 
                Scopes.UgcImageUpload, 
                Scopes.UserFollowModify, 
                Scopes.UserFollowRead, 
                Scopes.UserLibraryModify, 
                Scopes.UserLibraryRead, 
                Scopes.UserReadEmail, 
                Scopes.UserReadPrivate, 
                Scopes.UserTopRead, 
                Scopes.UserModifyPlaybackState, 
                Scopes.UserReadCurrentlyPlaying, 
                Scopes.UserReadPlaybackPosition, 
                Scopes.UserReadPlaybackState, 
                Scopes.UserReadRecentlyPlayed
            }
        };
        // start webserver for callback
        _ = CallBackListener.Instance;

        Uri uri = loginRequest.ToUri();
        MauiConnector.TriggerBrowerOpen(uri);
    }

    private static async void RefreshAuthentication(string refreshToken = null)
    {
        try
        {
            if (refreshToken == null) refreshToken = _initialResponse.RefreshToken;
            var newResponse = await new OAuthClient().RequestToken(
                new PKCETokenRefreshRequest(_clientId, refreshToken)
            );
            
            SpotifyClient = new SpotifyClient(newResponse.AccessToken);
            StorageHandler.RefreshToken = newResponse.RefreshToken;
            OnAuthenticate?.Invoke();
        }
        catch (APIException)
        {
            StorageHandler.RefreshToken = string.Empty;
        }
    }

    internal static async Task GetCallback(string code)
    {
        _initialResponse = await new OAuthClient().RequestToken(
            new PKCETokenRequest(_clientId, code, new Uri("http://127.0.0.1:5000/callback"), _verifier)
        );

        SpotifyClient = new SpotifyClient(_initialResponse.AccessToken);
        StorageHandler.ClientId = _clientId;
        StorageHandler.RefreshToken = _initialResponse.RefreshToken;
        OnAuthenticate?.Invoke();
    }
}