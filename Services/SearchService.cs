using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using PlaylisterCli.Models;
using PlaylisterCli.Models.Data;
using PlaylisterCli.Repositories;
using Spectre.Console;

namespace PlaylisterCli.Services
{
    public interface ISearchService
    {
        Task SearchSongs();
    }

    public class SearchService : ISearchService
    {
        private readonly IDbRepository _repository;

        public SearchService(IDbRepository repository)
        {
            _repository = repository;
        }

        public async Task SearchSongs()
        {
            string query;

            while ((query = AnsiConsole.Ask<string>("[blue]enter your search[/]")).Length < 3)
            {
                AnsiConsole.MarkupLine("You search term must be at least 3 characters.");
            }

            ImmutableArray<Song> songs = (await _repository.SongSearch(query)!).ToImmutableArray();

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
