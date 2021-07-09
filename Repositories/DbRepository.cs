using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PlaylisterCli.Enums;
using PlaylisterCli.Models;
using PlaylisterCli.Models.Data;
using PlaylisterCli.Utilities;

namespace PlaylisterCli.Repositories
{
    public class DbRepository : IDbRepository
    {
        private readonly ILogger<DbRepository> _logger;
        private readonly string _connectionString;

        private SqliteConnection Connection => new(_connectionString);

        public DbRepository(ILogger<DbRepository> logger, IOptions<DatabaseOptions> options)
        {
            _logger = logger;

            _connectionString = new SqliteConnectionStringBuilder(options.Value.ConnectionString)
            {
                Mode = SqliteOpenMode.ReadOnly,
                Cache = SqliteCacheMode.Shared,
                ForeignKeys = true
            }.ToString();
        }

        public async Task<IEnumerable<Playlist>> GetAll()
        {
            await using SqliteConnection conn = Connection;
            return await conn.QueryAsync<Playlist>("SELECT * FROM Playlist");
        }

        public async Task<IEnumerable<Song>> SongSearch(string query)
        {
            const string searchTrackSql =
                "SELECT id, name, track_number, disc_number, duration_ms, album_id FROM Track WHERE name LIKE '%' || @Query || '%'";

            const string trackArtistKeySql =
                "SELECT track_id, artist_id, name as ArtistName FROM TrackArtist TA JOIN Artist A ON TA.artist_id = A.id WHERE TA.track_id IN @TrackIds";

            const string albumSql =
                "SELECT id, name, total_tracks, album_type, cast(release_date as string) FROM Album WHERE id IN @AlbumIds";

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
