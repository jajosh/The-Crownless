using System;

public class TileRepository
{
    public TileRepository()
    {
        using var conn = DataBase.GetConnection();
        using var cmd = conn.CreateCommand();

        cmd.CommandText =


    }
}
