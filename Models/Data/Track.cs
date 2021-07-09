namespace PlaylisterCli.Models.Data
{
    public record Track
    {
        public int DiscNumber { get; init; }

        public int DurationMs { get; init; }

        public string Id { get; init; }

        public string Name { get; init; }

        public int TrackNumber { get; init; }

        public string AlbumId { get; init; }
    }
}
