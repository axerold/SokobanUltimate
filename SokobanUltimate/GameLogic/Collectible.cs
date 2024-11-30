using Microsoft.Xna.Framework;

namespace SokobanUltimate.GameLogic;

public class Collectible : IEntity
{
    public IntVector2 Coordinates { get; set; }

    public Action ActedBy(IEntity entity, Action action)
    {
        throw new System.NotImplementedException();
    }

    public Properties GetProperties()
    {
        throw new System.NotImplementedException();
    }

    public bool isDead()
    {
        throw new System.NotImplementedException();
    }
}