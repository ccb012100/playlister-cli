using System.Collections.Generic;

namespace PlaylisterCli.Models.Data
{
    public record ArtistDiscography
    {
        public Artist Artist { get; init; }
        public IEnumerable<Album> Albums { get; init; }
    }
}