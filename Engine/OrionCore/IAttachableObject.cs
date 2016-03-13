using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core
{
    public interface IAttachableObject
    {
        int Id { get; set; }
        string Name { get; set; }
        Type AttachableType { get; }
        IEnumerable<Type> Interfaces { get; }

        object Clone();
    }
}
