namespace PlaylisterCli.Models.Data
{
    public record Playlist
    {
        public string Id { get; init; }

        public string? SnapshotId { get; init; }

        public string Name { get; init; }

        public bool Collaborative { get; init; }

        public string? Description { get; init; }

        public bool? Public { get; init; }

        public int Count { get; init; }

        public override string ToString() => Name;
    }
}
