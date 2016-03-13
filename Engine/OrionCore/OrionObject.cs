using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core
{
    public abstract class OrionObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }

        public OrionObject()
        {
            this.Id = -1;
            this.Name = string.Empty;
            this.Tag = string.Empty;
        }

        public virtual object Clone()
        {
            OrionObject clone = this.MemberwiseClone() as OrionObject;
            clone.Id = -1;
            return clone;
        }
    }
}
