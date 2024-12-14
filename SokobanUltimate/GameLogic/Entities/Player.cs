using Microsoft.Xna.Framework;
using Serilog;
using SokobanUltimate.Drawing;
using SokobanUltimate.GameLogic.Actions;
using SokobanUltimate.GameLogic.Interfaces;

namespace SokobanUltimate.GameLogic.Entities;

public class Player : IEntity
{
    private Point _coordinates;
    private Action _lastAction;

    public Player(Point coordinates)
    {
        _coordinates = coordinates;
        LastDirection = AnimationManager.Directions[0];
    }

    public Point Location
    {
        get => _coordinates;
        set => _coordinates = value;
    }

    public Point LastDirection
    {
        get;
        set;
    }

    public Action LastAction
    {
        get => _lastAction ?? new Action();
        set => _lastAction = value;
    }

    public Action OnAction(Action action)
    {
        return new Action();
    }

    public Properties GetProperties() => new(this);

    public bool isDead() => false;
}