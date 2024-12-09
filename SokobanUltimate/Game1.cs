﻿using System.Collections.Generic;
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
    private Texture2D _boxTexture;
    private Texture2D _wallTexture;
    private Texture2D _collectorTexture;

    private UIManager _uiManager;

    private int CellSize = 32;
    private int indent = 3;
    
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
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _playerTexture = Content.Load<Texture2D>("beards");
        _boxTexture = Content.Load<Texture2D>("box");
        _collectorTexture = Content.Load<Texture2D>("collector");
        _wallTexture = Content.Load<Texture2D>("wall");
        
        var mainFont = Content.Load<SpriteFont>("MainFont");
        _uiManager = new UIManager(mainFont, CellSize, indent);
    }

    protected override void Update(GameTime gameTime)
    {
        /*if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();*/

        _gameState.UpdateLevel(gameTime);

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
            foreach (var tenant in cell.Tenants)
            {
                texture = GetTextureByEntity(tenant);
                position = GetTexturePosition(tenant);
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
}