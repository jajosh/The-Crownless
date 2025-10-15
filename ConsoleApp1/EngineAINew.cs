//using System;
//using Windows.Media.Streaming.Adaptive;
//public enum ActionType { Action, BonusAction, Move, Wait, FreeAction }
//public enum Objective { Heal, Kill, Move, GetCover, Flank }
//public enum CoverGrade { Full, ThreeQuarters, Half, Quarter, None }
//public class EngineAINew
//{
//    public List<ICharacter> ActiveNPC { get; set; }
//    public List<ICharacter> LocationNPC { get; set; }
//    private static readonly (int x, int y)[] Directions = new[] {
//    (1, 0),   // right
//    (-1, 0),  // left
//    (0, 1),   // up
//    (0, -1)   // down
//};
//    public EngineAINew()
//    {

//    }
//    #region === Execute Turn Methods ===
//    // --- Executes the combat turn
//    public void ExecuteCombatTurn()
//    {

//    }
//    // --- Moved the NPC from one waypoint to the next
//    public void ExecuteRandomMove()
//    {

//    }
//    #endregion
//    #region === Brain Methods ===
//    public CombatPlan PlanTurns()
//    {

//    }
//    public List<RoundAction> PlanTrun()
//    {
//        var plan = new List<GameAction>();

//        // logic move action, bonus action?
//    }
//    #region === Movement lob ===
//    public RoundAction PlanMovement(ICharacter caster)
//    {
//        Grid currentGrid = Map.GridKey[(caster.Root.GridX, caster.Root.GridY)];
//        Dictionary<(int, int), Tile> localTiles = Map.LocalTiles();

//        (int, int)[] directions =
//        {
//            (1,0), (-1,0), (0,1), (0,-1)
//        };
//        foreach (var (dx, dy) in directions)
//        {
//            var neighborKey -(x + dx, y + dy);
//            if (localTiles.TryGetValue(nightborKey, out var neighor) && neighor.IsWalkable)
//            {

//            }
//        }
        
//    }
//    // Plan movement 
//    IEnumerable<Tile> GetNeighbors((int x, int y) current, Dictionary<(int, int), Tile> localTiles)
//    {
//        foreach (var (dx, dy) in Directions)
//        {
//            var neighborKey = (current.x + dx, current.y + dy);
//            if (localTiles.TryGetValue(neighborKey, out var neighbor) && neighbor.IsWalkable)
//                yield return neighbor;
//        }
//    }
//    public Tile PickRandomDestination(Dictionary<(int, int), Tile> localTiles)
//    {
//        var walkables = localTiles.Values.Where(t => t.IsWalkable).ToList();
//        return walkables[Random.Shared.Next(walkables.Count)];
//    }
//    #endregion
//    public RoundAction PlanAction()
//    {
//        // Plans the action to use
//    }
//    public RoundAction PlanBonusAction()
//    {
//        // Plans the bonus action to use
//    }
//    public List<Tile> FindPath(Grid grid, Tile start, Tile goal)
//    {

//    }

//    #endregion

//}
//#region === Helper Classes ===
//public class RoundAction
//{
//    public ActionType Type { get; set; }
//    public Tile? TargetTile { get; set; }
//    public IActionable? Target { get; set; }
//}
//public class CombatPlan
//{
//    public RoundAction Actions { get; set; }
//    public Objective Objective { get; set; }
//    public Tile? TargetTile { get; set; }
//    public IActionable target { get; set; }
//}
//class Node
//{
//    public int X, Y;
//    public bool IsWalkable;
//    public CoverGrade Cover;
//    public int MovementCost;
//    public float G; // cost from start
//    public float H; // heuristic to goal
//    public float F => G + H;
//    public Node Parent;
//}
//float Heuristic(int x1, int y1, int x2, int y2) => Math.Abs(x1 - x2 + Math.Abs(y1 - y2))
//#endregion
