using Microsoft.Xna.Framework;

namespace SokobanUltimate.GameLogic;

public class Space(IntVector2 coordinates) : IEntity
{
    public IntVector2 Coordinates
    {
        get => coordinates;
        set => coordinates = value;
    }
    public Action ActedBy(IEntity entity, Action action) => action;

    public Properties GetProperties() => new(this);
    public bool isDead() => false;
}