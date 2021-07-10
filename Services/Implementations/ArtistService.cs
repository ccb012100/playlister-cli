using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using PlaylisterCli.Models.Data;
using PlaylisterCli.Repositories;
using PlaylisterCli.Utilities;
using Spectre.Console;

namespace PlaylisterCli.Services.Implementations
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _repository;

        public ArtistService(IArtistRepository repository)
        {
            _repository = repository;
        }

        public async Task Search()
        {
            string query = SearchUtility.GetUserSearch();

            ImmutableArray<Artist> artists = (await _repository.Search(query)!).ToImmutableArray();

            if (!artists.Any())
            {
                AnsiConsole.MarkupLine($"[red]No matches for {query}[/]");
                return;
            }

            AnsiConsole.MarkupLine($"[green][bold]{artists.Length}[/] matches for [italic]{query}[/][/]");

            foreach (Artist artist in artists)
            {
                AnsiConsole.MarkupLine(artist.Name.EscapeMarkup());
            }
        }
    }
}
