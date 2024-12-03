using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SokobanUltimate.GameLogic;

public class GameState
{
    private static ILevel _currentLevel;

    public float cooldownTimer;
    public float moveCoolDown = 0.12f;

    public void LoadLevel(string charLevelMap)
    {
        _currentLevel = new Level(charLevelMap);
    }

    public void UpdateLevel(GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        cooldownTimer -= deltaTime;
        if (!(cooldownTimer <= 0.0f)) return;
        cooldownTimer = moveCoolDown;
        _currentLevel.Update();
    }

    public static ILevel GetCurrentLevel() => _currentLevel;
}