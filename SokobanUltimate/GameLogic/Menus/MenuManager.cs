using System;
using System.IO;
using Microsoft.Xna.Framework;
using SokobanUltimate.GameContent;

namespace SokobanUltimate.GameLogic.Menus;
using System.Text.Json;

public class MenuManager
{
   public static Menu CurrentMenu { get; private set; }
   public bool ToExit { get; private set; }

   public MenuManager()
   {
      LoadMenu("Menus/MainMenu.json");   
   }

   public void LoadMenu(string jsonPath)
   {
      var json = File.ReadAllText(jsonPath);
      CurrentMenu = JsonSerializer.Deserialize<Menu>(json);
   }

   public void Update()
   {
      var action = CurrentMenu?.CheckButtonPressed();
      if (action is null) return;
      switch (action)
      {
         case "start":
            CurrentMenu = null;
            GameState.LoadLevel(CharMaps.LevelOne);
            break;
         case "exit":
            ToExit = true;
            break;
      }
   }
}