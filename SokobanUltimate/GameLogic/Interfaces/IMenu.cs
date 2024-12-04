using System.Collections.Generic;

namespace SokobanUltimate.GameLogic.Interfaces;

public interface IMenu
{
    public List<IMenu> ChildMenus { get; set; }

    public void ProcessCommands();
}