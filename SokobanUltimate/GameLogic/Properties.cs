﻿namespace SokobanUltimate.GameLogic;

public class Properties
{
    public bool IsMoveable { get; }
    public bool IsRunByAi { get; }
    public bool IsInteractive { get; }

    public Properties(IEntity entity)
    {
        var defaultValues = entity switch
        {
            Player or Box => DefaultValuesForPlayerAndBox(),
            BoxCollector => DefaultValuesForCollector(),
            _ => DefaultValuesForWallOrSpace()
        };

        IsMoveable = defaultValues[0];
        IsRunByAi = defaultValues[1];
        IsInteractive = defaultValues[2];
    }

    private bool[] DefaultValuesForPlayerAndBox() => [true, false, true];
    private bool[] DefaultValuesForWallOrSpace() => [false, false, false];
    private bool[] DefaultValuesForCollector() => [false, false, true];


}