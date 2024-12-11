using System.Collections.Generic;
using SokobanUltimate.GameLogic.Interfaces;

namespace SokobanUltimate.GameLogic.Menus;

public class Menu
{
    public readonly string Title;
    public readonly List<Menu> ChildMenus;
    public readonly Menu parentMenu;

    private Menu _onSelect;
    
    public Menu(string title)
    {
        Title = title;
    }

    public void Invoke()
    {
        
    }
    public void ProcessCommands()
    {
        throw new System.NotImplementedException();
    }
}