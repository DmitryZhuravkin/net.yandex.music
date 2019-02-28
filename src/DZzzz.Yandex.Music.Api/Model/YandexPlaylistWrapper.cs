using Newtonsoft.Json;

namespace DZzzz.Yandex.Music.Api.Model
{
    public class YandexPlaylistWrapper
    {
        [JsonProperty("playlist")]
        public YandexPlaylist Playlist { get; set; }
    }
}