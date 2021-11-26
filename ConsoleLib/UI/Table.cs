using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLib.UI
{
    public class Table
    {
        protected int tableWidth;
        
        public Table(int width)
        {
            this.tableWidth = width;
        }

        public void PrintLine()
        {
            
            Console.WriteLine(new string('-', tableWidth));
        }

        public void PrintRow(params string[] columns)
        {

            string row = GetRow(columns);
            Console.WriteLine(row);
        }

        public string AlignCentre(string text, int width)
        {
           text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }

        protected string GetRow(string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }
            return row;
        }

        
    }
}
