using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLib.UI
{
    public class MessageBox
    {
        private int paddingTop;
        private int paddingLeft;
        private int width;
        private int height;
        private string title;

        public MessageBox(int width, int height, int top, string title)
        {
            this.width = width;
            this.height = height;
            this.title = title;
            this.paddingTop = top;
        }

        private MessageBox(int paddingLeft, int width, int height)
        {
            this.paddingLeft = paddingLeft;
            this.width = width;
            this.height = height;
        }

        public int PaddingLeft { get => paddingLeft; set => paddingLeft = value; }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public int PaddingTop { get => paddingTop; set => paddingTop = value; }

        public void Draw()
        {
            this.paddingLeft = Console.WindowWidth / 2 - width / 2;
            Console.CursorLeft = Console.WindowWidth / 2 - title.Length / 2;
            Console.CursorTop = this.paddingTop;
            Console.WriteLine(title);
            for (int i = 0; i < height; i++)
            {
                Console.CursorLeft = paddingLeft;
                if (i == 0 || i == height - 1)
                {
                    Console.CursorLeft = paddingLeft + 1;
                    for (int j = 0; j < width - 2; j++)
                        Console.Write("+");
                }
                else
                {
                    Console.Write("|");
                    Console.CursorLeft += (width - 2);
                    Console.Write("|");
                }
                Console.WriteLine();
            }
        }
    }
}
