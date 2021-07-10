using Spectre.Console;

namespace PlaylisterCli.Utilities
{
    public static class SearchUtility
    {
        private const int MinQueryLength = 3;

        public static string GetUserSearch()
        {
            string query;

            while ((query = AnsiConsole.Ask<string>("[blue]Enter your search[/]")).Length < MinQueryLength)
            {
                AnsiConsole.MarkupLine("You search term must be at least 3 characters.");
            }

            return query;
        }
    }
}