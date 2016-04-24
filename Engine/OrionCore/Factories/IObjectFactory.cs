using System.Xml.Linq;

namespace Orion.Core.Factories
{
    public interface IObjectFactory
    {
        bool CanHandle(string objectTypeName);
        GameObject GetObject(string objectTypeName, IFactoryManager manager, Module.Module module, XElement xmlNode);
    }
}
