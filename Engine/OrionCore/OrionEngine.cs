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

        /// <summary>
        /// Version number of the engine.
        /// </summary>
        public const string Version = "0.9.0.0";

        public static OrionEngine Instance
        {
            get
            {
                if (!_instance.IsInitialized)
                    throw new Exception("Engine must be initialized before use.");
                return _instance;
            }
        }

        public static void Initialize(Game game, GraphicsDeviceManager graphicsDM)
        {
            _instance.GraphicsDM = graphicsDM;
            _instance._Game = game;

            ContentManager.Instance = new ContentManager(game.Content);
            ContentManager.Instance.Graphics = game.GraphicsDevice;

            _instance.SetResolution(Settings.Instance.ResolutionX, Settings.Instance.ResolutionY, Settings.Instance.IsFullscreen);

            _instance.IsInitialized = true;
            _instance.RegisterComponent(new StandardGameComponent(game));
        }
        #endregion

        #region Construction
        private OrionEngine()
        {
            IsInitialized = false;
            GraphicsDM = null;

            ObjectFactories = new List<IObjectFactory>();
            ObjectFactories.Add(new CoreObjectFactory());

            DataFactories = new List<IDataFactory>();

            SceneFactories = new List<ISceneFactory>();
            SceneFactories.Add(new CoreSceneFactory());
        }
        #endregion

        #region Fields
        internal Game _Game = null;
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

        public IScene CurrentScene
        {
            get
            {
                StandardGameComponent component = GetComponent<StandardGameComponent>();
                if (component != null)
                    return component.CurrentScene;

                return null;
            }
            set
            {

                StandardGameComponent component = GetComponent<StandardGameComponent>();
                if (component != null && !ReferenceEquals(value, component))
                    component.CurrentScene = value;
            }
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

        public IList<ISceneFactory> SceneFactories
        {
            get;
            set;
        }
        #endregion

        public void SetCurrentModule(Module.Module module)
        {
            CurrentModule = module;
        }

        public void RegisterComponent(IGameComponent component)
        {
            foreach (IGameComponent compToCheck in _Game.Components)
            {
                if (ReferenceEquals(compToCheck, component))
                    return;
            }

            _Game.Components.Add(component);
        }

        public TComponent GetComponent<TComponent>()
        {
            Type type = typeof(TComponent);

            foreach (IGameComponent component in _Game.Components)
            {
                if (component.GetType().Equals(type))
                    return (TComponent)component;
            }

            return default(TComponent);
        }

        public TFactory GetFactoryFor<TFactory>(string objectTypeName)
        {
            Type facType = typeof(TFactory);

            if (facType.Equals(typeof(IObjectFactory)))
            {
                foreach (IObjectFactory fac in ObjectFactories)
                {
                    if (fac.CanHandle(objectTypeName))
                        return (TFactory)fac;
                }
            }
            else if (facType.Equals(typeof(ISceneFactory)))
            {
                foreach (ISceneFactory fac in ObjectFactories)
                {
                    if (fac.CanHandle(objectTypeName))
                        return (TFactory)fac;
                }
            }

            return default(TFactory);
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
            Camera2D camera = GetComponent<Camera2D>();

            if (camera == null)
                throw new InvalidOperationException("Operation cannot be performed without a registerd camera.");

            return Vector2.Transform(worldPos, camera.Transform);
        }

        public Vector2 ScreenToWorld(Vector2 screenPos)
        {
            Camera2D camera = GetComponent<Camera2D>();

            if (camera == null)
                throw new InvalidOperationException("Operation cannot be performed without a registerd camera.");

            return Vector2.Transform(screenPos, Matrix.Invert(camera.Transform));
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
