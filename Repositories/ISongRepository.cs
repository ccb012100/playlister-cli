using System.Collections.Generic;
using System.Threading.Tasks;
using PlaylisterCli.Models;
using PlaylisterCli.Models.Data;

namespace PlaylisterCli.Repositories
{
    public interface ISongRepository
    {
        /// <summary>
        /// Search for songs with name containing <paramref name="query"/>
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Collection of songs matching the query.</returns>
        Task<IEnumerable<Song>> Search(string query);
    }

    public interface IPlaylistRepository
    {
        /// <summary>
        /// Get all playlists in database.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Playlist>> GetAll();
    }
}
