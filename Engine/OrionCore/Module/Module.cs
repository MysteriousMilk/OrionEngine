// --------------------------------------------------------------
// This source code file is part of the Orion Engine.
// Developed by Michael Kyle - 2016
// --------------------------------------------------------------

using Orion.Core.Managers;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;

namespace Orion.Core.Module
{
    public enum ModuleAccessMode
    {
        Streaming,
        Disk
    }
    
    public class Module
    {
        private Dictionary<ResourceType, Dictionary<string, string>> _resourceMap;
        private List<GameVariable> _variables = new List<GameVariable>();

        public IPlatformModuleLoader Loader { get; set; }
        public IStartInfo StartInfo { get; set; }
        public string DatabasePath { get; set; }

        public IEnumerable<GameVariable> Variables
        {
            get
            {
                return _variables;
            }
        }

        public Module(IPlatformModuleLoader loader, IStartInfo startInfo)
        {
            SetupResourceMap();

            StartInfo = startInfo;
            Loader = loader;

            Loader.Prepare();
        }

        public Module(string modulePath, IPlatformModuleLoader loader, IStartInfo startInfo)
        {
            SetupResourceMap();

            StartInfo = startInfo;
            Loader = loader;

            Loader.Prepare(modulePath);

            LoadResFile();
            LoadModuleHeader();
            SetupDatabase();
        }

        public void RegisterVariable(GameVariable variable)
        {
            _variables.Add(variable);
        }

        private void SetupResourceMap()
        {
            _resourceMap = new Dictionary<ResourceType, Dictionary<string, string>>();
            _resourceMap.Add(ResourceType.Database, new Dictionary<string, string>());
            _resourceMap.Add(ResourceType.ParticleFx, new Dictionary<string, string>());
            _resourceMap.Add(ResourceType.Scene, new Dictionary<string, string>());
            _resourceMap.Add(ResourceType.Script, new Dictionary<string, string>());
            _resourceMap.Add(ResourceType.SpriteDef, new Dictionary<string, string>());
            _resourceMap.Add(ResourceType.Texture, new Dictionary<string, string>());
        }

        #region General Methods
        public Resource GetResourceByRef(string reference, ResourceType type)
        {
            string fileName = string.Empty;
            _resourceMap[type].TryGetValue(reference, out fileName);

            if (fileName != null)
                return new Resource(reference, fileName);

            return null;
        }

        public IList<Resource> GetResources(ResourceType type)
        {
            List<Resource> resList = new List<Resource>();

            foreach (var pair in _resourceMap[type])
                resList.Add(new Resource(pair.Key, pair.Value));

            return resList;
        }

        public Dictionary<string, string> GetResourceMapByType(ResourceType type)
        {
            return _resourceMap[type];
        }

        public string GetFileXML(string reference, ResourceType type)
        {
            string fileName = string.Empty;
            string xml = string.Empty;

            if (_resourceMap[type].TryGetValue(reference, out fileName))
            {
                xml = Loader.GetEntryAsString(fileName);
            }

            return xml;
        }

        public void LoadTexture(string reference)
        {
            string textureFileName = string.Empty;

            if (_resourceMap[ResourceType.Texture].TryGetValue(reference, out textureFileName))
            {
                byte[] texData = Loader.GetEntryAsBytes(textureFileName);
                using (var memStream = new MemoryStream(texData))
                    ContentManager.Instance.Load(memStream, reference, ContentType.Texture);
            }
        }

        public string GetResourcePath(string reference, ResourceType type)
        {
            string fileName = string.Empty;

            if (_resourceMap[ResourceType.Texture].TryGetValue(reference, out fileName))
                return fileName;
            else
                return string.Empty;
        }
        #endregion

        #region Resource File
        private void LoadResFile()
        {
            string xml = Loader.GetEntryAsString("ModuleResources.res");

            XDocument doc = XDocument.Parse(xml);
            foreach(XElement resTypeElement in doc.Element("ResourceMap").Elements())
            {
                switch(resTypeElement.Name.LocalName)
                {
                    case "Database":
                        LoadResourceNode(resTypeElement, ResourceType.Database);
                        break;
                    case "ParticleFXs":
                        LoadResourceNode(resTypeElement, ResourceType.ParticleFx);
                        break;
                    case "Scenes":
                        LoadResourceNode(resTypeElement, ResourceType.Scene);
                        break;
                    case "Scripts":
                        LoadResourceNode(resTypeElement, ResourceType.Script);
                        break;
                    case "SpriteDefinitions":
                        LoadResourceNode(resTypeElement, ResourceType.SpriteDef);
                        break;
                    case "Textures":
                        LoadResourceNode(resTypeElement, ResourceType.Texture);
                        break;
                }
            }
        }

        private void LoadResourceNode(XElement node, ResourceType nodeType)
        {
            foreach(XElement resNode in node.Elements())
            {
                if(resNode.Name.LocalName == "Resource")
                {
                    string resRef = resNode.Attribute("Ref").Value;
                    _resourceMap[nodeType].Add(resRef, resNode.Value);
                }
            }
        }
        #endregion

        #region Module.xml
        private void LoadModuleHeader()
        {
            string xml = Loader.GetEntryAsString("module.xml");

            XDocument doc = XDocument.Parse(xml);
            foreach(XElement node in doc.Element("Module").Elements())
            {
                switch(node.Name.LocalName)
                {
                    case "StartInfo":
                        this.StartInfo.LoadFromXML(node);
                        break;
                }
            }
        }
        #endregion

        #region Database
        private void SetupDatabase()
        {
            if (Loader.AccessMode == ModuleAccessMode.Streaming)
            {
                string dbFileName = string.Empty;

                if (_resourceMap[ResourceType.Database].TryGetValue("oriondb", out dbFileName))
                {
                    byte[] databaseData = Loader.GetEntryAsBytes(dbFileName);
                    DatabasePath = Loader.CopyDatabaseToStagingDir(dbFileName, databaseData);
                }
            }
        }
        #endregion
    }
}
