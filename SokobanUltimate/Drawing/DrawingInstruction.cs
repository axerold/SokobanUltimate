namespace SokobanUltimate.Drawing;

public class DrawingInstruction(
    bool drawTime = false,
    bool drawStepsCounter = false,
    bool drawPauseMenu = false,
    bool drawWinScreen = false,
    bool drawLossScreen = false)
{
    public bool DrawTime { get; } = drawTime;
    public bool DrawStepsCounter { get; } = drawStepsCounter;
    public bool DrawPauseMenu { get; } = drawPauseMenu;
    public bool DrawWinScreen { get; } = drawWinScreen;
    public bool DrawLossScreen { get; } = drawLossScreen;
}