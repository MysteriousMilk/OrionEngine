using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Orion.Core.Module
{
    public interface IStartInfo
    {
        string StartScene { get; set; }

        void LoadFromXML(XElement node);
    }
}
