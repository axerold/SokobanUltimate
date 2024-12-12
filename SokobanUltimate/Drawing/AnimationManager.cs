using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Serilog;
using SokobanUltimate.GameLogic;
using SokobanUltimate.GameLogic.Actions;
using SokobanUltimate.GameLogic.Entities;
using SokobanUltimate.GameLogic.Interfaces;
using SokobanUltimate.GameLogic.Levels;

namespace SokobanUltimate.Drawing;

public class AnimationManager
{
    public static readonly List<IntVector2> Directions =
    [
        new(0, -1), new(0, 1),
        new(1, 0), new(-1, 0)
    ];

    private Dictionary<IEntity, Animation> _entityAnimations = [];
    private TextureManager _textureManager;

    public AnimationManager(TextureManager manager)
    {
        _textureManager = manager;
        InitializeAnimations();
    }

    public void Reload()
    {
        InitializeAnimations();
    }

    public void Update(GameTime gameTime)
    {
        if (GameState.State is LevelState.Loss or LevelState.Win) return;
        
        foreach (var (entity, animation) in _entityAnimations)
        {
            if (entity is Player player)
            {
                switch (player.LastAction.CommandType)
                {
                    case CommandType.IDLE:
                        if (animation.IsActive) animation.Stop();
                        break;
                    case CommandType.MOVE:
                        if (!animation.IsActive) animation.Start();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (entity is Belt belt)
            {
                if (!animation.IsActive)
                    animation.Start();
            }
            animation.Update(gameTime);
        }
    }

    public Rectangle GetCurrentFrame(IEntity entity)
    {
        _entityAnimations.TryGetValue(entity, out var animation);
        return animation?.GetCurrentFrame() ?? new Rectangle();
    }

    private AnimationProperties GetAnimationProperties(IEntity entity)
    {
        return entity switch
        {
            Player => new AnimationProperties(_textureManager.GetTextureForEntity(entity), 24, 24,
                4, GameState.MoveCoolDown / 2f),
            Belt => new AnimationProperties(_textureManager.GetTextureForEntity(entity), 32, 32,
                32, 0.1f),
            _ => new AnimationProperties()
        };
    }

    private void InitializeAnimations()
    {
        foreach (var cell in GameState.GetCurrentLevel().Cells)
        {
            foreach (var tenant in cell.Tenants)
            {
                if (tenant is Player or Belt)
                    _entityAnimations[tenant] = new Animation(GetAnimationProperties(tenant), tenant);
            }
        }    
    }
}