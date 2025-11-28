using Microsoft.Data.Sqlite;

public static class DataBase
{
    private const string ConnectionString = "Data Source=game.db";
    public static SqliteConnection GetConnection()
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        return connection;
    }
}