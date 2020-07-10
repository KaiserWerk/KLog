using System;
using System.Collections.Generic;
using KLog.Writer;

namespace KLog
{
    public interface LogWriter
    {
        void Write(string s);
    }

    public class Logger
    {
        public enum Level
        {
            Debug,
            Info,
            Warn,
            Error
        }

        public Logger()
        {
            this.SetWriter(new ConsoleWriter());
        }

        private List<LogWriter> logWriters = new List<LogWriter>();

        public void AddWriter(LogWriter w)
        {
            this.logWriters.Add(w);
        }

        public void SetWriter(LogWriter w)
        {
            this.logWriters.Clear();
            this.logWriters.Add(w);
        }

        public void Debug(string s)
        {
            this.writeLogMessage(Level.Debug, s);
        }

        public void Debug(Exception e)
        {
            this.writeLogMessage(Level.Debug, e.Message);
        }

        public void Info(string s)
        {
            this.writeLogMessage(Level.Info, s);
        }

        public void Info(Exception e)
        {
            this.writeLogMessage(Level.Info, e.Message);
        }

        public void Warn(string s)
        {
            this.writeLogMessage(Level.Warn, s);
        }

        public void Warn(Exception e)
        {
            this.writeLogMessage(Level.Warn, e.Message);
        }

        public void Error(string s)
        {
            this.writeLogMessage(Level.Error, s);
        }

        public void Error(Exception e)
        {
            this.writeLogMessage(Level.Error, e.Message);
        }

        private void writeLogMessage(Level l, string msg)
        {
            var now = DateTime.Now;
            string line = $"date=\"{now.ToLongDateString()}\" time=\"{now.ToLongTimeString()}\" " +
                          $"level=\"{l.ToString()}\" message=\"{msg}\"";

            if (this.logWriters.Count > 0)
            {
                foreach (var writer in this.logWriters)
                {
                    writer.Write(line);
                }
            }
        }
    }
}
