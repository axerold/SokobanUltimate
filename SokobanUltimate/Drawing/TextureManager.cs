using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SokobanUltimate.GameLogic.Entities;
using SokobanUltimate.GameLogic.Interfaces;

namespace SokobanUltimate.Drawing;

public class TextureManager
{
    public readonly int CellSize;
    private Texture2D _playerMoveSprites;
    private Texture2D _boxTexture;
    private Texture2D _wallTexture;
    private Texture2D _collectorTexture;

    private Texture2D _buttonTexture;

    public TextureManager(int cellSize)
    {
        CellSize = cellSize;
    }

    public void LoadTextures(ContentManager manager)
    {
        _boxTexture = manager.Load<Texture2D>("box");
        _collectorTexture = manager.Load<Texture2D>("collector");
        _wallTexture = manager.Load<Texture2D>("wall");
        _playerMoveSprites = manager.Load<Texture2D>("player_move");
        
        _buttonTexture = manager.Load<Texture2D>("button");
    }

    public Texture2D GetTextureForEntity(IEntity entity)
    {
        return entity switch
        {
            Player => _playerMoveSprites,
            Box => _boxTexture,
            BoxCollector => _collectorTexture,
            Wall => _wallTexture,
            _ => null
        };
    }

    public Texture2D GetTextureForUI(string text)
    {
        return text switch
        {
            "button" => _buttonTexture,
            _ => null
        };
    }

    public Vector2 GetTexturePosition(IEntity entity)
    {
        return new Vector2(entity.Location.X * CellSize, entity.Location.Y * CellSize);
    }
}