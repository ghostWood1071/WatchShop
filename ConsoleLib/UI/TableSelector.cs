using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleLib.Interfaces;
namespace ConsoleLib.UI
{
    public class TableSelector: Table, ISelectable
    {
        private List<string> data;

        private string[] columns;

        public TableSelector(int width, List<string> data, params string[] columns) : base(width)
        {
            this.data = ConvertData(data);
            this.columns = columns;
        }

        private List<string> ConvertData(List<string> data)
        {
            List<string> result = new List<string>();
            for(int i = 0; i<data.Count; i++)
            {
                string[] info = data[i].Split('|');
                result.Add(GetRow(info));
            }
            return result;
        }

        public int Select()
        {
            Console.CursorVisible = false;
            int pos = 0;
            int padTop = 3;
            PrintLine();
            PrintRow(columns);
            PrintLine();
            for(int i = 0; i<data.Count; i++)
            {
                if (i == pos)
                    WriteSelectedRow(0, padTop + i, pos);
                else
                    Write(0, padTop + i, i);
            }
            Console.SetCursorPosition(0, data.Count+padTop);
            PrintLine();

            while (true)
            {
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        if (pos < data.Count - 1)
                            pos += 1;
                        if (pos > 0)
                            Write(0, padTop + pos - 1, pos - 1);
                        WriteSelectedRow(0, padTop + pos, pos);
                        Console.CursorTop = padTop + data.Count;
                        break;
                    case ConsoleKey.UpArrow:
                        if (pos > 0)
                            pos -= 1;
                        if (pos < data.Count - 1)
                            Write(0, padTop + pos + 1, pos + 1);
                        WriteSelectedRow(0, padTop + pos, pos);
                        Console.CursorTop = padTop + data.Count;
                        break;
                    case ConsoleKey.Enter:
                        Console.CursorVisible = true;
                        return pos;
                    case ConsoleKey.Escape:
                        Console.CursorVisible = true;
                        return -1;
                }
            }
        }

        public void Write(int x, int y, int pos)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(data[pos]);
        }

        public void WriteMessage(int x, int pos)
        {
            
        }

        public void WriteSelectedRow(int x, int y, int pos)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Blue;
            Write(x, y, pos);
            Console.ResetColor();
            Console.SetCursorPosition(x, data.Count + 1 + y);
            WriteMessage(x, pos);
        }

        public void UpdateData(List<string> data)
        {
            this.data = ConvertData(data);
        }

        
    }
}
