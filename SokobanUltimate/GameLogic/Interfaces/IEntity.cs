using Microsoft.Xna.Framework;
using SokobanUltimate.GameLogic.Actions;

namespace SokobanUltimate.GameLogic.Interfaces;

public interface IEntity
{
    public Point Location { get; set; }

    public Action OnAction(Action action);

    public Properties GetProperties();

    public bool isDead();
}