using SokobanUltimate.GameLogic.Actions;

namespace SokobanUltimate.GameLogic.Interfaces;

public interface IReactive
{
    public Action React(Action foreignAction, Action ownAction);
}