using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core
{
    public class ComponentNotRegisteredException : Exception
    {
        public string Type { get; set; }

        public ComponentNotRegisteredException(string type, string message)
            : base(message)
        {
            this.Type = type;
        }
    }

    public class LoadFromModuleException : Exception
    {
        public Type FailedLoadType { get; set; }

        public LoadFromModuleException(Type type, string message)
            : base(message)
        {
            this.FailedLoadType = type;
        }
    }
}
