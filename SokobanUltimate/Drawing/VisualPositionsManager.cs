using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SokobanUltimate.GameLogic;
using SokobanUltimate.GameLogic.Interfaces;

namespace SokobanUltimate.Drawing;

public class VisualPositionsManager
{
    private Dictionary<IEntity, Vector2> visualPositions = new();
    public bool Initialized { get; private set; }

    public void Initialize(TextureManager manager)
    {
        Initialized = true;
        foreach (var cell in GameState.GetCurrentLevel().Cells)
        {
            foreach (var tenant in cell.Tenants)
            {
                visualPositions[tenant] = manager.GetTexturePosition(tenant);
            }
        }
    }

    public void Update(TextureManager manager, GameTime gameTime, float moveSpeed)
    {
        foreach (var (tenant, visualLocation) in visualPositions)
        {
            var currentLocation = manager.GetTexturePosition(tenant);
            if (currentLocation == visualLocation)
            {
                continue;
            }

            var direction = Vector2.Normalize(currentLocation - visualLocation);
            var distanceToMove = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Vector2.Distance(currentLocation, visualLocation) <= distanceToMove)
            {
                visualPositions[tenant] = currentLocation;
            }
            else
            {
                visualPositions[tenant] += direction * distanceToMove;
            }
        }
    }

    public Vector2 GetVisualPosition(IEntity tenant)
    {
        visualPositions.TryGetValue(tenant, out var position);
        return position;
    }
    
}