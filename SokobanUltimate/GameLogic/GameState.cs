using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SokobanUltimate.GameLogic;

public class GameState
{
    private static ILevel currentLevel;

    public static List<KeyValuePair<IEntity, Action>> ActionList = new();

    public float cooldownTimer = 0.0f;
    public float moveCoolDown = 0.15f;

    public void LoadLevel(string charLevelMap)
    {
        currentLevel = new Level(charLevelMap);
        ActionList.Clear();
    }

    public void UpdateLevel(GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        cooldownTimer -= deltaTime;
        if (!(cooldownTimer <= 0.0f)) return;
        cooldownTimer = moveCoolDown;
        currentLevel.Update();
    }

    public ILevel GetCurrentLevel() => currentLevel;
}