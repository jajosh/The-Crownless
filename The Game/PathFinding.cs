using System;
using System.Collections.Generic;
namespace The_Game;
public class PathVector
{
	public List<RootComponent> Path { get; set; }
	public PathVector()
	{
		Path = new();
	}
	public void Add(RootComponent Point)
	{
		Path.Add(Point);
	}
}
public class PathFinding
{
	
	private static int ComputedScore(RootComponent Standing, RootComponent Destination)
	{
		return Math.Abs(Standing.LocalX - Destination.LocalX) + Math.Abs(Standing.LocalY - Destination.LocalY);
	}
	public static PathVector CreatePath(RootComponent StartingPoint, RootComponent Destination)
	{
		PathVector Results = new PathVector();

        int localX = StartingPoint.LocalX;
		int localY = StartingPoint.LocalY;
		int gridX = StartingPoint.GridX;
		int gridY = StartingPoint.GridY;

        while (localX != Destination.LocalX || localY != Destination.LocalY)
        {

            if (localX < Destination.LocalX)
            {
                localX++;
            }
            else if (localX > Destination.LocalX)
            {
                localX--;
            }
            if (localY < Destination.LocalY)
            {
                localY++;
            }
            else if (localY > Destination.LocalY)
            {
                localY--;
            }
            RootComponent standing = new RootComponent(StartingPoint.GridX, StartingPoint.GridY, localX, localY);
            Results.Add(standing);
        }
        return new PathVector();
    }
}

