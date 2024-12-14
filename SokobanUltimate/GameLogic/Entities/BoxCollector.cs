using Microsoft.Xna.Framework;
using SokobanUltimate.GameLogic.Actions;
using SokobanUltimate.GameLogic.Interfaces;

namespace SokobanUltimate.GameLogic.Entities;

public class BoxCollector(Point coordinates) : IEntity
{
    public Point Location
    {
        get => coordinates;
        set => coordinates = value;
    }

    private bool _boxReceived;
    public bool BoxReceived
    {
        set
        {
            var cell = GameState.GetCurrentLevel().Cells[Location.Y, Location.X];
            if (value && cell.Tenants is not null && cell.GetLastTenant() is Box)
                _boxReceived = true;
            if (!value && cell.Tenants is null || cell.GetLastTenant() is not Box)
                _boxReceived = false;
        }

        get => _boxReceived;
    }

    public Action OnAction(Action action) => new();

    public Properties GetProperties() => new(this);

    public bool isDead() => false;
}