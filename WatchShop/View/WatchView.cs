using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleLib.UI;
using ConsoleLib.Interfaces;
using WatchShop.Interfaces;
using WatchShop.Entity;
using System.Threading;

namespace WatchShop.View
{
    public class WatchView: View
    {
        private IWatchBuss watchBuss;
        private IBrandBuss brandBuss;
        public WatchView(IWatchBuss watchBuss, IBrandBuss brandBuss ,Stack<IView> pages): base(pages)
        {
            this.watchBuss = watchBuss;
            this.brandBuss = brandBuss;
        }

        public override void Show()
        {
            base.Show();
            MenuSelector menu = new MenuSelector(new string[] { 
                "1.Hiển thị toàn bộ mặt hàng",
                "2.Thêm mặt hàng mới",
                "3.Sửa thông tin mặt hàng",
                "4.Xóa mặt hàng",
                "5.Tìm kiếm",
                "6.Thoát"
            }, "Quản lý đồng hồ");
            switch (menu.Select())
            {
                case 0: ShowAll(); break;
                case 1: Add(); break;
                case 2: Update(); break;
                case 3: Delete(); break;
                case 4: Search(); break;
                case 5: Back(); break;
            }
        }

        private void Search()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("Từ khóa tìm kiếm: ");
                string keyword = Console.ReadLine();
                List<WatchResult> watches = watchBuss.Search(keyword);
                Print(watches);
                Console.WriteLine("Bạn có muốn xóa tiếp không?(nhấn esc để thoát)");
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    Show();
                    break;
                }
            }
        }

        private void Delete()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("Mã mặt hàng: ");
                string watchId = Console.ReadLine();
                WatchResult watch = watchBuss.GetWatch(watchId);
                if (watch == null)
                {
                    Console.WriteLine("Không tìm thấy mặt hàng nào!");
                    Console.WriteLine("Bạn có muốn xóa tiếp không?(nhấn esc để thoát)");
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        Show();
                        break;
                    }
                }
                else
                {
                    watchBuss.Delete(watch.WatchId);
                    Console.Write("Đã xóa...");
                    Console.WriteLine("Bạn có muốn xóa tiếp không?(nhấn esc để thoát)");
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        watchBuss.Save();
                        Show();
                        break;
                    }
                }
            }
        }

        private void Update()
        {
            string brandId, watchId, name, origin, type;
            int quantity=0;
            float price;
            WatchResult result;
            while (true)
            {
                Console.Clear();

                //mã mặt hàng
                while (true)
                {
                    Console.Write("Mã mặt hàng: ");
                    watchId = Console.ReadLine();
                    if (watchId != "")
                    {
                        result = watchBuss.GetWatch(watchId);
                        if (result == null)
                        {
                            Console.WriteLine("Không tìm được mặt hàng nào");
                        }
                        else break;
                        
                    }
                    else
                    {
                        Console.WriteLine("Không tìm thấy mặt hàng nào");
                        Console.WriteLine("Bạn có muốn sửa tiếp không?(nhấn esc để thoát)");
                        ConsoleKeyInfo esc = Console.ReadKey();
                        if (esc.Key == ConsoleKey.Escape)
                        {
                            Show();
                            return;
                        }
                    }
                }

                //mã nhãn hàng
                while (true)
                {
                    Console.Write("Mã nhãn hàng: ");
                    brandId = Console.ReadLine();
                    if (brandId != "")
                    {
                        Brand brand = brandBuss.GetBrand(brandId);
                        if (brand == null)
                        {
                            Console.WriteLine("Mã nhãn hàng không tồn tại!");
                            Thread.Sleep(300);
                        }
                        else
                        {
                            result.BrandId = brand.BrandId;
                            break;
                        };
                    }
                    else break;
                }

                //tên mặt hàng
                while (true)
                {
                    Console.Write("Tên mặt hàng: ");
                    name = Console.ReadLine();
                    if (name != "")
                    {
                        result.Name = name;
                        break;
                    }
                    else break;
                }

                //số lượng
                while (true)
                {
                    try
                    {
                        Console.Write("Số lượng: ");
                        quantity = int.Parse(Console.ReadLine());
                        if (quantity >= 0)
                        {
                            result.Quantity = quantity;
                            break;
                        }
                        else
                            Console.WriteLine("Số lượng >= 0");
                    }
                    catch
                    {
                        break;
                    }

                }

                //giá bán
                while (true)
                {
                    try
                    {
                        Console.Write("Giá bán: ");
                        price = float.Parse(Console.ReadLine());
                        if (price >= 0)
                        {
                            result.Price = price;
                            break;
                        }
                        else
                            Console.WriteLine("Giá tiền >=0");
                    }
                    catch
                    {
                        break;
                    }
                }

                //xuất xứ
                while (true)
                {
                    Console.Write("Xuất xứ: ");
                    origin = Console.ReadLine();
                    if (origin != "")
                    {
                        result.Origin = origin;
                        break;
                    }
                    else break;
                }

                //Loại
                while (true)
                {
                    Console.Write("Dành cho(Nam: 1/Nữ: 0): ");
                    type = Console.ReadLine();
                    if (type != "")
                    {
                        result.Type = type == "1" ? true : false;
                        break;
                    }
                    else break;
                }

                watchBuss.Update(result);
                Console.WriteLine("Bạn có muốn nhập tiếp không?(nhấn esc để thoát)");
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    watchBuss.Save();
                    Show();
                    break;
                }
            }
        }

        private void Add()
        {
            string brandId, watchId, name, origin, type;
            int quantity;
            float price;
            while (true)
            {
                Console.Clear();
                //mã nhãn hàng
                while (true)
                {
                    Console.Write("Mã nhãn hàng: ");
                    brandId = Console.ReadLine();
                    if (brandId != "") {
                        Brand brand = brandBuss.GetBrand(brandId);
                        if (brand == null)
                        {
                            Console.WriteLine("Mã nhãn hàng không tồn tại!");
                            Thread.Sleep(300);
                        }
                        else break;
                    }
                    else
                    {
                        Console.WriteLine("Mã nhãn hàng không được để trống!");
                        Thread.Sleep(300);
                    }
                }

                //mã mặt hàng
                while (true)
                {
                    Console.Write("Mã mặt hàng: ");
                    watchId = Console.ReadLine();
                    if (watchId != "")
                    {
                        WatchResult watch1 = watchBuss.GetWatch(watchId);
                        if (watch1 != null)
                        {
                            Console.WriteLine("Mã mặt hàng đã tồn tại!");
                            Thread.Sleep(300);
                        }
                        else break;
                    }
                    else
                    {
                        Console.WriteLine("Mã nhãn hàng không được để trống!");
                        Thread.Sleep(300);
                    }
                }

                //tên mặt hàng
                while (true)
                {
                    Console.Write("Tên mặt hàng: ");
                    name = Console.ReadLine();
                    if (name != "")
                        break;
                    else
                    {
                        Console.WriteLine("Tên mặt hàng không được để trống");
                    }    
                }

                //số lượng
                while (true)
                {
                    try
                    {
                        Console.Write("Số lượng: ");
                        quantity = int.Parse(Console.ReadLine());
                        if (quantity >= 0)
                            break;
                        else
                            Console.WriteLine("Số lượng >=0");
                    }
                    catch
                    {
                        Console.WriteLine("Số lượng là số từ (0-9)");
                    }

                }

                //giá bán
                while (true)
                {
                    try
                    {
                        Console.Write("Giá bán: ");
                        price = float.Parse(Console.ReadLine());
                        if (price >= 0)
                            break;
                        else
                            Console.WriteLine("Giá tiền >=0");
                    }
                    catch
                    {
                        Console.WriteLine("Số lượng là số từ (0-9)");
                    }
                }

                //xuất xứ
                while (true)
                {
                    Console.Write("Xuất xứ: ");
                    origin = Console.ReadLine();
                    if (origin != "")
                        break;
                    else
                    {
                        Console.WriteLine("Xuất xứ không được để trống");
                    }
                }

                //Loại
                while (true)
                {
                    Console.Write("Dành cho(Nam: 1/Nữ: 0): ");
                    type = Console.ReadLine();
                    if (type != "")
                        break;
                    else
                    {
                        Console.WriteLine("Loại không được để trống");
                    }
                }

                Watch watch = new Watch()
                {
                    BrandId = brandId,
                    Name = name,
                    Origin = origin,
                    Price = price,
                    Quantity = quantity,
                    Type = type == "1" ? true : false,
                    WatchId = watchId
                };
                watchBuss.Add(watch);
                Console.WriteLine("Bạn có muốn nhập tiếp không?(nhấn esc để thoát)");
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    watchBuss.Save();
                    Show();
                    break;
                }
            }
        }

        private void ShowAll()
        {
            while (true)
            {
                List<WatchResult> watches = watchBuss.GetWatches();
                Print(watches);
                Console.WriteLine("nhấn esc để thoát");
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    Show();
                    break;
                }
            }
        }

        private void Print(List<WatchResult> watches)
        {
            Console.Clear();
            Table table = new Table(90);
            table.PrintLine();
            table.PrintRow("Mã mặt hàng", "Nhãn hàng", "Tên sản phẩm", "Giá", "Số lượng", "Xuất xứ", "Dành cho");
            table.PrintLine();
            foreach (var sub in watches)
            {
                table.PrintRow(sub.WatchId, sub.BrandName, sub.Name, sub.Price.ToString(), sub.Quantity.ToString(), sub.Origin, sub.Type?"Nam":"Nữ");
            }
            table.PrintLine();
        }

        
    }
}
