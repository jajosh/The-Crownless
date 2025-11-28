using System;

// Used to track entity position
public class RootComponent : IEquatable<RootComponent>
{
    public int GridX { get; set; }
    public int GridY { get; set; }
    public int LocalX { get; set; }
    public int LocalY { get; set; }
    //Private constructor for EF core
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
    public bool Equals(RootComponent? other)
            => other is not null &&
               GridX == other.GridX &&
               GridY == other.GridY &&
               LocalX == other.LocalX &&
               LocalY == other.LocalY;

    public override bool Equals(object? obj) => Equals(obj as RootComponent);

    public override int GetHashCode() => HashCode.Combine(GridX, GridY, LocalX, LocalY);
}
