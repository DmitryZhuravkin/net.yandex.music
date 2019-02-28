using System.Collections.Generic;

using DZzzz.Yandex.Music.Synchronizer.Application.Model;

namespace DZzzz.Yandex.Music.Synchronizer.Application
{
    public interface IMusicService
    {
        bool IsTrackAlreadyExists(MusicTrack track);
        List<string> GetMusicTrackLocations(MusicTrack track);
    }
}