using System;

// Used to track entity position
public class RootComponent
{
    public int GridX { get; set; }
    public int GridY { get; set; }
    public int LocalX { get; set; }
    public int LocalY { get; set; }
    public RootComponent()
    { }
    public RootComponent Clone()
    {
        return new RootComponent
        {
            GridX = this.GridX,
            GridY = this.GridY,
            LocalX = this.LocalX,
            LocalY = this.LocalY
        };
    }
    public RootComponent(int gridX, int gridY, int localX, int localY)
    {
        GridX = gridX;
        GridY = gridY;
        LocalX = localX;
        LocalY = localY;
    }
}
