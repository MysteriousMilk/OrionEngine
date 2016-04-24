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

            _lua.RegisterFunction("GetModuleVariable", GetType().GetMethod("GetModuleVariable"));
            _lua.RegisterFunction("GetSceneVariable", GetType().GetMethod("GetSceneVariable"));
        }

        public void BuildVariableTable()
        {
            if (OrionEngine.Instance.CurrentModule != null)
            {
                foreach (GameVariable variable in OrionEngine.Instance.CurrentModule.Variables)
                    _lua[variable.Name] = variable.Value;
            }

            if (OrionEngine.Instance.CurrentModule != null)
            {
                foreach (GameVariable variable in OrionEngine.Instance.CurrentScene.Variables)
                    _lua[variable.Name] = variable.Value;
            }
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

        public object GetModuleVariable(string varName)
        {
            if (OrionEngine.Instance.CurrentModule != null)
            {
                foreach (GameVariable variable in OrionEngine.Instance.CurrentModule.Variables)
                {
                    if (variable.Name.Equals(varName))
                        return variable.Value;
                }
            }

            return 0;
        }

        public object GetSceneVariable(string varName)
        {
            if (OrionEngine.Instance.CurrentScene != null)
            {
                foreach (GameVariable variable in OrionEngine.Instance.CurrentScene.Variables)
                {
                    if (variable.Name.Equals(varName))
                        return variable.Value;
                }
            }

            return 0;
        }
    }
}
