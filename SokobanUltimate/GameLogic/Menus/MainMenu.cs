﻿using System.Collections.Generic;
using SokobanUltimate.GameLogic.Interfaces;

namespace SokobanUltimate.GameLogic.Menus;

public class MainMenu : IMenu
{
    public List<IMenu> ChildMenus { get; set; }

    public void ProcessCommands()
    {
        throw new System.NotImplementedException();
    }
}