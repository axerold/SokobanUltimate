using SokobanUltimate.GameLogic.Levels;

namespace SokobanUltimate.GameLogic.Interfaces;

public interface ILevel
{
    public Cell[,] Cells { get; }

    public int StepCounter { get; }
    public void Update();

    public bool IsWin();

    public bool IsLoss();

    public int LevelHeight { get; }

    public int LevelWidth { get; }
}