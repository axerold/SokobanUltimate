using System.Collections.Generic;

namespace SokobanUltimate.GameLogic;

public class SubMenu : IMenu
{
    public List<IMenu> ChildMenus { get; set; }

    public void ProcessCommands()
    {
        throw new System.NotImplementedException();
    }
}