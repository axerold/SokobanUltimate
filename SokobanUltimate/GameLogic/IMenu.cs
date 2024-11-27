using System.Collections.Generic;

namespace SokobanUltimate.GameLogic;

public interface IMenu
{
    public List<IMenu> ChildMenus { get; set; }

    public void ProcessCommands();
}