using KLog.Interface;
using System.IO;

namespace KLog.Writer
{
    public class FileWriter : ILogWriter
    {
        private string file;

        public FileWriter(string filename)
        {
            this.file = filename;
            string path = Path.GetDirectoryName(this.file);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
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
