using System.Collections.Generic;
using SokobanUltimate.Control;
using SokobanUltimate.GameLogic.Interfaces;

namespace SokobanUltimate.GameLogic.Levels;

public class Map : ILevel
{
    public string[] CharsInitialState { get; }
    public Cell[,] Cells { get; }
    public int StepCounter { get; }

    public void Update(List<Query> queries)
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