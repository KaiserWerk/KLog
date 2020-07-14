using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KLog.Writer
{
    public class FileWriter : LogWriter
    {
        private string file;

        public FileWriter(string filename)
        {
            this.file = filename;
        }

        public void Write(string s)
        {
            try
            {
                string[] l = {s};
                File.AppendAllLines(this.file, l);
            } catch {}
        }
    }
}
