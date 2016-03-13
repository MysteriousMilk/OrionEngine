using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Orion.Core;
using Orion.Core.Effect;
using Orion.Core.Managers;
using Orion.Core.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.StandardComponent
{
    public class StandardGameComponent : DrawableGameComponent, IComponent
    {
        #region Fields
        private RenderTarget2D _renderTarget;
        private GraphicsDevice _graphicsDevice;
        private ICamera2D _camera;
        #endregion

        public IScene CurrentScene
        {
            get;
            set;
        }

        public PostProcessor PostProcessor
        {
            get;
            internal set;
        }

        public PreProcessor PreProcessor
        {
            get;
            internal set;
        }

        public StandardGameComponent(Game game, ICamera2D camera)
            : base(game)
        {
            _graphicsDevice = game.GraphicsDevice;
            _camera = camera;
        }

        public ICamera2D GetCamera()
        {
            return _camera;
        }

        public void AddSceneObject(OrionObject obj)
        {
            if(this.CurrentScene != null)
                this.CurrentScene.Add(obj);
        }

        public RenderTarget2D GetSnapshot()
        {
            if (CurrentScene == null)
                throw new InvalidOperationException("There must be a current scene in order to take a snapshot.");

            RenderTarget2D renderTarget = new RenderTarget2D(
                _graphicsDevice,
                _graphicsDevice.Viewport.Width,
                _graphicsDevice.Viewport.Height);

            _graphicsDevice.SetRenderTarget(renderTarget);
            CurrentScene.Draw();
            _graphicsDevice.SetRenderTarget(null);

            return renderTarget;
        }

        public void LoadSceneFromModule(Module module, string sceneRef)
        {
            try
            {
                this.CurrentScene = StandardScene.LoadFromModule(module, sceneRef, _graphicsDevice, _camera);

                // should unload old resources here
            }
            catch(Exception)
            {
                LogManager.Instance.LogError("Error loading scene from module.");
            }
        }

        #region DrawableGameComponent Overrides
        public override void Initialize()
        {
            PreProcessor = new PreProcessor();
            PostProcessor = new PostProcessor(_graphicsDevice);

            _renderTarget = new RenderTarget2D(
                _graphicsDevice,
                _graphicsDevice.Viewport.Width,
                _graphicsDevice.Viewport.Height,
                false,
                _graphicsDevice.PresentationParameters.BackBufferFormat,
                _graphicsDevice.PresentationParameters.DepthStencilFormat,
                _graphicsDevice.PresentationParameters.MultiSampleCount,
                RenderTargetUsage.DiscardContents
                );

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.CurrentScene != null)
                this.CurrentScene.Update(gameTime);

            PostProcessor.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // run the pre processor
            PreProcessor.Apply();

            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.Black);

            if (this.CurrentScene != null)
                this.CurrentScene.Draw();

            PostProcessor.Apply(_renderTarget, null);

            base.Draw(gameTime);
        }
        #endregion
    }
}
