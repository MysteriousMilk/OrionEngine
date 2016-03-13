using System;

namespace Orion.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class ShowInEditor : Attribute
    {
        public bool Show { get; set; }

        public ShowInEditor(bool show)
        {
            this.Show = show;
        }
    }
}
