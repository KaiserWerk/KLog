using System;
using System.Diagnostics;

namespace KLog.Writer
{
    public class ConsoleWriter : LogWriter
    {
        public void Write(string s)
        {
            Console.WriteLine(s);
            Debug.WriteLine(s);
        }
    }
}
