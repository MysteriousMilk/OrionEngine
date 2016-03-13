using Microsoft.Xna.Framework;
using System.Xml;
using System.Xml.Linq;

namespace Orion.Core.Factories
{
    public class CoreObjectFactory : IObjectFactory
    {
        public CoreObjectFactory()
        {

        }

        public bool CanHandle(string objectTypeName)
        {
            bool canHandle = false;

            switch (objectTypeName)
            {
                case "Sprite":
                    canHandle = true;
                    break;
                case "AnimatedSprite":
                    canHandle = true;
                    break;
            }

            return canHandle;
        }

        public OrionObject GetObject(string objectTypeName, IFactoryManager manager, Module.Module module, XElement xmlNode)
        {
            OrionObject obj = null;
            SpriteDefinition spriteDef = null;
            string tag = string.Empty;
            string name = string.Empty;
            int id = -1;

            foreach (XAttribute attribNode in xmlNode.Attributes())
            {
                switch (attribNode.Name.LocalName)
                {
                    case "Id":
                        id = XmlConvert.ToInt32(attribNode.Value);
                        break;
                    case "Name":
                        name = attribNode.Value;
                        break;
                    case "Tag":
                        tag = attribNode.Value;
                        break;
                    case "Ref":
                        spriteDef = SpriteDefinition.LoadFromModule(module, attribNode.Value);
                        break;
                }
            }

            obj = GetObject(objectTypeName, manager, spriteDef, xmlNode);
            obj.Id = id;
            obj.Name = name;
            obj.Tag = tag;

            return obj;
        }

        private OrionObject GetObject(string objectTypeName, IFactoryManager manager, SpriteDefinition spriteDef, XElement xmlNode)
        {
            OrionObject obj = null;

            switch (objectTypeName)
            {
                case "Sprite":
                case "AnimatedSprite":
                    obj = LoadSprite(spriteDef, xmlNode);
                    break;
            }

            return obj;
        }

        public Sprite LoadSprite(SpriteDefinition spriteDef, XElement xmlNode)
        {
            float x = 0;
            float y = 0;
            int zorder = 0;
            float rotation = 0;
            float alpha = 255;
            Color tint = Color.White;

            foreach (XElement element in xmlNode.Elements())
            {
                switch (element.Name.LocalName)
                {
                    case "Position":
                        x = XmlConvert.ToSingle(element.Attribute("X").Value);
                        y = XmlConvert.ToSingle(element.Attribute("Y").Value);
                        break;
                    case "ZOrder":
                        zorder = XmlConvert.ToInt32(element.Value);
                        break;
                    case "Tint":
                        tint = LoadTint(element);
                        break;
                    case "Alpha":
                        alpha = XmlConvert.ToSingle(element.Value);
                        break;
                    case "Rotation":
                        rotation = XmlConvert.ToSingle(element.Value);
                        break;
                }
            }

            Sprite sprite = new Sprite(spriteDef);
            sprite.Position = new Vector2(x, y);
            sprite.ZOrder = zorder;
            sprite.Tint = tint;
            sprite.Alpha = alpha;
            sprite.Rotation = rotation;

            return sprite;
        }

        private Color LoadTint(XElement node)
        {
            byte r = 0;
            byte g = 0;
            byte b = 0;

            foreach (XAttribute attrib in node.Attributes())
            {
                switch (attrib.Name.LocalName)
                {
                    case "R":
                        r = XmlConvert.ToByte(attrib.Value);
                        break;
                    case "G":
                        g = XmlConvert.ToByte(attrib.Value);
                        break;
                    case "B":
                        b = XmlConvert.ToByte(attrib.Value);
                        break;
                }
            }

            return new Color(r, g, b);
        }
    }
}
