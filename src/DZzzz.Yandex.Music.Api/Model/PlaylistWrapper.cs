using Newtonsoft.Json;

namespace DZzzz.Yandex.Music.Api.Model
{
    public class PlaylistWrapper
    {
        [JsonProperty("playlist")]
        public Playlist Playlist { get; set; }
    }
}