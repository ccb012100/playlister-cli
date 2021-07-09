using System.Collections.Generic;
using System.Threading.Tasks;
using PlaylisterCli.Models;
using PlaylisterCli.Models.Data;

namespace PlaylisterCli.Repositories
{
    public interface IDbRepository
    {
        /// <summary>
        /// Get all playlists in database.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Playlist>> GetAll();

        /// <summary>
        /// Search for songs with name containing <paramref name="query"/>
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Collection of songs matching the query.</returns>
        Task<IEnumerable<Song>> SongSearch(string query);
    }
}
