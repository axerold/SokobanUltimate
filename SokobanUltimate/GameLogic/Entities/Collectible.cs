using SokobanUltimate.GameLogic.Actions;
using SokobanUltimate.GameLogic.Interfaces;

namespace SokobanUltimate.GameLogic.Entities;

public class Collectible : IEntity
{
    public IntVector2 Location { get; set; }

    public Action OnAction(Action action)
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