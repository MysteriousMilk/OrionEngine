using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Orion.Core;
using Orion.Core.Managers;
using Orion.Core.Module;
using SQLite.Net.Interop;
using System;

namespace ParticleSystemExample
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        Camera2D _camera;
        SceneBase _scene;
        Entity _ship;

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
            LogManager.Instance.SetOutputStream(Console.Out);
            OrionEngine.Initialize(_graphics, Content);

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

            ContentManager.Instance.Load("default_par.png", "def_particle", ContentType.Texture);
            ContentManager.Instance.Load("gen_figher_01.png", "fighter", ContentType.Texture);

            _scene = new SceneBase(GraphicsDevice, _camera);

            Sprite shipSprite = new Sprite("fighter");
            shipSprite.ZOrder = 2;

            _ship = new Entity();
            _ship.Attach(shipSprite);

            ParticleSystemSettings pss = new ParticleSystemSettings(
                OrionEngine.Randomizer,
                ContentManager.Instance.Get("def_particle", ContentType.Texture) as Texture2D
                );
            ParticleEmitter emitter = new ParticleEmitter(pss);
            emitter.Settings.StartVelocity = new Range<Vector2>(
                new Vector2(5, 15),
                new Vector2(-5, 15),
                OrionEngine.Randomizer
                );
            emitter.Settings.EndVelocity = new Range<Vector2>(
                new Vector2(5, 15),
                new Vector2(-5, 15),
                OrionEngine.Randomizer
                );
            emitter.Settings.StartColor = new Range<Color>(
                Color.Yellow,
                Color.Orange,
                OrionEngine.Randomizer
                );
            emitter.Settings.EndColor = new Range<Color>(
                new Color(Color.Yellow, 25),
                new Color(Color.Orange, 25),
                OrionEngine.Randomizer
                );
            emitter.Settings.Alpha = new Range<int>(150, 25, OrionEngine.Randomizer);
            emitter.Settings.Size = new Range<float>(0.7f, 0.2f, OrionEngine.Randomizer);
            emitter.Start();
            emitter.Position = new Vector2(0, 20);

            _ship.Attach(emitter);
            _ship.Position = new Vector2(0, -100);
            _ship.Rotation = 0;

            _scene.Add(_ship);
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

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                float rotation = _ship.Rotation + 25f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (rotation > 360)
                    rotation -= 360;
                _ship.Rotation = rotation;
            }

            _scene.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _scene.Draw();

            base.Draw(gameTime);
        }
    }
}
