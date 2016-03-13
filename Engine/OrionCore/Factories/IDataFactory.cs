using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.Factories
{
    public interface IDataFactory
    {
        bool CanHandle(string dataTypeName);
        IOrionDataObject GetDataObject(string dataTypeName, int id);
        IOrionDataObject GetDataObject(string dataTypeName, string tag);
        void RelinkDatabase(string databasePath);
    }
}
