using SpotifyAPI.Web;

namespace SpotiSharpBackend;

public class SongData
{
    public FullTrack? FullTrack { get; set; } = new FullTrack();
    public TrackAudioFeatures? AudioFeatures { get; set; } = new TrackAudioFeatures();
}