namespace SokobanUltimate.Control;

public class Query
{
    public string Command { get; }
    public string Info { get; }
    public int Priority { get; }

    public Query(string command, string info, int priority)
    {
        Command = command;
        Info = info;
        Priority = priority;
    }
}