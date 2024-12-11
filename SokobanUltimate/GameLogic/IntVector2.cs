using System;
using System.Numerics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace SokobanUltimate.GameLogic;

public record struct IntVector2
{
    public readonly int X;
    public readonly int Y;
    
    public IntVector2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static IntVector2 operator +(IntVector2 a, IntVector2 b)
    {
        return new IntVector2(a.X + b.X, a.Y + b.Y);
    }

    public static IntVector2 operator -(IntVector2 a, IntVector2 b)
    {
        return new IntVector2(a.X - b.X, a.Y - b.Y);
    }
}