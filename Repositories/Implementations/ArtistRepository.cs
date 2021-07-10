using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using PlaylisterCli.Models.Data;

namespace PlaylisterCli.Repositories.Implementations
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly ILogger<ArtistRepository> _logger;
        private readonly IConnectionFactory _connectionFactory;
        private SqliteConnection Connection => _connectionFactory.Connection;

        public ArtistRepository(IConnectionFactory connectionFactory, ILogger<ArtistRepository> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<Artist>> Search(string query)
        {
            const string searchSql =
                "SELECT id, name FROM Artist WHERE name LIKE '%' || @Query || '%'";

            await using SqliteConnection conn = Connection;

            ImmutableArray<Artist> artists = (await conn.QueryAsync<Artist>(searchSql, new {Query = query})!)
                .ToImmutableArray();

            return artists;
        }
    }
}
