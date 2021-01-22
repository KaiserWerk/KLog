using KLog.Interface;
using System.Collections.Generic;

namespace KLog
{
    public class KLogOptions
    {
        public List<ILogWriter> Writer = new List<ILogWriter>();
        public Level MinLevel = Level.Error;
    }
}
