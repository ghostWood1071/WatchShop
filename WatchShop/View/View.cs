using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.Interfaces;
namespace WatchShop.View
{
    public class View : IView
    {
        protected Stack<IView> pages;
        public View(Stack<IView> pages)
        {
            this.pages = pages;
        }
        public void Back()
        {
            if (pages.Count == 0)
                return;
            IView view = pages.Pop();
            view.Show();
        }

        public virtual void Show()
        {
            Console.Clear();
            Console.OutputEncoding = Encoding.UTF8;
        }
    }
}
