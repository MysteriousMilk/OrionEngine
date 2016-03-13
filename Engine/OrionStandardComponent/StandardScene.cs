using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Orion.Core.Module;
using System.Xml;
using Orion.Core;
using Orion.Core.UI;
using Orion.Core.Factories;
using Orion.Core.Entity;

namespace Orion.StandardComponent
{
    public class StandardScene : SceneBase
    {
        private SpriteBatch _bkgSpriteBatch;
        private SpriteBatch _overlaySpriteBatch;

        #region Properties
        public Texture2D Background
        {
            get;
            set;
        }

        public List<ParallaxSurface> ParallaxLayers
        {
            get;
            set;
        }

        public BlendState BackgroundBlendState
        {
            get;
            set;
        }
        #endregion

        public StandardScene(GraphicsDevice graphics, ICamera2D camera)
            : base(graphics, camera)
        {
            _bkgSpriteBatch = new SpriteBatch(graphics);
            _overlaySpriteBatch = new SpriteBatch(graphics);
            this.BackgroundBlendState = BlendState.AlphaBlend;

            this.ParallaxLayers = new List<ParallaxSurface>();
        }

        public void SetBackground(string backgroundRef)
        {
            this.Background = ContentManager.Instance.Get(backgroundRef, ContentType.Texture) as Texture2D;
        }

        #region SceneBase Overrides
        public override void Update(GameTime gameTime)
        {
            foreach (ParallaxSurface parallax in this.ParallaxLayers)
                parallax.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw()
        {
            _bkgSpriteBatch.Begin(SpriteSortMode.Deferred, this.BackgroundBlendState, SamplerState.LinearWrap, null, null);

            if(this.Background != null)
                _bkgSpriteBatch.Draw(this.Background, Vector2.Zero, Color.White);

            foreach(ParallaxSurface parallax in this.ParallaxLayers)
            {
                _bkgSpriteBatch.Draw(parallax.Texture, Vector2.Zero,
                    new Rectangle(
                        (int)(this.Camera.Position.X * parallax.ScrollFactor),
                        (int)(this.Camera.Position.Y * parallax.ScrollFactor),
                        Settings.Instance.ResolutionX,
                        Settings.Instance.ResolutionY
                    ),
                    Color.White);
            }

            _bkgSpriteBatch.End();

            base.Draw();
        }
        #endregion

        #region LoadFromXML
        public static IScene LoadFromModule(Module module, string reference, GraphicsDevice graphics, ICamera2D camera)
        {
            StandardScene scene = null;

            string xml = module.GetFileXML(reference, ResourceType.Scene);

            scene = new StandardScene(graphics, camera);

            XDocument doc = XDocument.Parse(xml);
            foreach (XElement sceneNode in doc.Element("Scene").Elements())
            {
                switch (sceneNode.Name.LocalName)
                {
                    case "Header":
                        LoadSceneHeader(module, sceneNode, scene);
                        break;

                    case "EntityList":
                        LoadSceneSpriteList(module, sceneNode, scene);
                        break;
                }
            }

            return scene;
        }

        private static void LoadSceneHeader(Module module, XElement node, StandardScene scene)
        {
            foreach (XElement headerNode in node.Elements())
            {
                switch (headerNode.Name.LocalName)
                {
                    case "Name":
                        scene.Name = headerNode.Value;
                        break;
                    case "Width":
                        break;
                    case "Height":
                        break;
                    case "Background":
                        {
                            string bkgRef = headerNode.Attribute("Ref").Value;
                            module.LoadTexture(bkgRef);
                            scene.SetBackground(bkgRef);
                        }
                        break;
                }
            }
        }

        private static void LoadSceneSpriteList(Module module, XElement node, StandardScene scene)
        {
            foreach (XElement spriteNode in node.Elements())
            {
                if (spriteNode.Name.LocalName == "Entity")
                {
                    string objectTypeName = string.Empty;
                    foreach (XAttribute attrib in spriteNode.Attributes())
                    {
                        if (attrib.Name.LocalName.Equals("Type"))
                        {
                            objectTypeName = attrib.Value;
                            break;
                        }
                    }

                    foreach (IObjectFactory fac in OrionEngine.Instance.ObjectFactories)
                    {
                        if (fac.CanHandle(objectTypeName))
                        {
                            scene.Add(fac.GetObject(objectTypeName, OrionEngine.Instance, module, spriteNode));
                            break;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
