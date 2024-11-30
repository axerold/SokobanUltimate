using Microsoft.Xna.Framework;

namespace SokobanUltimate.GameLogic;

public interface IEntity
{
    public IntVector2 Coordinates { get; set; }
    
    public Action ActedBy(IEntity entity, Action action);

    public Properties GetProperties();

    public bool isDead();
}