using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Serilog;
using SokobanUltimate.GameLogic;
using SokobanUltimate.GameLogic.Entities;
using SokobanUltimate.GameLogic.Interfaces;
using SokobanUltimate.GameLogic.Levels;

namespace SokobanUltimate.Drawing;

public class Animation
{
    public AnimationProperties Properties { get; }
    private float _timer;
    private int _currentFrame;

    private IEntity _entity;
    public bool IsActive { get; private set; }

    public Animation(AnimationProperties properties, IEntity entity)
    {
        Properties = properties;
        _entity = entity;
    }

    public void Start()
    {
        IsActive = true;
        _currentFrame = 0;
        _timer = 0.0f;
    }

    public void Stop()
    {
        IsActive = false;
    }

    public void Update(GameTime gameTime)
    {
        if (!IsActive) return;
        _timer += (float) gameTime.ElapsedGameTime.TotalSeconds;

        if (!(_timer > Properties.FrameDuration)) return;
        _timer -= Properties.FrameDuration;
        _currentFrame = (_currentFrame + 1) % Properties.FrameCount;
    }

    public Rectangle GetCurrentFrame()
    {
        var valueX = _currentFrame % (Properties.Texture.Width / Properties.FrameWidth) * Properties.FrameWidth;
        var valueY = 0;
        if (_entity is not Player player)
            return new Rectangle(valueX, valueY, Properties.FrameWidth, Properties.FrameHeight);
        
        valueY = AnimationManager.Directions.IndexOf(player.LastDirection) * Properties.FrameHeight;

        return new Rectangle(valueX, valueY, Properties.FrameWidth, Properties.FrameHeight);
    }

}