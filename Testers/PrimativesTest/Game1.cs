using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Orion.Core;
using Orion.Core.Module;
using SQLite.Net.Interop;

namespace PrimativesTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        Camera2D _camera;
        Scene _scene;
        Line2D _line;

        public Game1(IPlatformModuleLoader loader, ISQLitePlatform dbPlatform)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            OrionEngine.Initialize(this, _graphics);

            _camera = new Camera2D(this);
            _camera.Enabled = true;
            Components.Add(_camera);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _scene = new Scene(GraphicsDevice, _camera);

            Rectangle2D rect = new Rectangle2D(Vector2.Zero, 400, 400, Color.Yellow, Color.Black, 2);
            _scene.Add(rect);

            _line = new Line2D(new Vector2(-200, -0), new Vector2(200, 0), Color.Black, 2);
            _scene.Add(_line);

            Circle2D circle = new Circle2D(Vector2.Zero, 200, Color.Black, 2);
            _scene.Add(circle);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _line.Rotation = ((_line.Rotation + 1.0f) * (float)gameTime.ElapsedGameTime.TotalSeconds) - 360.0f;

            _scene.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _scene.Draw();

            base.Draw(gameTime);
        }
    }
}
