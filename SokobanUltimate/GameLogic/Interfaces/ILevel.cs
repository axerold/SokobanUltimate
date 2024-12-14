using System.Collections.Generic;
using SokobanUltimate.Control;
using SokobanUltimate.GameLogic.Levels;

namespace SokobanUltimate.GameLogic.Interfaces;

public interface ILevel
{
    public Cell[,] Cells { get; }

    public int StepCounter { get; }
    public void Update(List<Query> queries);

    public bool IsWin();

    public bool IsLoss();

    public int LevelHeight { get; }

    public int LevelWidth { get; }
}