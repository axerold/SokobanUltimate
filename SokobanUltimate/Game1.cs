using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SokobanUltimate.Drawing;
using SokobanUltimate.GameLogic;
using Serilog;
using SokobanUltimate.GameLogic.Entities;
using SokobanUltimate.GameLogic.Menus;

namespace SokobanUltimate;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameState _gameState;

    private UIManager _uiManager;

    private Animation playerAnimation;
    private AnimationManager _animationManager;
    private TextureManager _textureManager;
    private VisualPositionsManager _visualPositionsManager = new();
    
    private MenuManager _menuManager;
    private MenuRenderer _menuRenderer;

    private static int CellSize = 32;
    private int indent = 3;
    private float moveSpeed = CellSize / GameState.MoveCoolDown;
    private SpriteFont _mainFont;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _gameState = new GameState();
        _menuManager = new MenuManager();
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day).CreateLogger();
        Log.Information("Игра запущена");
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _textureManager = new TextureManager(32);
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _textureManager.LoadTextures(Content);
        
        _mainFont = Content.Load<SpriteFont>("retro");
        _menuRenderer = new MenuRenderer(_mainFont);
    }

    protected override void Update(GameTime gameTime)
    {
        _menuManager.Update();
        if (_menuManager.ToExit) Exit();

        if (GameState.GetCurrentLevel() is not null && !_visualPositionsManager.Initialized)
        {
            _visualPositionsManager.Initialize(_textureManager);
            _uiManager = new UIManager(_mainFont, CellSize, indent);
            _animationManager = new AnimationManager(_textureManager);
        }
        _gameState.UpdateLevel(gameTime);
        if (GameState.RestartActivated)
        {
            _visualPositionsManager.Initialize(_textureManager);
            _animationManager.Reload();
            GameState.RestartActivated = false;
        }

        if (_visualPositionsManager.Initialized)
        {
            _animationManager.Update(gameTime);
            _visualPositionsManager.Update(_textureManager, gameTime, moveSpeed);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        
        _spriteBatch.Begin();
        if (MenuManager.CurrentMenu is not null) 
            _menuRenderer.Render(_spriteBatch, _textureManager);
        if (GameState.GetCurrentLevel() is not null)
        {
            LayersDrawing(_spriteBatch);
            _uiManager.DrawUI(_spriteBatch);
        }

        _spriteBatch.End();
        
        base.Draw(gameTime);
    }

    private void LayersDrawing(SpriteBatch batch)
    {
        var cells = GameState.GetCurrentLevel().Cells;
        foreach (var cell in cells)
        {
            var texture = _textureManager.GetTextureForEntity(cell.Landlord);
            var position = _textureManager.GetTexturePosition(cell.Landlord);
            if (texture is not null)
                batch.Draw(texture, position, Color.White);
        }

        foreach (var cell in cells)
        {
            foreach (var tenant in cell.Tenants)
            {
                var position = _visualPositionsManager.GetVisualPosition(tenant);
                if (tenant is Player)
                {
                    var sourceRectangle = _animationManager.GetCurrentFrame(tenant);
                    batch.Draw(_textureManager.GetTextureForEntity(tenant), position, sourceRectangle: sourceRectangle, 
                        Color.White, 0f, Vector2.Zero, 
                        new Vector2(32f / 24f, 32f / 24f), SpriteEffects.None, 0);
                    
                    continue;
                }
                var texture = _textureManager.GetTextureForEntity(tenant);
                
                if (texture is not null)
                    batch.Draw(texture, position, Color.White);
            }
        }
    }
}