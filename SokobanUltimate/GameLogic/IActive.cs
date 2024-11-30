namespace SokobanUltimate.GameLogic;

public interface IActive
{
    public Action Act();
    
    public Action Act(IEntity entity, Action action);
}