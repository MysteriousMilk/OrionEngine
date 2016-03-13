using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Orion.Core
{
    public sealed class Settings
    {
        private static Settings _instance = null;
        private Settings()
        {
            Load();
        }

        public static Settings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Settings();
                return _instance;
            }
        }

        public int ResolutionX { get; set; }
        public int ResolutionY { get; set; }
        public int MaxParticlesPerSystem { get; set; }
        public bool IsFullscreen { get; set; }

        public void Load()
        {
            XDocument document = XDocument.Load(@"Data\settings.xml");

            foreach(XElement element in document.Element("Settings").Elements())
            {
                switch(element.Name.LocalName)
                {
                    case "ResolutionX":
                        this.ResolutionX = XmlConvert.ToInt32(element.Value);
                        break;
                    case "ResolutionY":
                        this.ResolutionY = XmlConvert.ToInt32(element.Value);
                        break;
                    case "IsFullscreen":
                        this.IsFullscreen = XmlConvert.ToBoolean(element.Value.ToLower());
                        break;
                    case "MaxParticlesPerSystem":
                        this.MaxParticlesPerSystem = XmlConvert.ToInt32(element.Value);
                        break;
                }
            }
        }
    }
}
