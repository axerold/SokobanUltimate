using Microsoft.Xna.Framework;

namespace SokobanUltimate.GameLogic;

public class Map : ILevel
{
    public Cell[,] Cells { get; }
    public int StepCounter { get; }

    public void Update()
    {
        throw new System.NotImplementedException();
    }

    public bool IsWin()
    {
        throw new System.NotImplementedException();
    }

    public bool IsLoss()
    {
        throw new System.NotImplementedException();
    }

    public int LevelHeight { get; }
    public int LevelWidth { get; }
}