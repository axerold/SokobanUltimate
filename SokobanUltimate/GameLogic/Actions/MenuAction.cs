namespace SokobanUltimate.GameLogic.Actions;

public class MenuAction
{
    public readonly MenuCommandType CommandType;
    public readonly string Target;
    public readonly object? Value;

    public MenuAction(MenuCommandType commandType, string target, object value = null)
    {
        CommandType = commandType;
        Target = target;
        Value = value;
    }
}