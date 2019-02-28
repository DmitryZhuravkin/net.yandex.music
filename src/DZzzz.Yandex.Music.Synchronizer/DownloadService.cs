using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using DZzzz.Net.Http;
using DZzzz.Net.Http.Interfaces;
using DZzzz.Net.Logging.Interfaces;
using DZzzz.Net.Logging.Model;
using DZzzz.Yandex.Music.Synchronizer.Application.Model;
using DZzzz.Yandex.Music.Synchronizer.Queue;

namespace DZzzz.Yandex.Music.Synchronizer.Application
{
    public class DownloadService : IDisposable
    {
        private readonly ISyncQueue<MusicTrack> queue;
        private readonly CancellationToken cancellationToken;
        private readonly ILogger logger;
        private readonly IMusicService musicService;
        private readonly IHttpClientFactory clientFactory = new SingleInstanceHttpClientFactory();

        private const int ThreadsCount = 30;
        private const int ParallelOperationsCount = 20;
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(ParallelOperationsCount);

        private List<Thread> threads = new List<Thread>();

        public DownloadService(ISyncQueue<MusicTrack> queue, IMusicService musicService,
            CancellationToken cancellationToken, ILogger logger)
        {
            this.queue = queue;
            this.cancellationToken = cancellationToken;
            this.logger = logger;
            this.musicService = musicService;
        }

        public void Start()
        {
            for (int i = 0; i < ThreadsCount; i++)
            {
                Thread thread = new Thread(ProcessThreadHandler) { IsBackground = true };
                thread.Start();

                threads.Add(thread);
            }
        }

        public void Dispose()
        {
            foreach (Thread thread in threads)
            {
                thread.Join(2000);
            }

            // wait all threads
            clientFactory?.Dispose();
            semaphoreSlim?.Dispose();
        }

        private void ProcessThreadHandler(object state)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (!semaphoreSlim.Wait(2000))
                    {
                        Thread.Sleep(500);
                        continue;
                    }

                    try
                    {
                        MusicTrack musicTrack = queue.TryDequeue();

                        if (musicTrack != null)
                        {
                            logger.Write<DownloadService>($"Download. Start. {musicTrack.Playlist}: {musicTrack.Title}");

                            List<string> trackLocations = musicService.GetMusicTrackLocations(musicTrack);

                            if (trackLocations != null && trackLocations.Any())
                            {
                                //DownloadAsync(musicTrack.DownloadUrl, trackLocation).GetAwaiter().GetResult();
                                logger.Write<DownloadService>($"Download. Finish. {musicTrack.Playlist}: {musicTrack.Title}. Location: {trackLocations[0]}.");
                            }
                        }
                    }
                    finally
                    {
                        semaphoreSlim.Release();
                    }
                }
            }
            catch (Exception e)
            {
                logger.Write<DownloadService>($"{nameof(ProcessThreadHandler)}", LogLevel.Error, e);
            }
        }

        private Task DownloadAsync(string requestUri, List<string> files)
        {
            if (requestUri == null)
            {
                throw new ArgumentNullException(nameof(requestUri));
            }

            return DownloadAsync(new Uri(requestUri), files);
        }

        private async Task DownloadAsync(Uri requestUri, List<string> files)
        {
            if (files == null)
            {
                throw new ArgumentNullException(nameof(files));
            }

            HttpClient httpClient = clientFactory.GetHttpClient();

            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                using (HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false))
                {
                    using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                    {
                        foreach (string file in files)
                        {
                            await SaveFileAsync(contentStream, file);
                        }
                    }
                }
            }
        }

        private async Task SaveFileAsync(Stream contentStream, string fileName)
        {
            using (Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await contentStream.CopyToAsync(stream);
            }
        }
    }
}