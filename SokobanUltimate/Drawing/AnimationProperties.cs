using Microsoft.Xna.Framework.Graphics;

namespace SokobanUltimate.Drawing;

public struct AnimationProperties(Texture2D texture, int frameWidth, int frameHeight, 
    int frameCount, float frameDuration)
{
    public readonly Texture2D Texture = texture;
    public readonly int FrameWidth = frameWidth;
    public readonly int FrameHeight = frameHeight;
    public readonly int FrameCount = frameCount;
    public readonly float FrameDuration = frameDuration;
}