using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.Managers
{
    public sealed class LogManager
    {
        #region Instance
        private static LogManager _instance = null;
        public static LogManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LogManager();
                return _instance;
            }
        }
        #endregion

        private List<string> _log;

        private LogManager()
        {
            _log = new List<string>();
        }

        public void LogMessage(string msg)
        {
            _log.Add(msg);
        }

        public void LogError(string err)
        {
            _log.Add("[Error] " + err);
        }

        public void LogException(Exception e)
        {
            _log.Add("[" + e.GetType().Name + "] " + e.Message);
        }
    }
}
