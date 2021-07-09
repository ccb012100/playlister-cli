namespace PlaylisterCli.Models.Data
{
    public record TrackArtistKey
    {
        public string TrackId { get; init; }
        public string ArtistId { get; init; }
        public string ArtistName { get; init; }

        public Artist ToArtist() => new() { Id = ArtistId, Name = ArtistName};
    }
}
