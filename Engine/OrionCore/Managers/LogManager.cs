using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
        private TextWriter _writer;

        private LogManager()
        {
            _log = new List<string>();
        }

        public void LogMessage(object sender, object source, string msg)
        {
            if (sender == null)
                throw new ArgumentNullException("sender");

            StringBuilder sb = new StringBuilder();
            sb.Append("[" + sender.GetType().Name + "] ");
            if (source != null)
                sb.Append("<" + source.GetType().Name + "> ");
            sb.Append(msg);

            _log.Add(sb.ToString());

            if (_writer != null)
                _writer.WriteLine(sb.ToString());
        }

        public void LogError(string err)
        {
            string msg = "[Error] " + err;
            _log.Add(msg);

            if (_writer != null)
                _writer.WriteLine(msg);
        }

        public void LogException(Exception e)
        {
            string msg = "[" + e.GetType().Name + "] " + e.Message;
            _log.Add(msg);

            if (_writer != null)
                _writer.WriteLine(msg);
        }

        public void SetOutputStream(TextWriter writer)
        {
            _writer = writer;
        }
    }
}
