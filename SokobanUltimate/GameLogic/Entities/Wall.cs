using Microsoft.Xna.Framework;
using SokobanUltimate.GameLogic.Actions;
using SokobanUltimate.GameLogic.Interfaces;

namespace SokobanUltimate.GameLogic.Entities;

public class Wall : IEntity
{
    public Wall(Point location)
    {
        Location = location;
    }
    public Point Location { get; set; }

    public Action OnAction(Action action)
    {
        throw new System.NotImplementedException();
    }

    public Properties GetProperties() => new(this);

    public bool isDead() => false;
}