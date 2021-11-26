using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLib.Interfaces
{
    public interface IDataHelper
    {
        public List<string> ReadFile(string path);
        public bool WriteFile<T>(string path, List<T> data);
        public List<T> Convert<T>(List<string> data);
        public string CreateID();
    }
}
