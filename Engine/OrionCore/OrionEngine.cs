using Microsoft.Xna.Framework;
using Orion.Core.Factories;
using Orion.Core.Managers;
using System;
using System.Collections.Generic;

namespace Orion.Core
{
    public sealed class OrionEngine : IFactoryManager
    {
        #region Singleton/Static Properties
        private static readonly OrionEngine _instance = new OrionEngine();
        public static readonly Random Randomizer = new Random();

        public static OrionEngine Instance
        {
            get
            {
                if (!_instance.IsInitialized)
                    throw new Exception("Engine must be initialized before use.");
                return _instance;
            }
        }

        public static void Initialize(GraphicsDeviceManager deviceMangager, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            _instance.GraphicsDM = deviceMangager;

            ContentManager.Instance = new ContentManager(content);
            ContentManager.Instance.Graphics = deviceMangager.GraphicsDevice;

            _instance.SetResolution(Settings.Instance.ResolutionX, Settings.Instance.ResolutionY, Settings.Instance.IsFullscreen);

            _instance.IsInitialized = true;
        }
        #endregion

        #region Construction
        private OrionEngine()
        {
            IsInitialized = false;
            GraphicsDM = null;
            Components = new List<IComponent>();

            ObjectFactories = new List<IObjectFactory>();
            ObjectFactories.Add(new CoreObjectFactory());

            DataFactories = new List<IDataFactory>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// A reference to MonoGame's GraphicsDeviceManager.
        /// </summary>
        public GraphicsDeviceManager GraphicsDM
        {
            get;
            set;
        }

        /// <summary>
        /// Returns true when the engine is initialized.
        /// </summary>
        public bool IsInitialized
        {
            get;
            set;
        }

        /// <summary>
        /// The current loaded module.
        /// </summary>
        public Module.Module CurrentModule
        {
            get;
            private set;
        }

        /// <summary>
        /// Registered Engine Components
        /// </summary>
        public List<IComponent> Components
        {
            get;
            private set;
        }

        /// <summary>
        /// The current scene being rendered to the screen.
        /// </summary>
        public IComponent ActiveComponent
        {
            get;
            private set;
        }

        public GameObject Player
        {
            get;
            set;
        }

        public IList<IObjectFactory> ObjectFactories
        {
            get;
            set;
        }

        public IList<IDataFactory> DataFactories
        {
            get;
            set;
        }
        #endregion

        public void SetCurrentModule(Module.Module module)
        {
            CurrentModule = module;
        }

        public void SetActiveComponent(IComponent component)
        {
            ActiveComponent = component;
        }

        public void RegisterComponent(IComponent component)
        {
            Components.Add(component);
        }

        public IObjectFactory GetFactoryFor(string objectTypeName)
        {
            foreach (IObjectFactory fac in ObjectFactories)
            {
                if (fac.CanHandle(objectTypeName))
                    return fac;
            }
            return null;
        }

        public IOrionDataObject GetDataObject(string dataTypeName, int id)
        {
            IOrionDataObject dataObject = null;

            foreach (IDataFactory fac in DataFactories)
            {
                if (fac.CanHandle(dataTypeName))
                {
                    dataObject = fac.GetDataObject(dataTypeName, id);
                    break;
                }
            }

            return dataObject;
        }

        public IOrionDataObject GetDataObject(string dataTypeName, string tag)
        {
            IOrionDataObject dataObject = null;

            foreach (IDataFactory fac in DataFactories)
            {
                if (fac.CanHandle(dataTypeName))
                {
                    dataObject = fac.GetDataObject(dataTypeName, tag);
                    break;
                }
            }

            return dataObject;
        }

        public Vector2 WorldToScreen(Vector2 worldPos)
        {
            return Vector2.Transform(worldPos, ActiveComponent.GetCamera().Transform);
        }

        public Vector2 ScreenToWorld(Vector2 screenPos)
        {
            return Vector2.Transform(screenPos, Matrix.Invert(ActiveComponent.GetCamera().Transform));
        }

        public double GetRandomDouble()
        {
            return Randomizer.NextDouble();
        }

        private void SetResolution(int windowSizeX, int windowSizeY, bool isFullscreen)
        {
            GraphicsDM.PreferredBackBufferWidth = windowSizeX;
            GraphicsDM.PreferredBackBufferHeight = windowSizeY;
            GraphicsDM.IsFullScreen = isFullscreen;
            GraphicsDM.ApplyChanges();
            ContentManager.Instance.Graphics = GraphicsDM.GraphicsDevice;
        }
    }
}
