using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.Factories
{
    public interface IFactoryManager
    {
        IList<IObjectFactory> ObjectFactories { get; }
        IList<ISceneFactory> SceneFactories { get; }
        IList<IDataFactory> DataFactories { get; }

        TFactory GetFactoryFor<TFactory>(string objectTypeName);
    }
}
