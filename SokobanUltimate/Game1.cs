using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SokobanUltimate.GameContent;
using SokobanUltimate.GameLogic;

namespace SokobanUltimate;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameState _gameState;

    private Texture2D _playerTexture;
    private Texture2D _boxTexture;
    private Texture2D _wallTexture;
    private Texture2D _collectorTexture;

    private int CellSize = 32;
    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _gameState = new GameState();
        _gameState.LoadLevel(CharMaps.LevelOne);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _playerTexture = Content.Load<Texture2D>("beards");
        _boxTexture = Content.Load<Texture2D>("box");
        _collectorTexture = Content.Load<Texture2D>("collector");
        _wallTexture = Content.Load<Texture2D>("wall");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _gameState.UpdateLevel(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        
        _spriteBatch.Begin();
        foreach (var texturePosition in GetTexturesPositions())
        {
            if (texturePosition.Value is not null)
                _spriteBatch.Draw(texturePosition.Value, texturePosition.Key, Color.White);
        }
        _spriteBatch.End();
        

        base.Draw(gameTime);
    }

    private KeyValuePair<Vector2, Texture2D>[,] GetTexturesPositions()
    {
        var currentLevel = _gameState.GetCurrentLevel();
        var levelHeight = currentLevel.GetLevelHeight();
        var levelWidth = currentLevel.GetLevelWidth();
        
        var currentState = currentLevel.GetCurrentState();
        var texturesToPositions = new KeyValuePair<Vector2, Texture2D>[levelHeight, levelWidth];
        for (var i = 0; i < currentLevel.GetLevelHeight(); i++)
        {
            for (var j = 0; j < currentLevel.GetLevelWidth(); j++)
            {
                Texture2D texture2D = null;
                if (currentState[i, j] is Player) texture2D = _playerTexture;
                if (currentState[i, j] is Box) texture2D = _boxTexture;
                if (currentState[i, j] is BoxCollector) texture2D = _collectorTexture;
                if (currentState[i, j] is Wall) texture2D = _wallTexture;
                texturesToPositions[i, j] = 
                    new KeyValuePair<Vector2, Texture2D>(new Vector2(j * CellSize, i * CellSize), texture2D);

            }
        }

        return texturesToPositions;
    }
}