using Microsoft.Xna.Framework;
using System.Xml;
using System.Xml.Linq;

namespace Orion.Core.Factories
{
    public class CoreSceneFactory : ISceneFactory
    {
        public bool CanHandle(string sceneTypeName)
        {
            bool canHandle = false;

            switch (sceneTypeName)
            {
                case "Scene":
                    canHandle = true;
                    break;
                case "ParallaxScene":
                    canHandle = true;
                    break;
            }

            return canHandle;
        }

        public IScene GetScene(string objectTypeName, IFactoryManager manager, Module.Module module, XElement xmlNode)
        {
            IScene scene = null;

            if (manager is OrionEngine)
            {
                OrionEngine engine = (OrionEngine)manager;
                Camera2D camera = engine.GetComponent<Camera2D>();

                if (camera != null)
                {
                    if (objectTypeName.Equals("Scene"))
                        scene = new Scene(engine.GraphicsDM.GraphicsDevice, camera);
                    else if (objectTypeName.Equals("ParallaxScene"))
                        scene = new ParallaxScene(engine.GraphicsDM.GraphicsDevice, camera);                        
                }

                if (scene != null)
                {
                    foreach (XElement sceneNode in xmlNode.Elements())
                    {
                        switch (sceneNode.Name.LocalName)
                        {
                            case "Header":
                                if (scene is Scene)
                                    LoadHeader(module, sceneNode, scene as Scene);
                                else if (scene is ParallaxScene)
                                    LoadHeader(module, sceneNode, scene as ParallaxScene);
                                break;

                            case "EntityList":
                                LoadEntityList(module, manager, sceneNode, scene);
                                break;
                        }
                    }
                }
            }

            return scene;
        }

        public void LoadHeader(Module.Module module, XElement xmlNode, Scene scene)
        {
            float width = 0.0f;
            float height = 0.0f;

            foreach (XElement headerNode in xmlNode.Elements())
            {
                switch (headerNode.Name.LocalName)
                {
                    case "Name":
                        scene.Name = headerNode.Value;
                        break;
                    case "Width":
                        width = XmlConvert.ToSingle(headerNode.Value);
                        break;
                    case "Height":
                        height = XmlConvert.ToSingle(headerNode.Value);
                        break;
                }
            }

            scene.Dimensions = new Dimension(width, height);
        }

        public void LoadHeader(Module.Module module, XElement xmlNode, ParallaxScene scene)
        {
            float width = 0.0f;
            float height = 0.0f;

            foreach (XElement headerNode in xmlNode.Elements())
            {
                switch (headerNode.Name.LocalName)
                {
                    case "Name":
                        scene.Name = headerNode.Value;
                        break;
                    case "Width":
                        width = XmlConvert.ToSingle(headerNode.Value);
                        break;
                    case "Height":
                        height = XmlConvert.ToSingle(headerNode.Value);
                        break;
                    case "Background":
                        {
                            string bkgRef = headerNode.Attribute("Ref").Value;
                            module.LoadTexture(bkgRef);
                            scene.SetBackground(bkgRef);
                        }
                        break;
                    case "ParallaxLayerList":
                        LoadParallaxSurfaces(headerNode, scene);
                        break;
                }
            }

            scene.Dimensions = new Dimension(width, height);
        }

        private void LoadParallaxSurfaces(XElement xmlNode, ParallaxScene scene)
        {
            foreach (XElement element in xmlNode.Elements())
            {
                if (element.Name.LocalName == "ParallaxSurface")
                {
                    string textureRef = string.Empty;
                    Vector2 scrollVec = Vector2.Zero;
                    float scrollFactor = 0.0f;

                    foreach (XElement childElement in element.Elements())
                    {
                        switch (childElement.Name.LocalName)
                        {
                            case "Texture":
                                textureRef = childElement.Attribute("Ref").Value;
                                break;
                            case "ScrollVector":
                                {
                                    float x = XmlConvert.ToSingle(childElement.Attribute("X").Value);
                                    float y = XmlConvert.ToSingle(childElement.Attribute("Y").Value);
                                    scrollVec = new Vector2(x, y);
                                }
                                break;
                            case "ScrollFactor":
                                scrollFactor = XmlConvert.ToSingle(childElement.Value);
                                break;
                        }
                    }

                    ParallaxSurface surface = new ParallaxSurface(textureRef);
                    surface.Scroll = scrollVec;
                    surface.ScrollFactor = scrollFactor;
                    scene.ParallaxLayers.Add(surface);
                }
            }
        }

        private void LoadEntityList(Module.Module module, IFactoryManager manager, XElement xmlNode, IScene scene)
        {
            foreach (XElement entityNode in xmlNode.Elements())
            {
                if (entityNode.Name.LocalName == "Entity")
                {
                    string objectTypeName = string.Empty;
                    foreach (XAttribute attrib in entityNode.Attributes())
                    {
                        if (attrib.Name.LocalName.Equals("Type"))
                        {
                            objectTypeName = attrib.Value;
                            break;
                        }
                    }

                    IObjectFactory fac = manager.GetFactoryFor<IObjectFactory>(objectTypeName);
                    scene.Add(fac.GetObject(objectTypeName, manager, module, entityNode));
                }
            }
        }
    }
}
