using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using PlaylisterCli.Models.Data;

namespace PlaylisterCli.Repositories.Implementations
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly ILogger<PlaylistRepository> _logger;
        private readonly IConnectionFactory _connectionFactory;

        public PlaylistRepository(IConnectionFactory connectionFactory, ILogger<PlaylistRepository> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        private SqliteConnection Connection => _connectionFactory.Connection;

        public async Task<IEnumerable<Playlist>> GetAll()
        {
            await using SqliteConnection conn = Connection;
            return await conn.QueryAsync<Playlist>("SELECT * FROM Playlist");
        }
    }
}