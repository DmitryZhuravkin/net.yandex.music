﻿using System.Collections.Generic;

using Newtonsoft.Json;

namespace DZzzz.Yandex.Music.Api.Model
{
    public class Playlist
    {
        [JsonProperty("kind")]
        public long Kind { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("trackCount")]
        public long TrackCount { get; set; }
        
        [JsonProperty("tracks", NullValueHandling = NullValueHandling.Ignore)]
        public List<Track> Tracks { get; set; }
    }
}