﻿using Microsoft.Xna.Framework;

namespace SokobanUltimate.GameLogic;

public class Wall : IEntity
{
    public Wall(IntVector2 location)
    {
        Location = location;
    }
    public IntVector2 Location { get; set; }

    public Action OnAction(Action action)
    {
        throw new System.NotImplementedException();
    }

    public Properties GetProperties() => new(this);

    public bool isDead() => false;
}