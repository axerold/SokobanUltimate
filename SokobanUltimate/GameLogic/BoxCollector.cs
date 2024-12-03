using System.Linq;
using Microsoft.Xna.Framework;

namespace SokobanUltimate.GameLogic;

public class BoxCollector(IntVector2 coordinates) : IEntity
{
    public IntVector2 Location
    {
        get => coordinates;
        set => coordinates = value;
    }

    private bool _boxReceived;
    public bool BoxReceived
    {
        set
        {
            if (value && GameState.GetCurrentLevel().Cells[Location.Y, Location.X].Tenants.Last() is Box)
                _boxReceived = true;
        }

        get => _boxReceived;
    }

    public Action OnAction(Action action) => new();

    public Properties GetProperties() => new(this);

    public bool isDead() => false;
}