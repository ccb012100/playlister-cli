using System.Collections.Generic;
using System.Linq;
using PlaylisterCli.Models.Data;
using Spectre.Console;

namespace PlaylisterCli.Services.Implementations
{
    public class PlaylistService : IPlaylistService
    {
        private static Table CreatePlaylistTable(IEnumerable<Playlist> playlists)
        {
            var table = new Table {Caption = new TableTitle("Playlists")};

            table.Border(TableBorder.Rounded)
                .AddColumn("Name").AddColumn("Description").AddColumn("Count")
                .Columns[2].RightAligned();

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
