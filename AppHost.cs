using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PlaylisterCli.Models;
using PlaylisterCli.Services;
using Spectre.Console;

namespace PlaylisterCli
{
    public class AppHost
    {
        private readonly ILogger<AppHost> _logger;
        private readonly ISongService _songService;
        private readonly IArtistService _artistService;

        public AppHost(ILogger<AppHost> logger, ISongService songService, IArtistService artistService)
        {
            _logger = logger;
            _songService = songService;
            _artistService = artistService;
        }

        public async Task Run()
        {
            _logger.LogInformation("Running AppHost");

            MenuOption selection;

            while ((selection = GetUserMenuSelection()) is not MenuOption.Exit)
            {
                AnsiConsole.MarkupLine($"[yellow]you selected [bold orange3]{selection}[/][/]");

                switch (selection)
                {
                    case MenuOption.SongSearch:
                        await _songService.Search();
                        break;
                    case MenuOption.ArtistSearch:
                        await _artistService.Search();
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
                .AddChoices(
                    MenuOption.SongSearch,
                    MenuOption.ArtistSearch,
                    MenuOption.AlbumSearch,
                    MenuOption.PlaylistSearch,
                    MenuOption.Exit);

            return AnsiConsole.Prompt(prompt);
        }
    }
}
