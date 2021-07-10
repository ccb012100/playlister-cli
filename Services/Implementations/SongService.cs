using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using PlaylisterCli.Models;
using PlaylisterCli.Repositories;
using PlaylisterCli.Utilities;
using Spectre.Console;

namespace PlaylisterCli.Services.Implementations
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _repository;

        public SongService(ISongRepository repository)
        {
            _repository = repository;
        }

        public async Task Search()
        {
            string query = SearchUtility.GetUserSearch();

            ImmutableArray<Song> songs = (await _repository.Search(query)!).ToImmutableArray();

            if (!songs.Any())
            {
                AnsiConsole.MarkupLine($"[red]No matches for {query}[/]");
                return;
            }

            RenderSongTable(songs, query);
        }

        private static void RenderSongTable(ImmutableArray<Song> songs, string query)
        {
            var table = new Table {Caption = new TableTitle($"{songs.Length} matches for \"{query}\"")};
            table.Border(TableBorder.Rounded)
                .AddColumn("Name").AddColumn("Artists").AddColumn("Album").AddColumn("Length")
                .Columns[3].RightAligned();

            foreach (Song song in songs.OrderBy(x => x.Name))
            {
                AddSongRow(table, song);
            }

            AnsiConsole.Render(table);
        }

        private static void AddSongRow(Table table, Song song) =>
            table.AddRow(song.Name.EscapeMarkup(),
                song.Artists.Select(x => x.Name).Aggregate((v1, v2) => $"{v1}, {v2}").EscapeMarkup(),
                song.Album.Name.EscapeMarkup(), song.Duration.EscapeMarkup());
    }
}
