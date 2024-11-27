using Microsoft.Xna.Framework;

namespace SokobanUltimate.GameLogic;

public class Space : IEntity
{
    public Vector2 Coordinates { get; set; }
    public Action Act() => new Action(CommandType.IDLE);

    public Properties GetProperties() => new(this);
}