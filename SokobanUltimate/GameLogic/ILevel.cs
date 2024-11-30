namespace SokobanUltimate.GameLogic;

public interface ILevel
{
    public void Update();

    public bool IsWin();

    public bool IsLoss();

    public IEntity[,] GetCurrentState();

    public int GetLevelHeight();

    public int GetLevelWidth();
}