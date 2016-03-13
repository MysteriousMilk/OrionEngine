using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core
{
    public interface IComponent
    {
        IScene CurrentScene { get; set; }
        ICamera2D GetCamera();
        void AddSceneObject(OrionObject obj);
        void LoadSceneFromModule(Module.Module module, string sceneRef);
    }
}
