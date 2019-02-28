using System.Collections.Generic;

using Newtonsoft.Json;

namespace DZzzz.Yandex.Music.Api.Model
{
    public class YandexLibrary
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("playlistIds")]
        public List<long> PlaylistIds { get; set; }

        [JsonProperty("playlists")]
        public List<YandexPlaylist> Playlists { get; set; }

        [JsonProperty("hasTracks")]
        public bool HasTracks { get; set; }
    }
}