using System.Collections.Generic;

namespace SokobanUltimate.GameLogic;

public class Box : IEntity
{
    private IntVector2 _location;
    public IntVector2 Location 
    { 
        get => _location;
        set
        {
            _location = value;
            UpdateNeighbors();
        }
    }

    private Dictionary<IntVector2, Cell> _neighborCells = new();

    public Box(IntVector2 location)
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

    public Properties GetProperties() => new(this);

    public bool isDead()
    {
        //TODO
        return false;
    }

    private void UpdateNeighbors()
    {
        foreach (var direction in Level.Directions)
        {
            var newLocation = Location + direction;
            _neighborCells[direction] = GameState.GetCurrentLevel().Cells[newLocation.Y, newLocation.X];
        }
    }

    private bool IsBlocked(Cell cell)
    {
        if (cell.Landlord is Wall || cell.DeadZone) return true;
        if (cell.Tenants is null || cell.GetLastTenant() is not Box) return false;
        //TODO
        return false;


    }
}