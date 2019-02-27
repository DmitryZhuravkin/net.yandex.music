using System.Threading.Tasks;

using DZzzz.Net.Http;
using DZzzz.Net.Http.Configuration;
using DZzzz.Net.Http.Interfaces;
using DZzzz.Yandex.Music.Api;
using DZzzz.Yandex.Music.Api.Model;

namespace DZzzz.Yandex.Music.Synchronizer
{
    public class Program
    {
        private const string UserName = "d.zhuravkin";

        private static readonly IHttpClientFactory clientFactory = new SingleInstanceHttpClientFactory();

        public static void Main(string[] args)
        {
            ProcessAsync().GetAwaiter().GetResult();
        }

        private static async Task ProcessAsync()
        {
            HttpServiceClientConfiguration yandexApiConfiguration = new HttpServiceClientConfiguration
            {
                BaseUrl = "https://music.yandex.ru"
            };

            HttpServiceClientConfiguration storageYandexApiConfiguration = new HttpServiceClientConfiguration
            {
                BaseUrl = "http://storage.music.yandex.ru"
            };

            MusicServiceClient musicServiceClient = new MusicServiceClient(yandexApiConfiguration, clientFactory);
            MusicStorageServiceClient musicStorageServiceClient = new MusicStorageServiceClient(storageYandexApiConfiguration, clientFactory);

            Library library = await musicServiceClient.GetLibraryAsync(UserName, "playlists");

            foreach (Playlist playlistInfo in library.Playlists)
            {
                var playlistWrapper = await musicServiceClient.GetUserPlaylistAsync(UserName, playlistInfo.Kind);

                if (playlistWrapper?.Playlist != null)
                {
                    foreach (Track playlistTrack in playlistWrapper?.Playlist.Tracks)
                    {
                        string url = await musicStorageServiceClient.GetTrackDonwloadUriAsync(playlistTrack.StorageDir);
                    }
                }
            }
        }
    }
}
