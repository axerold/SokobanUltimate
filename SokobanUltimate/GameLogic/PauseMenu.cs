﻿using System.Collections.Generic;

namespace SokobanUltimate.GameLogic;

public class PauseMenu : IMenu
{
    public List<IMenu> ChildMenus { get; set; }
    public void ProcessCommands()
    {
        throw new System.NotImplementedException();
    }
}