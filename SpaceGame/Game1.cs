using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using SpaceGame.Effects;
using SpaceGame.Managers;
using SpaceGame.Sprites;
using System;
using System.Collections.Generic;

namespace SpaceGame
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera2D camera;

        public static Random r;
        public static float zoom = 3f;
        public static Dictionary<string, Texture2D> textures;
        public static Vector2 screenSize { get { return new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height); } }
        public static Vector2 zoomedScreenSize { get { return screenSize / zoom; } }
        public static Vector2 positionCenter { get { return playerManager.playerPosition; } }
        public static Vector2 screenCenter { get { return screenSize / 2f; } }
        public static PlayerManager playerManager;
        
        Vector2 windowSize { get { return new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height); } }
        Vector2 windowCenter { get { return windowSize / 2f; } }

        private WorldManager _worldManager;
        private ParticleManager _particleManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = (int)screenSize.X;
            graphics.PreferredBackBufferHeight = (int)screenSize.Y;
            graphics.IsFullScreen = true;
        }

        protected override void Initialize()
        {
            camera = new Camera2D(GraphicsDevice)
            {
                Zoom = zoom,
                Position = -screenSize / 2f
            };
            IsMouseVisible = true;
            IsFixedTimeStep = true;
            graphics.SynchronizeWithVerticalRetrace = true;
            r = new Random();
            base.Initialize();
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void LoadContent()
        {
            textures = new Dictionary<string, Texture2D>()
            {
                { "basic_ship_main", Content.Load<Texture2D>("Ships/PlayerShips/basic_ship_main") },
                { "basic_ship_wings", Content.Load<Texture2D>("Ships/PlayerShips/basic_ship_wings") },
                { "smoke", Content.Load<Texture2D>("Effects/smoke") },
            };

            playerManager = new PlayerManager(camera);
            _worldManager = new WorldManager();
            _particleManager = new ParticleManager();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            playerManager.Update(gameTime);
            _worldManager.Update(gameTime);
            _particleManager.AddParticle(new Smoke(playerManager.playerPosition));
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, transformMatrix: camera.GetViewMatrix()); 
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.DrawRectangle(new Rectangle(-10, -10, 20, 20), Color.Red);
            _worldManager.Draw(spriteBatch);
            playerManager.Draw(spriteBatch);
            _particleManager.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
