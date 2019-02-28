using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DZzzz.Net.Http;
using DZzzz.Net.Http.Configuration;
using DZzzz.Net.Http.Interfaces;
using DZzzz.Net.Logging.Interfaces;
using DZzzz.Yandex.Music.Api;
using DZzzz.Yandex.Music.Api.Model;
using DZzzz.Yandex.Music.Synchronizer.Application;
using DZzzz.Yandex.Music.Synchronizer.Application.Model;
using DZzzz.Yandex.Music.Synchronizer.Queue;

namespace DZzzz.Yandex.Music.Synchronizer
{
    public class YandexQueueBuilder
    {
        private readonly ISyncQueue<MusicTrack> queue;
        private readonly IMusicService musicService;
        private readonly CancellationToken cancellationToken;
        private readonly ILogger logger;

        private readonly HttpServiceClientConfiguration yandexApiConfiguration;
        private readonly HttpServiceClientConfiguration storageYandexApiConfiguration;

        private const string UserName = "d.zhuravkin";

        public YandexQueueBuilder(ISyncQueue<MusicTrack> queue, IMusicService musicService, CancellationToken cancellationToken, ILogger logger)
        {
            this.queue = queue;
            this.musicService = musicService;
            this.cancellationToken = cancellationToken;
            this.logger = logger;

            yandexApiConfiguration = new HttpServiceClientConfiguration { BaseUrl = "https://music.yandex.ru" };
            storageYandexApiConfiguration = new HttpServiceClientConfiguration { BaseUrl = "http://storage.music.yandex.ru" };
        }

        public async Task BuildDownloadQueueAsync()
        {
            using (IHttpClientFactory yandexApiClientFactory = new SingleInstanceHttpClientFactory(),
                storageYandexApiClientFactory = new SingleInstanceHttpClientFactory())
            {
                YandexMusicServiceClient musicServiceClient = new YandexMusicServiceClient(yandexApiConfiguration, yandexApiClientFactory);
                YandexMusicStorageServiceClient musicStorageServiceClient = new YandexMusicStorageServiceClient(storageYandexApiConfiguration, storageYandexApiClientFactory);

                YandexLibrary library = await musicServiceClient.GetLibraryAsync(UserName, "playlists").ConfigureAwait(false);

                List<Task<YandexPlaylistWrapper>> tasks = new List<Task<YandexPlaylistWrapper>>();

                foreach (YandexPlaylist playlistInfo in library.Playlists)
                {
                    tasks.Add(musicServiceClient.GetUserPlaylistAsync(UserName, playlistInfo.Kind));
                }

                YandexPlaylistWrapper[] playlists = await Task.WhenAll(tasks);

                foreach (YandexPlaylistWrapper yandexPlaylistWrapper in playlists)
                {
                    if (yandexPlaylistWrapper?.Playlist != null)
                    {
                        await ProcessYandexPlaylistAsync(musicStorageServiceClient, yandexPlaylistWrapper.Playlist).ConfigureAwait(false);
                    }
                }
            }
        }

        private async Task ProcessYandexPlaylistAsync(YandexMusicStorageServiceClient musicStorageServiceClient, YandexPlaylist playlist)
        {
            foreach (YandexTrack yandexTrack in playlist.Tracks)
            {
                MusicTrack track = BuildMusicTrack(yandexTrack, playlist.Title);

                if (!musicService.IsTrackAlreadyExists(track))
                {
                    track.DownloadUrl = await musicStorageServiceClient.GetTrackDonwloadUriAsync(yandexTrack.StorageDir).ConfigureAwait(false);

                    queue.Enqueue(track);
                }
            }
        }

        private MusicTrack BuildMusicTrack(YandexTrack playlistTrack, string playlist)
        {
            MusicTrack track = new MusicTrack(playlistTrack.Title, playlist);

            playlistTrack.Artists?.ForEach(c => track.Artists.Add(new MusicArtist
            {
                Title = c.Name
            }));

            return track;
        }
    }
}