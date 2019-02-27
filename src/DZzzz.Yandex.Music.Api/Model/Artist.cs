using Newtonsoft.Json;

namespace DZzzz.Yandex.Music.Api.Model
{
    public class Artist
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("various")]
        public bool Various { get; set; }

        [JsonProperty("composer")]
        public bool Composer { get; set; }
    }
}