using System.Collections.Generic;

namespace DZzzz.Yandex.Music.Synchronizer.Application.Model
{
    public class MusicTrack
    {
        #region properties

        public string Title { get; }

        public string Extension { get; } = "mp3";

        public string Playlist { get; }

        public string DownloadUrl { get; set; }

        public List<MusicArtist> Artists { get; } = new List<MusicArtist>();

        #endregion
        #region methods

        #region ctor/dtor

        public MusicTrack(string title, string playlist)
        {
            Title = title;
            Playlist = playlist;
        }

        #endregion

        #endregion
    }
}