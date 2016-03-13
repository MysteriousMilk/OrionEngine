using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Orion.Core
{
    public class SpriteDefinition
    {
        #region Fields
        private static SpriteDefinition _emptyDef = new SpriteDefinition();
        #endregion

        #region Properties
        public string ReferenceName { get; set; }
        public string Path { get; set; }
        public string Tag { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }
        public int FrameCount { get; set; }

        public Dictionary<string, Animation> AnimationList { get; set; }
        #endregion

        public static SpriteDefinition Empty
        {
            get { return _emptyDef; }
        }

        public SpriteDefinition()
        {
            this.Rows = 0;
            this.Columns = 0;
            this.FrameWidth = 0;
            this.FrameHeight = 0;
            this.FrameCount = 0;
            this.AnimationList = new Dictionary<string, Animation>();
        }

        public SpriteDefinition(string refName, string path, int rows, int columns, int width, int height)
        {
            this.ReferenceName = refName;
            this.Path = path;
            this.Rows = rows;
            this.Columns = columns;
            this.FrameWidth = width;
            this.FrameHeight = FrameHeight;
            this.FrameCount = rows * columns;
            this.AnimationList = new Dictionary<string, Animation>();
        }

        public SpriteDefinition Clone()
        {
            SpriteDefinition clone = this.MemberwiseClone() as SpriteDefinition;

            clone.AnimationList = new Dictionary<string, Animation>();
            foreach (KeyValuePair<string, Animation> pair in this.AnimationList)
                clone.AnimationList.Add(pair.Key, pair.Value.Clone());

            return clone;
        }

        public static SpriteDefinition LoadFromModule(Orion.Core.Module.Module module, string reference)
        {
            SpriteDefinition spriteDef = null;

            string xml = module.GetFileXML(reference, ResourceType.SpriteDef);

            spriteDef = new SpriteDefinition();

            XDocument doc = XDocument.Parse(xml);
            spriteDef.Tag = doc.Element("SpriteDefinition").Attribute("Tag").Value;

            foreach (XElement node in doc.Element("SpriteDefinition").Elements())
            {
                switch (node.Name.LocalName)
                {
                    case "Resource":
                        {
                            spriteDef.ReferenceName = node.Attribute("Ref").Value;
                            spriteDef.Path = module.GetResourcePath(spriteDef.ReferenceName, ResourceType.Texture);

                            module.LoadTexture(spriteDef.ReferenceName);
                        }
                        break;
                    case "FrameData":
                        LoadFrameData(node, spriteDef);
                        break;
                    case "AnimationList":
                        foreach (XElement listNode in node.Elements())
                            if (listNode.Name.LocalName == "Animation")
                                LoadAnimation(listNode, spriteDef);
                        break;
                }
            }

            return spriteDef;
        }

        private static void LoadFrameData(XElement node, SpriteDefinition spriteDef)
        {
            foreach (XElement frameNode in node.Elements())
            {
                switch (frameNode.Name.LocalName)
                {
                    case "Rows":
                        spriteDef.Rows = XmlConvert.ToInt32(frameNode.Value);
                        break;
                    case "Columns":
                        spriteDef.Columns = XmlConvert.ToInt32(frameNode.Value);
                        break;
                    case "Width":
                        spriteDef.FrameWidth = XmlConvert.ToInt32(frameNode.Value);
                        break;
                    case "Height":
                        spriteDef.FrameHeight = XmlConvert.ToInt32(frameNode.Value);
                        break;
                }
            }

            spriteDef.FrameCount = spriteDef.FrameWidth * spriteDef.FrameHeight;
        }

        private static void LoadAnimation(XElement node, SpriteDefinition spriteDef)
        {
            string tag = string.Empty;
            int start = 0;
            int end = 0;
            int timePerFrame = 0;
            bool loop = false;

            foreach (XElement animationNode in node.Elements())
            {
                switch (animationNode.Name.LocalName)
                {
                    case "Tag":
                        tag = animationNode.Value;
                        break;
                    case "StartFrame":
                        start = XmlConvert.ToInt32(animationNode.Value);
                        break;
                    case "EndFrame":
                        end = XmlConvert.ToInt32(animationNode.Value);
                        break;
                    case "TimePerFrame":
                        timePerFrame = XmlConvert.ToInt32(animationNode.Value);
                        break;
                    case "Loop":
                        loop = XmlConvert.ToBoolean(animationNode.Value.ToLower());
                        break;
                }
            }

            spriteDef.AnimationList.Add(tag, new Animation(tag, start, end, timePerFrame, loop));
        }
    }
}
