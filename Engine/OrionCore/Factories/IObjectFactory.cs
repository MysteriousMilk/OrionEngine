using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Orion.Core.Factories
{
    public interface IObjectFactory
    {
        bool CanHandle(string objectTypeName);
        OrionObject GetObject(string objectTypeName, IFactoryManager manager, Module.Module module, XElement xmlNode);
        //OrionObject GetObject(string objectTypeName, IFactoryManager manager, SpriteDefinition spriteDef, XElement xmlNode);
    }
}
