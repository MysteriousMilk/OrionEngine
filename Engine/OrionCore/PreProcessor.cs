using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core
{
    public class PreProcessor
    {
        private List<IPreProcessorItem> _PreProcessorList = new List<IPreProcessorItem>();

        public PreProcessor()
        {

        }

        public void AddItem(IPreProcessorItem item)
        {
            if (!_PreProcessorList.Contains(item))
                _PreProcessorList.Add(item);
        }

        public void RemoveItem(IPreProcessorItem item)
        {
            _PreProcessorList.Remove(item);
        }

        public void Apply()
        {
            foreach (IPreProcessorItem item in _PreProcessorList)
                item.PreProcess();
        }
    }
}
