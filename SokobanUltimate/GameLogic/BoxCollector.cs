using Microsoft.Xna.Framework;

namespace SokobanUltimate.GameLogic;

public class BoxCollector(IntVector2 coordinates) : IEntity
{
    public IntVector2 Coordinates
    {
        get => coordinates;
        set => coordinates = value;
    }
    public bool BoxReceived { get; set; }


    public Action ActedBy(IEntity entity, Action action)
    {
        return Level.IdleAction;
    }

    public Properties GetProperties() => new(this);

    public bool isDead() => false;
}