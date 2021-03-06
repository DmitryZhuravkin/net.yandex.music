﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using DZzzz.Yandex.Music.Synchronizer.Application.Model;

namespace DZzzz.Yandex.Music.Synchronizer.Application
{
    public class MusicService : IMusicService
    {
        private readonly string library;
        private const string NewPlaylistName = "new";

        public MusicService(string library)
        {
            this.library = library;

            CreateFolderIfNoExists(library);
        }

        public bool IsTrackAlreadyExists(MusicTrack track)
        {
            string trackLocation = BuildMusicTrackLocation(track, track.Playlist);

            return File.Exists(trackLocation);
        }

        public List<string> GetMusicTrackLocations(MusicTrack track)
        {
            List<string> locations = new List<string>
            {
                BuildMusicTrackLocation(track, track.Playlist)
            };

            if (!IsTrackAlreadyExists(track))
            {
                string newPlayListLocation = BuildMusicTrackLocation(track, NewPlaylistName);
                locations.Add(newPlayListLocation);
            }

            return locations;
        }

        public void UpdateMusicTrackTags(MusicTrack track, List<string> musicTrackLocations)
        {
            // TODO:
        }

        private string BuildMusicTrackLocation(MusicTrack musicTrack, string playlist)
        {
            string playlistSp = RemoveSpecialCharactersFromPath(playlist);
            string playlistFolderLocation = Path.Combine(library, playlistSp);
            CreateFolderIfNoExists(playlistFolderLocation);

            string artist = RemoveSpecialCharactersFromFileName(musicTrack.GetArtistName());
            string title = RemoveSpecialCharactersFromFileName(musicTrack.Title);

            string fileLocation = Path.Combine(playlistFolderLocation, $"{GetSmallTitle(artist)} - {title}.{musicTrack.Extension}");

            return fileLocation;
        }

        private string GetSmallTitle(string value, int limit = 20)
        {
            if (!string.IsNullOrWhiteSpace(value) && value.Length > limit)
            {
                return value.Substring(0, limit - 3) + "...";
            }

            return value;
        }

        private static string RemoveSpecialCharactersFromFileName(string str)
        {
            if (!String.IsNullOrWhiteSpace(str))
            {
                return str.Trim(Path.GetInvalidFileNameChars()).Replace("\\", "-").Replace("/", "-");
            }

            return str;
        }

        private static string RemoveSpecialCharactersFromPath(string str)
        {
            if (!String.IsNullOrWhiteSpace(str))
            {
                return str.Trim(Path.GetInvalidPathChars());
            }

            return str;
        }

        private static void CreateFolderIfNoExists(string folder)
        {
            try
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
            }
            catch
            {
                // DO NOTHING
            }
        }
    }
}