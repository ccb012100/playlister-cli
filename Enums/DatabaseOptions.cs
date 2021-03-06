namespace PlaylisterCli.Enums
{
    public record DatabaseOptions
    {
        public const string Database = "Database";

        public string ConnectionString { get; init; }
    }
}
