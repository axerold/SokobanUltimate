namespace SokobanUltimate.GameLogic;

public interface IInteractive
{
    public void InteractWith(IEntity entity);

    public void GetInteractedBy(IEntity entity);
}