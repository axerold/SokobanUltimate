using Microsoft.Xna.Framework;

namespace SokobanUltimate.GameLogic;

public interface IEntity
{
    public IntVector2 Location { get; set; }

    public Action OnAction(Action action);

    public Properties GetProperties();

    public bool isDead();
}