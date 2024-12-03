namespace SokobanUltimate.GameLogic;

public interface ILevel
{
    public Cell[,] Cells { get; }
    public void Update();

    public bool IsWin();

    public bool IsLoss();

    public int LevelHeight { get; }

    public int LevelWidth { get; }
}