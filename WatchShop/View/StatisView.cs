using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.Entity;
using WatchShop.Interfaces;
using ConsoleLib.UI;
namespace WatchShop.View
{
    public class StatisView:View
    {
        private IStatisticBuss statistic;
        public StatisView(IStatisticBuss statisticBuss, Stack<IView> views) : base(views)
        {
            this.statistic = statisticBuss;
        }

        public override void Show()
        {
            base.Show();
            MenuSelector selector = new MenuSelector(new string[] {
                "Thống kê",
                "Thoát"
            }, "Thống kê doanh thu");
            switch (selector.Select())
            {
                case 0: ShowAll(); break;
                default: Back(); break;
            }
        }
        private void ShowAll()
        {
            while (true)
            {
                Console.Clear();
                int year = TypeNumber("Năm");
                List<StatisResult> results = statistic.Statis(year);
                Print(results);
                Console.WriteLine("Bạn có muốn xem tiếp không?(nhấn esc để thoát)");
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    Show();
                    break;
                }
            }
        }
        private int TypeNumber(string header, bool isUpdate = false)
        {
            while (true)
            {
                try
                {
                    Console.Write(header + ": ");
                    int price = int.Parse(Console.ReadLine());
                    if (price >= 0)
                        return price;
                    else
                        Console.WriteLine($"{header} >=0");
                }
                catch
                {
                    if (isUpdate)
                        return -1;
                    Console.WriteLine($"{header} không được để trống");
                }
            }
        }

        private void Print(List<StatisResult> orders)
        {
            Console.Clear();
            Table table = new Table(100);
            table.PrintLine();
            table.PrintRow("Tháng", "Thu", "Chi", "Lợi nhuận");
            table.PrintLine();
            foreach (var sub in orders)
            {
                table.PrintRow(sub.Month.ToString(), sub.Sell.ToString(), sub.Spend.ToString(), sub.Profit.ToString());
            }
            table.PrintLine();
        }
    }
}
