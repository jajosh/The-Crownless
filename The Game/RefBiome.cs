using System;
public enum GridBiomeType // The biome of the grid
{
    Any = 1,
    BorelForest =2,
    TemperateBroadleafForest= 3,

}
public enum GridBiomeSubType // What features the grid has
{
    Any = 1, // Used for random description
    AbandonedBuilding = 2,
    Town = 3,
    HighTown = 4,
    LowTown = 5,
    RoyalTown = 6,
    Farm = 7,
    Forest = 8

}
/// <summary>
/// Allows the Grid Tile Description to hold either BridBiomeType or GridBiomeSubType
/// </summary>
public struct RefBiome
{
    //var desc1 = new BiomeRef(GridBiomeType.Forest);
    //var desc2 = new BiomeRef(GridBiomeSubType.DarkForest);
    //var desc3 = BiomeRef.Any;  // matches everything
    public GridBiomeType? Main { get; }
    public GridBiomeSubType? Sub { get; }
    public bool IsAny { get; }   // <--- wildcard flag

    // Constructors
    public RefBiome(GridBiomeType type)
    {
        Main = type;
        Sub = null;
        IsAny = false;
    }

    public RefBiome(GridBiomeSubType subtype)
    {
        Main = null;
        Sub = subtype;
        IsAny = false;
    }

    private RefBiome(bool any)   // special ctor for Any
    {
        Main = null;
        Sub = null;
        IsAny = any;
    }

    public static RefBiome Any => new RefBiome(true);

    public override string ToString()
    {
        if (IsAny) return "Any";
        return Main?.ToString() ?? Sub?.ToString() ?? "Unknown";
    }
}