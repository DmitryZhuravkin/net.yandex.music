using System.Net.Http;
using System.Threading.Tasks;

using DZzzz.Net.Http;
using DZzzz.Net.Http.Configuration;
using DZzzz.Net.Http.Interfaces;
using DZzzz.Yandex.Music.Api.Model;

namespace DZzzz.Yandex.Music.Api
{
    public class MusicServiceClient : JsonHttpServiceClient
    {
        public MusicServiceClient(
            HttpServiceClientConfiguration configuration,
            IHttpClientFactory httpClientFactory)
            : base(configuration, httpClientFactory)
        {
        }

        public Task<Library> GetLibraryAsync(string owner, string filter)
        {
            string url = $"/handlers/library.jsx?owner={owner}&filter={filter}";
            return SendRequestWithResultAsync<Library>(url, HttpMethod.Get);
        }

        public Task<PlaylistWrapper> GetUserPlaylistAsync(string owner, long playlistKind)
        {
            string url = $"/handlers/playlist.jsx?owner={owner}&kinds={playlistKind}";
            return SendRequestWithResultAsync<PlaylistWrapper>(url, HttpMethod.Get);
        }
    }
}
