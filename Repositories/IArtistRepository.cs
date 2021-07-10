using System.Collections.Generic;
using System.Threading.Tasks;
using PlaylisterCli.Models.Data;

namespace PlaylisterCli.Repositories
{
    public interface IArtistRepository
    {
        /// <summary>
        /// Search for artists with name containing <paramref name="query"/>
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Collection of artists matching the query.</returns>
        Task<IEnumerable<Artist>> Search(string query);
    }
}
