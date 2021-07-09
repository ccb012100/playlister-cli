using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PlaylisterCli.Models;
using PlaylisterCli.Models.Data;
using PlaylisterCli.Services;
using Spectre.Console;

namespace PlaylisterCli
{
    public class AppHost
    {
        private readonly ILogger<AppHost> _logger;
        private readonly ISearchService _searchService;

        public AppHost(ILogger<AppHost> logger, ISearchService searchService)
        {
            _logger = logger;
            _searchService = searchService;
        }

        public async Task Run()
        {
            _logger.LogInformation("Running AppHost");

            MenuOption selection;

            while ((selection = GetUserMenuSelection()) is not MenuOption.Exit)
            {
                AnsiConsole.MarkupLine($"[yellow]you selected {selection}[/]");
                switch (selection)
                {
                    case MenuOption.SongSearch:
                        await _searchService.SearchSongs();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"Invalid selection {selection}");
                }
            }
        }

        private static MenuOption GetUserMenuSelection()
        {
            SelectionPrompt<MenuOption> prompt = new SelectionPrompt<MenuOption>()
                .Title("[green]Choose an option:[/]")
                .PageSize(10)
                .MoreChoicesText("[blue]Scroll up/down to see more options[/]")
                .AddChoices(MenuOption.SongSearch, MenuOption.Exit);

            return AnsiConsole.Prompt(prompt);
        }

        private static Table CreatePlaylistTable(IEnumerable<Playlist> playlists)
        {
            var table = new Table {Caption = new TableTitle("Playlists")};
            table.AddColumn("Name").AddColumn("Description").AddColumn("Count");

            foreach (Playlist playlist in playlists.OrderBy(x => x.Name))
            {
                AddPlaylistRow(table, playlist);
            }

            return table;
        }

        private static void AddPlaylistRow(Table table, Playlist plist)
        {
            table.AddRow(plist.Name, plist.Description ?? "", plist.Count.ToString());
        }
    }
}
