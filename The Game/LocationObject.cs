using System;
using System.ComponentModel.DataAnnotations.Schema;
public enum LocationType
{
    Settlement,
    Dungeon,
    Link,

}
// Localizes the grids into "chunks" for NPC AI, and easier sorting
public class LocationObject
{
    public LocationType Type { get; set; }
    public int ID { get; set; }
    public string Name { get; set; }
    [NotMapped]
    public List<TransitionPoint> TransitionPoints { get; set; }// Stand here, to go there
    [NotMapped]
    public List<GridObject> LocationMap { get; set; }

    public LocationObject()
    {
        LocationMap = new List<GridObject>();
        Name = string.Empty;
        TransitionPoints = new List<TransitionPoint>();
    }
}
public class TransitionPoint
{
    public int ID { get; set; }
    public int LocationID { get; set; }
    public RootComponent From { get; set; }
    public RootComponent To { get; set; }
}