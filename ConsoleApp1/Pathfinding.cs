using System;

/// <summary>
/// Node used in pathfinding algorithm to track path costs and parents.
/// </summary>
public class PathNode
{
    public Tile Tile { get; }
    public PathNode Parent { get; set; }
    public int G { get; set; } // cost so far
    public int H { get; set; } // heuristic
    public int F => G + H;     // total cost

    /// <summary>
    /// Constructor for PathNode.
    /// </summary>
    /// <param name="tile">The tile for this node.</param>
    public PathNode(Tile tile)
    {
        Tile = tile;
    }
}

/// <summary>
/// Static class providing pathfinding utilities using A* algorithm.
/// </summary>
public static class Pathfinding
{
    /// <summary>
    /// Finds the shortest path from start to goal using A* with provided neighbor function.
    /// </summary>
    /// <param name="start">Starting tile.</param>
    /// <param name="goal">Goal tile.</param>
    /// <param name="getNeighbors">Function to get neighbors for a tile.</param>
    /// <returns>List of tiles forming the path, or empty if no path.</returns>
    public static List<Tile> FindPath(Tile start, Tile goal, Func<Tile, IEnumerable<Tile>> getNeighbors)
    {
        // Open set for nodes to evaluate.
        var openSet = new List<PathNode>();
        // Closed set for visited tiles.
        var closedSet = new HashSet<Tile>();

        // Initialize start node.
        PathNode startNode = new PathNode(start) { G = 0, H = Heuristic(start, goal) };
        openSet.Add(startNode);

        // Main A* loop.
        while (openSet.Count > 0)
        {
            // Select node with lowest F, tie-break on H.
            PathNode current = openSet.OrderBy(n => n.F).ThenBy(n => n.H).First(); // Minor opt: tie-break on H

            // If goal reached, reconstruct path.
            if (current.Tile == goal)
                return ReconstructPath(current);

            // Move to closed set.
            openSet.Remove(current);
            closedSet.Add(current.Tile);

            // Evaluate neighbors.
            foreach (var neighborTile in getNeighbors(current.Tile))
            {
                // Skip if already closed.
                if (closedSet.Contains(neighborTile))
                    continue;

                // Base cost per move (could be varied per tile).
                int cost = 1; // Could be neighborTile.MovementCost if added to Tile
                int tentativeG = current.G + cost;

                // Find or create neighbor node.
                var neighborNode = openSet.FirstOrDefault(n => n.Tile == neighborTile);
                if (neighborNode == null)
                {
                    neighborNode = new PathNode(neighborTile) { H = Heuristic(neighborTile, goal) };
                    openSet.Add(neighborNode);
                }
                else if (tentativeG >= neighborNode.G)
                {
                    continue;
                }

                // Update node if better path found.
                neighborNode.Parent = current;
                neighborNode.G = tentativeG;
            }
        }

        return new List<Tile>(); // no path
    }

    /// <summary>
    /// Calculates Manhattan distance heuristic between two tiles.
    /// </summary>
    /// <param name="a">First tile.</param>
    /// <param name="b">Second tile.</param>
    /// <returns>The heuristic distance.</returns>
    public static int Heuristic(Tile a, Tile b)
    {
        // Manhattan distance (grid-based movement)
        return Math.Abs(a.LocalX - b.LocalX) + Math.Abs(a.LocalY - b.LocalY);
    }

    /// <summary>
    /// Reconstructs the path from goal node back to start using parents.
    /// </summary>
    /// <param name="node">The goal node.</param>
    /// <returns>List of tiles in path order.</returns>
    private static List<Tile> ReconstructPath(PathNode node)
    {
        // Build path by backtracking parents.
        var path = new List<Tile>();
        while (node != null)
        {
            path.Add(node.Tile);
            node = node.Parent;
        }
        // Reverse to get start-to-goal order.
        path.Reverse();
        return path;
    }
}
