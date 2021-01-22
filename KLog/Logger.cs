using KLog.Interface;
using KLog.Writer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace KLog
{
    public class Logger
    {
        private Level minLevel = Level.Debug;
        private Mutex mutex = new Mutex();
        private Timer timer;
        private bool commitLogEnabled;
        private ConcurrentStack<string> commitLog = new ConcurrentStack<string>();
        private List<ILogWriter> logWriters = new List<ILogWriter>();

        public Logger()
        { }

        public Logger(KLogOptions options)
        {
            this.logWriters = options.Writer ?? new List<ILogWriter>();
            this.minLevel = options.MinLevel;
        }

        public void SetMinLevel(Level lvl)
        {
            this.minLevel = lvl;
        }

        private void IntervalledWrite(object sender, ElapsedEventArgs e)
        {
            if (!this.commitLogEnabled)
                return;
            
            this.mutex.WaitOne();
            foreach (string item in this.commitLog)
            {
                if (this.logWriters.Count > 0)
                {
                    foreach (var writer in this.logWriters)
                    {
                        writer.Write(item);
                    }
                }
            }
            this.commitLog.Clear();
            this.mutex.ReleaseMutex();
        }

        public void UseCommitLog(bool enabled)
        {
            this.commitLogEnabled = enabled;
            if (enabled)
            {
                this.timer = new Timer();
                this.timer.Interval = 20000;
                this.timer.Elapsed += this.IntervalledWrite;
                this.timer.Enabled = true;
            }
            else
            {
                this.timer = null;
            }
        }

        public bool IsCommitLogEnabled()
        {
            return this.commitLogEnabled;
        }

        public void AddWriter(ILogWriter w)
        {
            this.logWriters.Add(w);
        }

        public void SetWriter(ILogWriter w)
        {
            this.logWriters.Clear();
            this.logWriters.Add(w);
        }

        public void Debug(string s)
        {
            this.WriteLogMessage(Level.Debug, s);
        }

        public void Debug(Exception e)
        {
            this.WriteLogMessage(Level.Debug, e.Message);
        }

        public void Info(string s)
        {
            this.WriteLogMessage(Level.Info, s);
        }

        public void Info(Exception e)
        {
            this.WriteLogMessage(Level.Info, e.Message);
        }

        public void Warn(string s)
        {
            this.WriteLogMessage(Level.Warn, s);
        }

        public void Warn(Exception e)
        {
            this.WriteLogMessage(Level.Warn, e.Message);
        }

        public void Error(string s)
        {
            this.WriteLogMessage(Level.Error, s);
        }

        public void Error(Exception e)
        {
            this.WriteLogMessage(Level.Error, e.Message);
        }

        public void Metric(string ctx, int ms)
        {

        }

        private void WriteLogMessage(Level l, string msg)
        {
            var now = DateTime.Now;
            
            string line = $"date=\"{now.ToLongDateString()}\" time=\"{now.ToLongTimeString()}\" " +
                          $"level=\"{l.ToString()}\" message=\"{msg}\"";

            if (l >= this.minLevel)
            {
                if (!this.commitLogEnabled)
                {

                    if (this.logWriters.Count > 0)
                    {
                        foreach (var writer in this.logWriters)
                        {
                            writer.Write(line);
                        }
                    }
                }
                else
                {
                    this.commitLog.Push(line);
                }
            }
        }
    }
}
