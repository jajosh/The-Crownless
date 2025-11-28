using System;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Data.Sqlite;
public class GameDataBase
{
    public string  dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data\\TheCrownlessDB.db");

    public string  connectionString;

	public static SqliteConnection MyConnection = new SqliteConnection();
    public GameDataBase()
	{
        connectionString = $"Data Source={dbPath};";
        MyConnection = new SqliteConnection(connectionString);
        MyConnection.Open();
	}
    public static void CloseConnection()
    {
        MyConnection.Close();
    }
}
