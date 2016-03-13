using NLua;
using Orion.Core;
using Orion.Core.Managers;
using System;

namespace OrionLuaComponentWin32
{
    public class LuaComponent
    {
        private Lua _lua;

        private LuaComponent()
        {
            _lua = new Lua();
        }

        public void ExecuteScript(string scriptRef)
        {
            try
            {
                string script = OrionEngine.Instance.CurrentModule.GetFileXML(scriptRef, ResourceType.Script);
                _lua.DoString(script);
            }
            catch(Exception)
            {
                LogManager.Instance.LogError("Script [" + scriptRef + "] could not be executed.");
            }
        }
    }
}
