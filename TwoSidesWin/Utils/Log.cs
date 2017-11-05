using System;
using System.Globalization;
using System.IO;

namespace TwoSides.Utils
{
    public class Log
    {
        public bool HasData;
        readonly string _path;
        
        public Log(string path) {
            _path = path;
            HasData = true;
        }
        
        public void DeleteLog()
        {
            File.Delete(_path + ".log");
        }
        
        public void WriteLog(string info)
        {
            StreamWriter file = File.AppendText($"{_path}.log");
            if (HasData) info = $"{DateTime.Now.ToString("[yyyy:dd:hh:mm:ss]" , CultureInfo.CurrentCulture)} {info}";
            file.WriteLine(info);
            file.Close();
        }
    }
}
