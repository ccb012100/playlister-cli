using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PlaylisterCli.Enums;
using PlaylisterCli.Models;
using PlaylisterCli.Models.Data;

namespace PlaylisterCli.Repositories.Implementations
{
    public class SongRepository : ISongRepository
    {
        private readonly ILogger<SongRepository> _logger;
        private readonly IConnectionFactory _connectionFactory;

        private SqliteConnection Connection => _connectionFactory.Connection;

        public SongRepository(ILogger<SongRepository> logger, IOptions<DatabaseOptions> options,
            IConnectionFactory connectionFactory)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Song>> Search(string query)
        {
            const string searchTrackSql =
                "SELECT id, name, track_number, disc_number, duration_ms, album_id FROM Track WHERE name LIKE '%' || @Query || '%'";

            const string trackArtistKeySql =
                "SELECT track_id, artist_id, name as ArtistName FROM TrackArtist TA JOIN Artist A ON TA.artist_id = A.id WHERE TA.track_id IN @TrackIds";

            const string albumSql =
                "SELECT id, name, total_tracks, album_type, release_date FROM Album WHERE id IN @AlbumIds";

            await using SqliteConnection conn = Connection;

            ImmutableArray<Track> tracks = (await conn.QueryAsync<Track>(searchTrackSql, new {Query = query})!)
                .ToImmutableArray();

            DynamicParameters trackIds = new();
            trackIds.Add("@TrackIds", tracks.Select(track => track.Id));

            DynamicParameters albumIds = new();
            albumIds.Add("@AlbumIds", tracks.Select(track => track.AlbumId));

            ImmutableArray<TrackArtistKey> trackArtistKeys =
                (await conn.QueryAsync<TrackArtistKey>(trackArtistKeySql, trackIds)!).ToImmutableArray();

            ImmutableArray<Album> albums = (await conn.QueryAsync<Album>(albumSql, albumIds)!).ToImmutableArray();

            IEnumerable<Song> songs = tracks
                .Select(track => new Song(track, albums.Single(x => x.Id == track.AlbumId),
                    trackArtistKeys.Where(x => x.TrackId == track.Id).Select(y => y.ToArtist())
                ));

            return songs;
        }
    }
}
