using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.Interfaces;
using ConsoleLib.UI;
namespace WatchShop.View
{
    class Homeview : View, IMainMenu
    {
        private IView[] views;
        
        public Homeview(IView[] views, Stack<IView> pages):base(pages)
        {
            this.views = views;
        }

        public void Goto(IView view)
        {
            Console.Clear();
            this.pages.Push(this);
            view.Show();
        }

        public override void Show()
        {
            base.Show();
            MenuSelector selector = new MenuSelector(new string[]{
                "1.Quản lý nhãn hàng",
                "2.Quản lý đồng hồ",
                "3.Quản lý nhập hàng",
                "4.Quản lý bán hàng",
                "5.Thống kê"
            }, "Chương trình quản lý cửa hàng đồng hồ");
            int page = selector.Select();
            Goto(views[page]);
        }
    }
}
