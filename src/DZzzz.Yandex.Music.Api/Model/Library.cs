using System.Collections.Generic;

using Newtonsoft.Json;

namespace DZzzz.Yandex.Music.Api.Model
{
    public class Library
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("playlistIds")]
        public List<long> PlaylistIds { get; set; }

        [JsonProperty("playlists")]
        public List<Playlist> Playlists { get; set; }

        [JsonProperty("hasTracks")]
        public bool HasTracks { get; set; }
    }
}