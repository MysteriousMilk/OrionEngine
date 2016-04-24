using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Orion.Core.Effect;
using Orion.Core.Factories;
using Orion.Core.Managers;
using System;
using System.Xml.Linq;

namespace Orion.Core
{
    public class StandardGameComponent : DrawableGameComponent
    {
        #region Fields
        private RenderTarget2D _renderTarget;
        private GraphicsDevice _graphicsDevice;
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

        public StandardGameComponent(Game game)
            : base(game)
        {
            _graphicsDevice = game.GraphicsDevice;


            Camera2D camera = OrionEngine.Instance.GetComponent<Camera2D>();

            if (camera == null)
            {
                camera = new Camera2D(game);
                camera.Enabled = true;
                OrionEngine.Instance.RegisterComponent(camera);
            }

            OrionEngine.Instance.RegisterComponent(this);
        }

        public void AddSceneObject(GameObject obj)
        {
            if(CurrentScene != null)
                CurrentScene.Add(obj);
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

        public void LoadSceneFromModule(Module.Module module, string sceneRef)
        {
            try
            {
                XDocument doc = XDocument.Parse(module.GetFileXML(sceneRef, ResourceType.Scene));

                XElement root = doc.Element("Scene");
                string typeName = root.Attribute("Type").Value;

                ISceneFactory fac = OrionEngine.Instance.GetFactoryFor<ISceneFactory>(typeName);
                CurrentScene = fac.GetScene(typeName, OrionEngine.Instance, module, root);
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
            if (CurrentScene != null)
                CurrentScene.Update(gameTime);

            PostProcessor.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // run the pre processor
            PreProcessor.Apply();

            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.Black);

            if (CurrentScene != null)
                CurrentScene.Draw();

            PostProcessor.Apply(_renderTarget, null);

            base.Draw(gameTime);
        }
        #endregion
    }
}
