using System;
using System.Threading;

using DZzzz.Net.Logging.Interfaces;
using DZzzz.Yandex.Music.Synchronizer.Application;
using DZzzz.Yandex.Music.Synchronizer.Application.Model;
using DZzzz.Yandex.Music.Synchronizer.Logging;
using DZzzz.Yandex.Music.Synchronizer.Queue;

namespace DZzzz.Yandex.Music.Synchronizer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            CancellationTokenSource tokenSource = new CancellationTokenSource();

            ISyncQueue<MusicTrack> musicTrackQueue = new MemorySyncQueue<MusicTrack>();
            IMusicService musicService = new MusicService(@"e:\music\");
            ILogger logger = new ConsoleLogger();

            YandexQueueBuilder queueBuilder = new YandexQueueBuilder(musicTrackQueue, musicService, tokenSource.Token, logger);

            using (DownloadService downloadService = new DownloadService(musicTrackQueue, musicService, tokenSource.Token, logger))
            {
                downloadService.Start();

                queueBuilder.BuildDownloadQueueAsync();

                Console.ReadKey(true);

                tokenSource.Cancel();
            }
        }
    }
}
