using System;
using System.Collections.Generic;
using PlaylisterCli.Models.Data;

namespace PlaylisterCli.Models
{
    public record Song
    {
        public Song(Track track, Album album, IEnumerable<Artist> artists)
        {
            Album = album;
            Artists = artists;

            DurationMs = track.DurationMs;
            DiscNumber = track.DiscNumber;
            Id = track.Id;
            Name = track.Name;
            TrackNumber = track.TrackNumber;
        }

        public Album Album { get; init; }

        public IEnumerable<Artist> Artists { get; init; }

        public int DiscNumber { get; init; }

        public int DurationMs { get; init; }

        public string Id { get; init; }

        public string Name { get; init; }

        public int TrackNumber { get; init; }

        /// <summary>
        /// DurationMs formatted as mm:ss
        /// </summary>
        public string Duration
        {
            get
            {
                TimeSpan timespan = TimeSpan.FromMilliseconds(DurationMs);
                return $"{(int) timespan.TotalMinutes}:{timespan.Seconds:00}";
            }
        }
    }
}
