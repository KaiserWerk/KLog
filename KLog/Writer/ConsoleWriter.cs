using System;
using System.Diagnostics;
using KLog.Interface;

namespace KLog.Writer
{
    public class ConsoleWriter : ILogWriter
    {
        public void Write(string s)
        {
            Console.WriteLine(s);
            Debug.WriteLine(s);
        }
    }
}
