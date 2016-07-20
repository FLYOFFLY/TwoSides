using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TwoSides.Utils
{
    public class Log
    {
        public bool hasData;
        string path;
        
        public Log(string path) {
            this.path = path;
            hasData = true;
        }
        
        public void deleteLog()
        {
            File.Delete(path + ".log");
        }
        
        public void WriteLog(string info)
        {
            StreamWriter file = File.AppendText(path + ".log");
            if (hasData) info = DateTime.Now.ToString("[yyyy:dd:hh:mm:ss]")+" "+info;
            file.WriteLine(info);
            file.Close();
        }
    }
}
