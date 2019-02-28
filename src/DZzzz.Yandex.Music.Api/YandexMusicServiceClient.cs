using System.Net.Http;
using System.Threading.Tasks;

using DZzzz.Net.Http;
using DZzzz.Net.Http.Configuration;
using DZzzz.Net.Http.Interfaces;
using DZzzz.Yandex.Music.Api.Model;

namespace DZzzz.Yandex.Music.Api
{
    public class YandexMusicServiceClient : JsonHttpServiceClient
    {
        public YandexMusicServiceClient(
            HttpServiceClientConfiguration configuration,
            IHttpClientFactory httpClientFactory)
            : base(configuration, httpClientFactory)
        {
        }

        public Task<YandexLibrary> GetLibraryAsync(string owner, string filter)
        {
            string url = $"/handlers/library.jsx?owner={owner}&filter={filter}";
            return SendRequestWithResultAsync<YandexLibrary>(url, HttpMethod.Get);
        }

        public Task<YandexPlaylistWrapper> GetUserPlaylistAsync(string owner, long playlistKind)
        {
            string url = $"/handlers/playlist.jsx?owner={owner}&kinds={playlistKind}";
            return SendRequestWithResultAsync<YandexPlaylistWrapper>(url, HttpMethod.Get);
        }
    }
}
