using Microsoft.Xna.Framework;

namespace SokobanUltimate.GameLogic;

public class Space : IEntity
{
    public IntVector2 Location { get; set; }

    public Space(IntVector2 location)
    {
        Location = location;
    }

    public Action OnAction(Action action)
    {
        throw new System.NotImplementedException();
    }

    public Properties GetProperties() => new(this);
    public bool isDead() => false;
}