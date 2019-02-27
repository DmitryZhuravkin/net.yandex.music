using System.Collections.Generic;
using Newtonsoft.Json;

namespace DZzzz.Yandex.Music.Api.Model
{
    public class Track
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("realId")]
        public long? RealId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("storageDir")]
        public string StorageDir { get; set; }

        [JsonProperty("artists")]
        public List<Artist> Artists { get; set; }
    }
}