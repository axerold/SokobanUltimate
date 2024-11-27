using Microsoft.Xna.Framework;

namespace SokobanUltimate.GameLogic;

public class Box : IEntity
{
    public Vector2 Coordinates { get; set; }

    public Action Act()
    {
        throw new System.NotImplementedException();
    }

    public Properties GetProperties()
    {
        throw new System.NotImplementedException();
    }
}