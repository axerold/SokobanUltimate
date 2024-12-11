using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SokobanUltimate.Drawing;
using SokobanUltimate.GameContent;
using SokobanUltimate.GameLogic;
using Serilog;
using Serilog.Core;
using SokobanUltimate.GameLogic.Entities;
using SokobanUltimate.GameLogic.Interfaces;

namespace SokobanUltimate;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameState _gameState;

    private Texture2D _playerTexture;
    private Texture2D _playerMoveSprites;
    private Texture2D _boxTexture;
    private Texture2D _wallTexture;
    private Texture2D _collectorTexture;

    private Dictionary<IEntity, Vector2> visualPositions = new();

    private UIManager _uiManager;

    private Animation playerAnimation;
    private AnimationManager _animationManager;

    private static int CellSize = 32;
    private int indent = 3;
    private float moveSpeed = CellSize / GameState.MoveCoolDown;
    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _gameState = new GameState();
        GameState.LoadLevel(CharMaps.LevelOne);
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day).CreateLogger();
        Log.Information("Игра запущена");
        InitializeVisualPositions();
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _playerTexture = Content.Load<Texture2D>("beards");
        _boxTexture = Content.Load<Texture2D>("box");
        _collectorTexture = Content.Load<Texture2D>("collector");
        _wallTexture = Content.Load<Texture2D>("wall");
        _playerMoveSprites = Content.Load<Texture2D>("player_move");
        
        var mainFont = Content.Load<SpriteFont>("MainFont");
        _uiManager = new UIManager(mainFont, CellSize, indent);
        _animationManager = new AnimationManager(_playerMoveSprites);
    }

    protected override void Update(GameTime gameTime)
    {
        /*if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();*/

        _gameState.UpdateLevel(gameTime);
        if (GameState.RestartActivated)
        {
            InitializeVisualPositions();
            _animationManager.Reload();
            GameState.RestartActivated = false;
        }
        _animationManager.Update(gameTime);
        UpdateVisualPositions(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        
        _spriteBatch.Begin();
        LayersDrawing(_spriteBatch);
        _uiManager.DrawUI(_spriteBatch);
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }

    private void LayersDrawing(SpriteBatch batch)
    {
        var cells = GameState.GetCurrentLevel().Cells;
        foreach (var cell in cells)
        {
            var texture = GetTextureByEntity(cell.Landlord);
            var position = GetTexturePosition(cell.Landlord);
            if (texture is not null)
                batch.Draw(texture, position, Color.White);
        }

        foreach (var cell in cells)
        {
            foreach (var tenant in cell.Tenants)
            {
                var position = visualPositions[tenant];
                if (tenant is Player)
                {
                    var sourceRectangle = _animationManager.GetCurrentFrame(tenant);
                    batch.Draw(_playerMoveSprites, position, sourceRectangle: sourceRectangle, 
                        Color.White, 0f, Vector2.Zero, 
                        new Vector2(32f / 24f, 32f / 24f), SpriteEffects.None, 0);
                    
                    continue;
                }
                var texture = GetTextureByEntity(tenant);
                
                if (texture is not null)
                    batch.Draw(texture, position, Color.White);
            }
        }
    }

    private Texture2D GetTextureByEntity(IEntity entity)
    {
        return entity switch
        {
            Player => _playerTexture,
            Box => _boxTexture,
            BoxCollector => _collectorTexture,
            Wall => _wallTexture,
            _ => null
        };
    }

    private Vector2 GetTexturePosition(IEntity entity)
    {
        return new Vector2(entity.Location.X * CellSize, entity.Location.Y * CellSize);
    }

    private void InitializeVisualPositions()
    {
        foreach (var cell in GameState.GetCurrentLevel().Cells)
        {
            foreach (var tenant in cell.Tenants)
            {
                visualPositions[tenant] = GetTexturePosition(tenant);
            }
        }
    }

    private void UpdateVisualPositions(GameTime gameTime)
    {
        foreach (var (tenant, visualLocation) in visualPositions)
        {
            var currentLocation = GetTexturePosition(tenant);
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
}