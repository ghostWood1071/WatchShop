using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleLib.Interfaces;
namespace ConsoleLib.UI
{
    public class MenuSelector: ISelectable
    {
        private string[] ultilities;
        private string title;
        public MenuSelector(string[] ultilities, string title)
        {
            this.ultilities = ultilities;
            this.title = title;
        }

        public int Select()
        {
            int padLeft = (Console.WindowWidth - title.Length) / 2;
            int padTop = 10;
            Console.SetCursorPosition(padLeft, padTop);
            Console.Write(title);
            padTop += 1;
            int selectPos = 0;
            for (int i = 0; i < ultilities.Length; i++)
            {
                if (i == selectPos)
                    WriteSelectedRow(padLeft, padTop + i, i);
                else
                    Write(padLeft, padTop + i, i);
            }
            while (true)
            {
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        if (selectPos < ultilities.Length - 1)
                            selectPos += 1;
                        if (selectPos > 0)
                            Write(padLeft, padTop + selectPos - 1, selectPos - 1);
                        WriteSelectedRow(padLeft, padTop + selectPos, selectPos);
                        break;
                    case ConsoleKey.UpArrow:
                        if (selectPos > 0)
                            selectPos -= 1;
                        if (selectPos < ultilities.Length - 1)
                            Write(padLeft, padTop + selectPos + 1, selectPos + 1);
                        WriteSelectedRow(padLeft, padTop + selectPos, selectPos);
                        break;
                    case ConsoleKey.Enter:
                        return selectPos;

                }
            }

        }

        public void Write(int x, int y, int pos)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(ultilities[pos]);
        }

        public void WriteSelectedRow(int x, int y, int pos)
        {

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Blue;
            Write(x, y, pos);
            Console.ResetColor();
            Console.SetCursorPosition(x, ultilities.Length + 1 + y);
            WriteMessage(x, pos);
        }

        public void WriteMessage(int x, int pos)
        {
            Console.SetCursorPosition(x, 11 + ultilities.Length);
            Console.Write("Bạn đang chọn: " + (pos + 1) + "    ");
        }

        
    }
}
