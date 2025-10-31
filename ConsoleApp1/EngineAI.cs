//using System;
//using System.Collections.Generic;
//using System.Linq;




///// <summary>
///// The EngineAI class handles AI decision-making for entities in a turn-based combat system.
///// It generates actions for movement, attacks, and bonus actions, incorporating pathfinding,
///// target selection, and retreat logic for more intelligent behavior.
///// </summary>
//public class EngineAI
//{
//    /// <summary>
//    /// Optional property to store the next action the AI might take. Currently not used in the provided methods.
//    /// </summary>
//    public Action? NextAction { get; set; }

//    /// <summary>
//    /// Default constructor for EngineAI. Initializes an instance without any specific setup.
//    /// </summary>
//    public EngineAI()
//    {

//    }

//    /// <summary>
//    /// Retrieves the Tile object corresponding to an IActionable entity's current position from the game map.
//    /// </summary>
//    /// <param name="entity">The entity whose tile is being retrieved.</param>
//    /// <param name="saveGame">The current save game state containing the map.</param>
//    /// <returns>The Tile at the entity's grid and local coordinates.</returns>
//    private Tile GetTile(IActionable entity, SaveGame saveGame)
//    {
//        // Uses the Map utility to find the tile based on the entity's position coordinates.
//        return Map.FindTile(saveGame.GameMap, entity.Root.GridX, entity.Root.GridY, entity.Root.LocalX, entity.Root.LocalY);
//    }

//    /// <summary>
//    /// Selects the best enemy target for the decider based on a scoring system that prioritizes low-health and nearby enemies.
//    /// </summary>
//    /// <param name="decider">The entity making the decision (e.g., an NPC).</param>
//    /// <param name="enemies">List of potential enemy targets.</param>
//    /// <param name="saveGame">The current save game state for map and position data.</param>
//    /// <returns>The best target enemy, or null if none are available.</returns>
//    private IActionable FindBestTarget(ICharacter decider, List<ICharacter> enemies, SaveGame saveGame)
//    {
//        // Early exit if no enemies are provided or the list is empty.
//        if (enemies == null || enemies.Count == 0) return null;

//        // Get the tile of the deciding entity.
//        Tile deciderTile = GetTile(decider, saveGame);
//        IActionable best = null;
//        double maxScore = double.MinValue;

//        // Iterate through each enemy to calculate a threat score.
//        foreach (var enemy in enemies)
//        {
//            // Skip if the enemy is not alive.
//            if (!enemy.Health.IsAlive) continue;

//            // Get the enemy's tile and calculate Manhattan distance.
//            Tile enemyTile = GetTile(enemy, saveGame);
//            int dist = Pathfinding.Heuristic(deciderTile, enemyTile);
//            // Skip if distance is zero (should not happen, but prevents self-targeting).
//            if (dist == 0) continue; // Skip self

//            // Calculate health percentage remaining (lower health = higher vulnerability).
//            double healthPercent = (double)(enemy.Health.MaxHP - enemy.Health.CurrentHP) / enemy.Health.MaxHP;
//            // Score favors damaged (high healthPercent) and close (low dist) targets.
//            // Weights: 10 for health, 5 for proximity (inverse distance).
//            double score = healthPercent * 10 + (1.0 / (dist + 1)) * 5; // Favor low HP and close targets

//            // Update best target if this score is higher.
//            if (score > maxScore)
//            {
//                maxScore = score;
//                best = enemy;
//            }
//        }

//        return best;
//    }

//    /// <summary>
//    /// Generates a list of combat actions for the decider's turn, including movement, attacks, and bonus actions.
//    /// Handles retreat logic if health is low, and simulates movement for action planning.
//    /// </summary>
//    /// <param name="saveGame">The current save game state.</param>
//    /// <param name="Decider">The entity whose turn is being planned.</param>
//    /// <param name="Enemies">List of enemies.</param>
//    /// <param name="Allies">List of allies.</param>
//    /// <returns>A list of CombatAction objects representing the planned turn.</returns>
//    public List<CombatAction> GenerateTurnActions(SaveGame saveGame, ICharacter Decider, List<ICharacter> Enemies, List<ICharacter> Allies)
//    {
//        // Initialize empty list for actions.
//        var actions = new List<CombatAction>();

//        // If the decider is not alive, return empty actions (no turn for defeated entities).
//        if (!Decider.Health.IsAlive)
//        {
//            return actions; // Empty if not alive
//        }

//        // Get current tile of the decider.
//        Tile currentTile = GetTile(Decider, saveGame);
//        // Find the best target using the scoring system.
//        IActionable target = FindBestTarget(Decider, Enemies, saveGame);

//        // If no valid target, add a wait action and return.
//        if (target == null)
//        {
//            actions.Add(new CombatAction { Type = CombatActionType.Wait });
//            return actions;
//        }

//        // Get the target's tile.
//        Tile targetTile = GetTile(target, saveGame);

//        // Calculate health percentage to determine if retreat is needed (below 20%).
//        double hpPercent = (double)Decider.Health.CurrentHP / Decider.Health.MaxHP;
//        bool shouldRetreat = hpPercent < 0.2;

//        // Temporary list for movement actions.
//        List<CombatAction> movementActions = new List<CombatAction>();

//        if (shouldRetreat)
//        {
//            // Retreat logic: Move away from enemies, towards allies if possible
//            // Find a suitable retreat position.
//            var retreatTile = FindRetreatPosition(Decider, Enemies, Allies, saveGame);
//            if (retreatTile != null)
//            {
//                // Use pathfinding to get path to retreat tile.
//                var path = Pathfinding.FindPath(currentTile, retreatTile, t => GetNeighbors(t, saveGame, Decider));
//                if (path.Count > 1)
//                {
//                    // Limit moves to the decider's speed.
//                    int speed = Decider.TileSpeed;
//                    int movesToMake = Math.Min(path.Count - 1, speed);
//                    // Add move actions for each step in the path.
//                    for (int i = 1; i <= movesToMake; i++)
//                    {
//                        movementActions.Add(new CombatAction { Type = CombatActionType.Move, TargetTile = path[i] });
//                    }
//                    // Simulate updating current position for further planning.
//                    currentTile = path[movesToMake]; // Simulate position
//                }
//            }
//        }
//        else
//        {
//            // Normal movement towards target if not in range
//            // Check if already in attack range (assumes melee range of 1 tile).
//            bool inRange = Decider.CanAttack(target) && Pathfinding.Heuristic(currentTile, targetTile) <= 1; // Assume range 1 for melee

//            if (!inRange)
//            {
//                // Find path to target.
//                var path = Pathfinding.FindPath(currentTile, targetTile, t => GetNeighbors(t, saveGame, Decider));
//                if (path.Count > 1)
//                {
//                    // Limit to speed and add move actions.
//                    int speed = Decider.TileSpeed;
//                    int movesToMake = Math.Min(path.Count - 1, speed);
//                    for (int i = 1; i <= movesToMake; i++)
//                    {
//                        movementActions.Add(new CombatAction { Type = CombatActionType.Move, TargetTile = path[i] });
//                    }
//                    // Simulate new position and re-check range.
//                    currentTile = path[movesToMake]; // Simulate
//                    inRange = Pathfinding.Heuristic(currentTile, targetTile) <= 1 && Decider.CanAttack(target);
//                }
//            }
//        }

//        // Add planned movements to the main action list.
//        actions.AddRange(movementActions);

//        // Plan main actions (e.g., attacks).
//        int actionCount = Decider.ActionCount;
//        for (int a = 0; a < actionCount; a++)
//        {
//            if (target is ICharacter realTarget)
//            {
//                // Attack if target is alive, not retreating, and in range.
//                if (realTarget.Health.IsAlive && !shouldRetreat && Decider.CanAttack(target))
//                {
//                    actions.Add(new CombatAction { Type = CombatActionType.Attack, TargetEntity = realTarget });
//                }
//                else
//                {
//                    actions.Add(new CombatAction { Type = CombatActionType.Wait });
//                }
//            }
//        }

//        // Plan bonus actions.
//        int bonusCount = Decider.BonusActionCount;
//        for (int b = 0; b < bonusCount; b++)
//        {
//            // Add bonus if available.
//            if (Decider.CanUseBonus())
//            {
//                actions.Add(new CombatAction { Type = CombatActionType.BonusAction });
//            }
//        }

//        return actions;
//    }

//    /// <summary>
//    /// Finds a suitable retreat position by evaluating tiles within movement range,
//    /// maximizing distance from enemies and minimizing distance to allies.
//    /// </summary>
//    /// <param name="decider">The retreating entity.</param>
//    /// <param name="enemies">List of enemies to avoid.</param>
//    /// <param name="allies">List of allies to approach.</param>
//    /// <param name="saveGame">The current save game state.</param>
//    /// <returns>The best retreat tile, or null if none found.</returns>
//    private Tile FindRetreatPosition(IActionable decider, List<ICharacter> enemies, List<ICharacter> allies, SaveGame saveGame)
//    {
//        // Simple: Find a tile far from enemies, close to allies
//        // Get current tile.
//        Tile current = GetTile(decider, saveGame);
//        var possibleTiles = new List<Tile>();

//        // Use BFS to explore all reachable tiles within speed limit.
//        var queue = new Queue<(Tile tile, int dist)>();
//        var visited = new HashSet<Tile>();
//        queue.Enqueue((current, 0));
//        visited.Add(current);

//        int maxDist = decider.TileSpeed;

//        while (queue.Count > 0)
//        {
//            var (tile, d) = queue.Dequeue();
//            // Skip if beyond movement range.
//            if (d > maxDist) continue;

//            // Add to possible tiles.
//            possibleTiles.Add(tile);

//            // Enqueue walkable neighbors.
//            foreach (var neighbor in GetNeighbors(tile, saveGame, decider))
//            {
//                if (!visited.Contains(neighbor))
//                {
//                    visited.Add(neighbor);
//                    queue.Enqueue((neighbor, d + 1));
//                }
//            }
//        }

//        Tile best = null;
//        double bestScore = double.MinValue;

//        // Evaluate each possible tile.
//        foreach (var tile in possibleTiles)
//        {
//            // Sum distances to all enemies (higher is better).
//            double enemyDistSum = enemies.Sum(e => Pathfinding.Heuristic(tile, GetTile(e, saveGame)));
//            // Sum distances to allies (lower is better; subtract to maximize score).
//            double allyDistSum = allies.Any() ? allies.Sum(a => Pathfinding.Heuristic(tile, GetTile(a, saveGame))) : 0;
//            double score = enemyDistSum - allyDistSum; // Maximize enemy dist, minimize ally dist

//            // Update best if score is higher.
//            if (score > bestScore)
//            {
//                bestScore = score;
//                best = tile;
//            }
//        }

//        return best;
//    }

//    /// <summary>
//    /// Determines the cardinal direction of movement from the starting tile to the next tile.
//    /// </summary>
//    /// <remarks>Returns 0 if the movement is diagonal, the tiles are the same, or they are not directly
//    /// adjacent. Only cardinal directions (north, south, east, west) are supported.</remarks>
//    /// <param name="start">The tile representing the starting position.</param>
//    /// <param name="nextStep">The tile representing the adjacent position to move to.</param>
//    /// <returns>An integer representing the direction of movement: 1 for north, 2 for south, 3 for east, 4 for west, or 0 if the
//    /// tiles are not adjacent in a cardinal direction.</returns>
//    public int DirectionFromTiles(Tile start, Tile nextStep)
//    {
//        // Calculate differences in local coordinates.
//        int dx = nextStep.LocalX - start.LocalX;
//        int dy = nextStep.LocalY - start.LocalY;

//        // Map differences to direction codes.
//        if (dx == -1 && dy == 0) return 1; // North
//        if (dx == 1 && dy == 0) return 2;  // South
//        if (dx == 0 && dy == 1) return 3;  // East
//        if (dx == 0 && dy == -1) return 4; // West

//        return 0; // no movement or diagonal (invalid)
//    }

//    /// <summary>
//    /// Finds directions to move towards better cover, defined as maximizing distance from the primary threat (player) and optionally other enemies.
//    /// </summary>
//    /// <param name="npc">The entity seeking cover.</param>
//    /// <param name="player">The primary threat (player).</param>
//    /// <param name="enemies">Optional list of additional enemies.</param>
//    /// <param name="allies">Optional list of allies (not currently used in logic).</param>
//    /// <param name="saveGame">Optional save game state; returns empty if null.</param>
//    /// <returns>List of directions (1-4) to potential cover positions.</returns>
//    public List<int> FindBestCover(IActionable npc, Player player, List<ICharacter>? enemies = null, List<ICharacter>? allies = null, SaveGame saveGame = null)
//    {
//        // Initialize empty list for directions.
//        List<int> moveList = new List<int>();
//        // Stub: Return empty if no saveGame provided.
//        if (saveGame == null) return moveList; // Stub if no saveGame

//        // Get current and player tiles.
//        Tile current = GetTile(npc, saveGame);
//        Tile playerTile = GetTile(player, saveGame);
//        // Get list of neighboring tiles.
//        var neighbors = GetNeighbors(current, saveGame, npc).ToList();

//        // Calculate current distance to player.
//        int currentDist = Pathfinding.Heuristic(current, playerTile);

//        // Check each neighbor.
//        foreach (var neigh in neighbors)
//        {
//            // Calculate new distance.
//            int dist = Pathfinding.Heuristic(neigh, playerTile);
//            // If farther from player, add the direction.
//            if (dist > currentDist)
//            {
//                int dir = DirectionFromTiles(current, neigh);
//                if (dir != 0) moveList.Add(dir);
//            }
//        }

//        // Optionally factor in other enemies/allies similarly
//        // Note: Current implementation only considers the player; could be expanded.
//        return moveList;
//    }

//    /// <summary>
//    /// Retrieves neighboring tiles for a given tile, considering walkability.
//    /// </summary>
//    /// <param name="tile">The central tile.</param>
//    /// <param name="saveGame">The save game state for map access.</param>
//    /// <param name="mover">Optional mover entity (not currently used).</param>
//    /// <returns>An enumerable of valid neighboring tiles.</returns>
//    public static IEnumerable<Tile> GetNeighbors(Tile tile, SaveGame saveGame, IActionable mover = null)
//    {
//        // Define cardinal directions (up, down, right, left).
//        var directions = new (int dx, int dy)[] { (1, 0), (-1, 0), (0, 1), (0, -1) };
//        foreach (var (dx, dy) in directions)
//        {
//            // Find the neighbor tile using map utility.
//            Tile neighbor = Map.FindTile(
//                saveGame.GameMap,
//                tile.GridX,
//                tile.GridY,
//                tile.LocalX + dx,
//                tile.LocalY + dy
//            );

//            // Yield if exists and is walkable.
//            if (neighbor != null && neighbor.IsWalkable)
//            {
//                // Check if occupied; assume a method IsOccupied(Tile, SaveGame)
//                // For now, yield if not null and walkable
//                // TODO: Implement occupancy check to avoid blocked tiles.
//                yield return neighbor;
//            }
//        }
//    }

//    /// <summary>
//    /// Plans main actions and bonus actions for an NPC, targeting the player.
//    /// </summary>
//    /// <param name="npc">The NPC planning actions.</param>
//    /// <param name="player">The target player.</param>
//    /// <returns>List of planned CombatActions.</returns>
//    public List<CombatAction> PlanActions(ICharacter npc, SaveGame saveGame)
//    {
//        // Initialize action list.
//        var actions = new List<CombatAction>();

//        // Add attack if possible and player is alive.
//        if (npc.CanAttack(saveGame.PlayerCharacter) && saveGame.PlayerCharacter.Health.IsAlive)
//        {
//            actions.Add(new CombatAction { Type = CombatActionType.Attack, TargetEntity = saveGame.PlayerCharacter });
//        }
//        else
//        {
//            actions.Add(new CombatAction { Type = CombatActionType.Wait });
//        }

//        // Add bonus action if available.
//        if (npc.CanUseBonus())
//        {
//            actions.Add(new CombatAction { Type = CombatActionType.BonusAction });
//        }

//        return actions;
//    }

//    /// <summary>
//    /// Plans movement actions for an NPC to approach the player if not in attack range.
//    /// </summary>
//    /// <param name="npc">The NPC planning movement.</param>
//    /// <param name="player">The target player.</param>
//    /// <param name="saveGame">The save game state.</param>
//    /// <returns>List of movement CombatActions.</returns>
//    private List<CombatAction> PlanMovement(ICharacter npc, SaveGame saveGame)
//    {
//        // Initialize movement list.
//        var movementActions = new List<CombatAction>();

//        // Get current and target tiles.
//        Tile currentTile = GetTile(npc, saveGame);
//        Tile targetTile = GetTile(saveGame.PlayerCharacter, saveGame);

//        // If not in attack range, plan path.
//        if (!npc.CanAttack(saveGame.PlayerCharacter))
//        {
//            var path = Pathfinding.FindPath(currentTile, targetTile, t => GetNeighbors(t, saveGame, npc));
//            if (path.Count > 1)
//            {
//                // Limit to speed and add moves.
//                int speed = npc.TileSpeed;
//                int moves = Math.Min(path.Count - 1, speed);
//                for (int i = 1; i <= moves; i++)
//                {
//                    movementActions.Add(new CombatAction { Type = CombatActionType.Move, TargetTile = path[i] });
//                }
//            }
//        }

//        return movementActions;
//    }

//    /// <summary>
//    /// Plans the full turn for an NPC, combining movement and actions.
//    /// </summary>
//    /// <param name="npc">The NPC whose turn is planned.</param>
//    /// <param name="player">The target player.</param>
//    /// <param name="saveGame">The save game state.</param>
//    /// <returns>List of all planned CombatActions.</returns>
//    public List<CombatAction> PlanTurn(ICharacter npc, SaveGame saveGame)
//    {
//        // Initialize plan list.
//        var plan = new List<CombatAction>();

//        // Movement first
//        plan.AddRange(PlanMovement(npc,saveGame));

//        // Then main action
//        plan.AddRange(PlanActions(npc, saveGame));

//        return plan;
//    }

//    #region === Wrapper for trun ===
//    /// <summary>
//    /// Executes the planned turn for an NPC, handling each action type.
//    /// Wraps in try-catch for error handling.
//    /// </summary>
//    /// <param name="npc">The NPC executing the turn.</param>
//    /// <param name="player">The player (used in planning).</param>
//    /// <param name="saveGame">The save game state.</param>
//    public void ExecuteTurn(ICharacter npc, ICharacter player, SaveGame saveGame)
//    {
//        try
//        {
//            // Generate the plan.
//            var plan = PlanTurn(npc,saveGame);

//            // Iterate through each action.
//            foreach (var action in plan)
//            {
//                switch (action.Type)
//                {
//                    case CombatActionType.Move:
//                        // Calculate direction and move if valid.
//                        int dir = DirectionFromTiles(
//                            Map.FindTile(saveGame.GameMap, npc.Root.GridX, npc.Root.GridY, npc.Root.LocalX, npc.Root.LocalY),
//                            action.TargetTile
//                        );
//                        if (dir != 0)
//                        {
//                            if (npc is NPC realNPC)
//                            {
//                                // Checks if the player moved. 
//                                bool didMove =  EngineMovement.Move(saveGame, realNPC, dir).Result;
//                                if (didMove)
//                                {// Successful check, nothing more to do
//                                    EngineGUI.WriteMessageToPlayer("npc moved!");
//                                }
//                                else // Failed check, need to replan movement and move again.
//                                {
//                                    EngineGUI.WriteMessageToPlayer("npc did not move... fucking fuck");
//                                    PlanTurn(npc, saveGame);
//                                }
//                            }
//                        }
//                        break;

//                    case CombatActionType.Attack:
//                        // Attack if target exists and is alive.
//                        if (action.TargetEntity != null && action.TargetEntity.Health.IsAlive)
//                        {
//                            npc.PerformAttack(action.TargetEntity);
//                        }
//                        break;

//                    case CombatActionType.BonusAction:
//                        // Perform bonus action.
//                        npc.PerformBonus(player);
//                        break;

//                    case CombatActionType.Wait:
//                        // Do nothing.
//                        break;
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            // Log or handle error, e.g., Console.WriteLine($"Error in ExecuteTurn: {ex.Message}");
//            // Note: In production, replace with proper logging.
//        }
//    }
//    public void ExecuteNonCombatTurn(ICharacter npc, Player player, SaveGame saveGame)
//    {

//    }
//    #endregion 

//}

//#region === Helper Classes ===

///// <summary>
///// Represents a single action in combat, such as move, attack, or bonus.
///// </summary>
//public class CombatAction
//{
//    /// <summary>
//    /// The type of action (Move, Attack, etc.).
//    /// </summary>
//    public CombatActionType Type { get; set; }
//    /// <summary>
//    /// Target tile for movement actions.
//    /// </summary>
//    public Tile TargetTile { get; set; } // for movement
//    /// <summary>
//    /// Target entity for attack actions.
//    /// </summary>
//    public ICharacter TargetEntity { get; set; } // for attack
//}

///// <summary>
///// Node used in pathfinding algorithm to track path costs and parents.
///// </summary>
//public class PathNode
//{
//    /// <summary>
//    /// The tile this node represents.
//    /// </summary>
//    public Tile Tile { get; }
//    /// <summary>
//    /// Parent node in the path.
//    /// </summary>
//    public PathNode Parent { get; set; }
//    /// <summary>
//    /// Cost from start to this node (G score).
//    /// </summary>
//    public int G { get; set; } // cost so far
//    /// <summary>
//    /// Heuristic estimate to goal (H score).
//    /// </summary>
//    public int H { get; set; } // heuristic
//    /// <summary>
//    /// Total estimated cost (F = G + H).
//    /// </summary>
//    public int F => G + H;     // total cost

//    /// <summary>
//    /// Constructor for PathNode.
//    /// </summary>
//    /// <param name="tile">The tile for this node.</param>
//    public PathNode(Tile tile)
//    {
//        Tile = tile;
//    }
//}

///// <summary>
///// Static class providing pathfinding utilities using A* algorithm.
///// </summary>
//public static class Pathfinding
//{
//    /// <summary>
//    /// Finds the shortest path from start to goal using A* with provided neighbor function.
//    /// </summary>
//    /// <param name="start">Starting tile.</param>
//    /// <param name="goal">Goal tile.</param>
//    /// <param name="getNeighbors">Function to get neighbors for a tile.</param>
//    /// <returns>List of tiles forming the path, or empty if no path.</returns>
//    public static List<Tile> FindPath(Tile start, Tile goal, Func<Tile, IEnumerable<Tile>> getNeighbors)
//    {
//        // Open set for nodes to evaluate.
//        var openSet = new List<PathNode>();
//        // Closed set for visited tiles.
//        var closedSet = new HashSet<Tile>();

//        // Initialize start node.
//        PathNode startNode = new PathNode(start) { G = 0, H = Heuristic(start, goal) };
//        openSet.Add(startNode);

//        // Main A* loop.
//        while (openSet.Count > 0)
//        {
//            // Select node with lowest F, tie-break on H.
//            PathNode current = openSet.OrderBy(n => n.F).ThenBy(n => n.H).First(); // Minor opt: tie-break on H

//            // If goal reached, reconstruct path.
//            if (current.Tile == goal)
//                return ReconstructPath(current);

//            // Move to closed set.
//            openSet.Remove(current);
//            closedSet.Add(current.Tile);

//            // Evaluate neighbors.
//            foreach (var neighborTile in getNeighbors(current.Tile))
//            {
//                // Skip if already closed.
//                if (closedSet.Contains(neighborTile))
//                    continue;

//                // Base cost per move (could be varied per tile).
//                int cost = 1; // Could be neighborTile.MovementCost if added to Tile
//                int tentativeG = current.G + cost;

//                // Find or create neighbor node.
//                var neighborNode = openSet.FirstOrDefault(n => n.Tile == neighborTile);
//                if (neighborNode == null)
//                {
//                    neighborNode = new PathNode(neighborTile) { H = Heuristic(neighborTile, goal) };
//                    openSet.Add(neighborNode);
//                }
//                else if (tentativeG >= neighborNode.G)
//                {
//                    continue;
//                }

//                // Update node if better path found.
//                neighborNode.Parent = current;
//                neighborNode.G = tentativeG;
//            }
//        }

//        return new List<Tile>(); // no path
//    }

//    /// <summary>
//    /// Calculates Manhattan distance heuristic between two tiles.
//    /// </summary>
//    /// <param name="a">First tile.</param>
//    /// <param name="b">Second tile.</param>
//    /// <returns>The heuristic distance.</returns>
//    public static int Heuristic(Tile a, Tile b)
//    {
//        // Manhattan distance (grid-based movement)
//        return Math.Abs(a.LocalX - b.LocalX) + Math.Abs(a.LocalY - b.LocalY);
//    }

//    /// <summary>
//    /// Reconstructs the path from goal node back to start using parents.
//    /// </summary>
//    /// <param name="node">The goal node.</param>
//    /// <returns>List of tiles in path order.</returns>
//    private static List<Tile> ReconstructPath(PathNode node)
//    {
//        // Build path by backtracking parents.
//        var path = new List<Tile>();
//        while (node != null)
//        {
//            path.Add(node.Tile);
//            node = node.Parent;
//        }
//        // Reverse to get start-to-goal order.
//        path.Reverse();
//        return path;
//    }
//}

//#endregion