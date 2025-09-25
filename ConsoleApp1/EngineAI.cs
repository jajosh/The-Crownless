using System;
public enum CombatActionType { Move, Attack, BonusAction, Wait }
public class EngineAI
{
    public Action? NextAction { get; set; }

    public EngineAI()
    {

    }
    public List<CombatAction> GenerateTurnActions(IActionable Decider, List<IActionable> Enemies, List<IActionable> Allies)
    {
        return null;
    }
    public int DirectionFromTiles(Tile start, Tile nextStep)
    {
        int dx = nextStep.LocalX - start.LocalX;
        int dy = nextStep.LocalY - start.LocalY;

        if (dx == -1 && dy == 0) return 1; // North
        if (dx == 1 && dy == 0) return 2;  // South
        if (dx == 0 && dy == 1) return 3;  // East
        if (dx == 0 && dy == -1) return 4; // West

        return 0; // no movement or diagonal (invalid)
    }
    public List<int> FindBestCover(IActionable npc, Player player, List<IActionable>? enemies = null, List<IActionable>? allies = null)
    {
        List<int> moveList = new();
        return moveList;
    }
    public static IEnumerable<Tile> GetNeighbors(Tile tile, SaveGame saveGame)
    {
        var directions = new (int dx, int dy)[] { (1, 0), (-1, 0), (0, 1), (0, -1) };
        foreach (var (dx, dy) in directions)
        {
            Tile neighbor = Map.FindTile(
                saveGame.GameMap,
                tile.GridX,
                tile.GridY,
                tile.LocalX + dx,
                tile.LocalY + dy
            );

            if (neighbor != null)
                yield return neighbor;
        }
    }  
}
#region === Helper Classes ===
public class CombatAction
{
    public CombatActionType Type { get; set; }
    public Tile TargetTile { get; set; } // for movement
    public IActionable TargetEntity { get; set; } // for attack
}
public class PathNode
{
    public Tile Tile { get; }
    public PathNode Parent { get; set; }
    public int G { get; set; } // cost so far
    public int H { get; set; } // heuristic
    public int F => G + H;     // total cost

    public PathNode(Tile tile)
    {
        Tile = tile;
    }
}
public static class Pathfinding
{
    public static List<Tile> FindPath(Tile start, Tile goal, Func<Tile, IEnumerable<Tile>> getNeighbors)
    {
        var openSet = new List<PathNode>();
        var closedSet = new HashSet<Tile>();

        PathNode startNode = new PathNode(start) { G = 0, H = Heuristic(start, goal) };
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            // Pick the node with the lowest F
            PathNode current = openSet.OrderBy(n => n.F).First();

            if (current.Tile == goal)
                return ReconstructPath(current);

            openSet.Remove(current);
            closedSet.Add(current.Tile);

            foreach (var neighborTile in getNeighbors(current.Tile))
            {
                if (!neighborTile.IsWalkable || closedSet.Contains(neighborTile))
                    continue;

                int tentativeG = current.G + 1;

                var neighborNode = openSet.FirstOrDefault(n => n.Tile == neighborTile);
                if (neighborNode == null)
                {
                    neighborNode = new PathNode(neighborTile);
                    openSet.Add(neighborNode);
                }
                else if (tentativeG >= neighborNode.G)
                {
                    continue; // not a better path
                }

                // This path is the best so far
                neighborNode.Parent = current;
                neighborNode.G = tentativeG;
                neighborNode.H = Heuristic(neighborTile, goal);
            }
        }

        return new List<Tile>(); // no path
    }

    private static int Heuristic(Tile a, Tile b)
    {
        // Manhattan distance (grid-based movement)
        return Math.Abs(a.LocalX - b.LocalX) + Math.Abs(a.LocalY - b.LocalY);
    }

    private static List<Tile> ReconstructPath(PathNode node)
    {
        var path = new List<Tile>();
        while (node != null)
        {
            path.Add(node.Tile);
            node = node.Parent;
        }
        path.Reverse();
        return path;
    }
}

#endregion