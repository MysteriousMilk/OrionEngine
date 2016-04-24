using System.Xml.Linq;

namespace Orion.Core.Factories
{
    public interface ISceneFactory
    {
        bool CanHandle(string sceneTypeName);
        IScene GetScene(string objectTypeName, IFactoryManager manager, Module.Module module, XElement xmlNode);
    }
}
