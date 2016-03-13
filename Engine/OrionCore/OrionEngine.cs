using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Orion.Core.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.IsInitialized = false;
            this.GraphicsDM = null;
            this.Components = new List<IComponent>();

            this.ObjectFactories = new List<IObjectFactory>();
            this.ObjectFactories.Add(new CoreObjectFactory());

            this.DataFactories = new List<IDataFactory>();
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

        public OrionObject Player
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
            this.CurrentModule = module;
        }

        public void SetActiveComponent(IComponent component)
        {
            this.ActiveComponent = component;
        }

        public void RegisterComponent(IComponent component)
        {
            this.Components.Add(component);
        }

        public IObjectFactory GetFactoryFor(string objectTypeName)
        {
            foreach (IObjectFactory fac in this.ObjectFactories)
            {
                if (fac.CanHandle(objectTypeName))
                    return fac;
            }
            return null;
        }

        public IOrionDataObject GetDataObject(string dataTypeName, int id)
        {
            IOrionDataObject dataObject = null;

            foreach (IDataFactory fac in this.DataFactories)
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

            foreach (IDataFactory fac in this.DataFactories)
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
            return Vector2.Transform(worldPos, this.ActiveComponent.GetCamera().Transform);
        }

        public Vector2 ScreenToWorld(Vector2 screenPos)
        {
            return Vector2.Transform(screenPos, Matrix.Invert(this.ActiveComponent.GetCamera().Transform));
        }

        public double GetRandomDouble()
        {
            return Randomizer.NextDouble();
        }

        private void SetResolution(int windowSizeX, int windowSizeY, bool isFullscreen)
        {
            this.GraphicsDM.PreferredBackBufferWidth = windowSizeX;
            this.GraphicsDM.PreferredBackBufferHeight = windowSizeY;
            this.GraphicsDM.IsFullScreen = isFullscreen;
            this.GraphicsDM.ApplyChanges();
            ContentManager.Instance.Graphics = this.GraphicsDM.GraphicsDevice;
        }
    }
}
