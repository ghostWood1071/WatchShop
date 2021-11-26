using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLib.Interfaces 
{ 
    public interface ISelectable
    {

        int Select();
        void Write(int x, int y, int pos);
        void WriteSelectedRow(int x, int y, int pos);
        void WriteMessage(int x, int pos);
    }
}
