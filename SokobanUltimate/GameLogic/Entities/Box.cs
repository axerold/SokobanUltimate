using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Serilog;
using SokobanUltimate.GameLogic.Actions;
using SokobanUltimate.GameLogic.Interfaces;
using SokobanUltimate.GameLogic.Levels;

namespace SokobanUltimate.GameLogic.Entities;

public class Box : IEntity, IReactive
{
    private Point _location;
    public Point Location 
    { 
        get => _location;
        set
        {
            _location = value;
            UpdateNeighbors();
        }
    }

    private Dictionary<Point, Cell> _neighborCells = new();

    public Box(Point location)
    {
        _location = location;
    }
    
    
    public Action OnAction(Action action)
    {
        UpdateNeighbors();
        var idleAction = new Action(CommandType.IDLE, this, Location);
        if (action.CommandType is not CommandType.MOVE || action.Initiator is not Player) 
            return idleAction;
        
        var direction = Location - action.Initiator.Location;
        var targetCell = _neighborCells![direction];
        if (targetCell.GetLastTenant() is Box || targetCell.Landlord is Wall) 
            return idleAction;
        
        return new Action(CommandType.MOVE, this, Location + direction);
    }
    
    public Action React(Action foreignAction, Action ownAction)
    {
        if (ownAction.CommandType is CommandType.IDLE && foreignAction.CommandType is CommandType.MOVE
                                                      && foreignAction.Initiator is Player)
            return new Action(initiator: foreignAction.Initiator, location: foreignAction.StartLocation);
        return foreignAction;
    }

    public Properties GetProperties() => new(this);

    public bool isDead()
    {
        var currentCell = GameState.GetCurrentLevel().Cells[Location.Y, Location.X];
        return currentCell.Landlord is not BoxCollector && IsGroupBoxBlocked(currentCell);
    }

    private void UpdateNeighbors()
    {
        foreach (var (_,direction) in Level.Directions)
        {
            var newLocation = Location + direction;
            _neighborCells[direction] = GameState.GetCurrentLevel().Cells[newLocation.Y, newLocation.X];
        }
    }

    private List<Cell> GetGroupOfConnectedBoxes(Cell current, HashSet<Cell> visited)
    {
        var group = new List<Cell>();
        var queue = new Queue<Cell>();
        queue.Enqueue(current);

        while (queue.Count > 0)
        {
            var cell = queue.Dequeue();
            if (visited.Contains(cell)) continue;

            visited.Add(cell);
            group.Add(cell);

            foreach (var (_, neighborCell) in ((Box)cell.GetLastTenant())._neighborCells)
            {
                if (neighborCell.GetLastTenant() is Box && !visited.Contains(neighborCell))
                {
                    queue.Enqueue(neighborCell);
                }
            }
        }

        return group;
    }

    private bool IsGroupBoxBlocked(Cell current)
    {
        var visited = new HashSet<Cell>();
        var group = GetGroupOfConnectedBoxes(current, visited);

        return group.All(cell => !((Box)cell.GetLastTenant()).AnyEmptySpaces());
    }
    
    private bool AnyEmptySpaces()
    {
        UpdateNeighbors();
        return (_neighborCells[Level.Directions["up"]].FreeToMove()
               && _neighborCells[Level.Directions["down"]].FreeToMove())
               || (_neighborCells[Level.Directions["left"]].FreeToMove()
               && _neighborCells[Level.Directions["right"]].FreeToMove());
    }
}