using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Orion.Core;
using Orion.Core.Managers;
using Orion.Core.Module;
using Orion.Core.Physics;
using SQLite.Net.Interop;
using System;

namespace PhysicsExample
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        Scene _scene;
        Player _player;

        KeyboardState _prevKeyState;

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
            OrionEngine.Instance.RegisterComponent(new PhysicsComponent(this, new Vector2(0.0f, 9.8f), 64f));

            LogManager.Instance.SetOutputStream(Console.Out);

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

            ContentManager.Instance.Load("green.png", "GreenBlock", ContentType.Texture);
            ContentManager.Instance.Load("ground.png", "GroundBlock", ContentType.Texture);
            ContentManager.Instance.Load("player_body.png", "PlayerBase", ContentType.Texture);
            ContentManager.Instance.Load("arm_back.png", "ArmBack", ContentType.Texture);
            ContentManager.Instance.Load("arm_front.png", "ArmFront", ContentType.Texture);
            ContentManager.Instance.Load("gun1.png", "Gun", ContentType.Texture);

            _scene = new Scene(GraphicsDevice, OrionEngine.Instance.GetComponent<Camera2D>());

            for (int x = -256; x <= 256; x += 64)
            {
                Entity floorBlock = new Entity();
                floorBlock.Attach(new Sprite("GroundBlock"));

                BoxCollider collider = OrionEngine.Instance.GetComponent<PhysicsComponent>().CreateBoxCollider(64, 64, 1, ColliderType.Static);
                floorBlock.Attach(collider);

                floorBlock.Position = new Vector2(x, 192);

                _scene.Add(floorBlock);
            }

            _player = new Player();
            _player.Position = new Vector2(0, -128);
            _scene.Add(_player);

            OrionEngine.Instance.CurrentScene = _scene;
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

            _prevKeyState = Keyboard.GetState();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
