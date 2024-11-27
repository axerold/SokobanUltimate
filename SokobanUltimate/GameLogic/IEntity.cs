using Microsoft.Xna.Framework;

namespace SokobanUltimate.GameLogic;

public interface IEntity
{
    public Vector2 Coordinates { get; set; }
    public Action Act();

    public Properties GetProperties();
}