using Microsoft.Data.Sqlite;

namespace PlaylisterCli.Repositories
{
    public interface IConnectionFactory
    {
        SqliteConnection Connection { get; }
    }
}
