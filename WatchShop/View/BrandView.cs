using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WatchShop.Interfaces;
using ConsoleLib.UI;
using WatchShop.Entity;
using System.Diagnostics;

namespace WatchShop.View
{
    public class BrandView : View
    {
        private IBrandBuss brandBuss;
        public BrandView(IBrandBuss brandBuss, Stack<IView> pages): base(pages)
        {
            this.brandBuss = brandBuss;
        }

        public override void Show()
        {
            base.Show();
            MenuSelector selector = new MenuSelector(new string[] {
                "1.Hiển thị toàn bộ nhãn hàng",
                "2.Thêm nhãn hàng",
                "3.Sửa thông tin nhãn hàng",
                "4.Xóa nhãn hàng",
                "5.Tìm kiếm",
                "6.Thoát"
            }, "Quản lý nhãn hàng");
            switch (selector.Select())
            {
                case 0: ShowAll(); break;
                case 1: Add(); break;
                case 2: Update(); break;
                case 3: Delete(); break;
                case 4: Search(); break;
                case 5: Back(); break;
            }
        }

        private void Add()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("Mã nhãn hàng: ");
                string brandId = Console.ReadLine();
                Console.Write("Tên nhãn hàng: ");
                string brandName = Console.ReadLine();
                if (brandId != "" && brandName != "")
                {
                    Brand brand = brandBuss.GetBrand(brandId);
                    if (brand != null)
                    {
                        Console.WriteLine("Mã sản nhãn hàng đã tồn tại");
                        Thread.Sleep(300);
                        Console.Clear();
                    }
                    else
                    {
                        brandBuss.Add(new Brand
                        {
                            BrandId = brandId,
                            BrandName = brandName
                        });
                        Console.WriteLine("Bạn có muốn nhập tiếp không?(nhấn esc để thoát)");
                        ConsoleKeyInfo key = Console.ReadKey();
                        if (key.Key == ConsoleKey.Escape)
                        {
                            brandBuss.Save();
                            Show();  
                            break;
                        }
                    }
                } else {
                    Console.WriteLine("Mã nhãn hàng và tên nhãn hàng không được để trống");
                }
                
            }
        }

        private void ShowAll()
        {
            while (true)
            {
                List<Brand> brands = brandBuss.GetBrands();
                Print(brands);
                Console.WriteLine("nhấn esc để thoát");
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    Show();
                    break;
                }
            }
        }

        private void Update()
        {
            while (true) 
            {
                Console.Clear();
                Console.Write("Mã nhãn hàng: ");
                string brandId = Console.ReadLine();
                Brand brand = brandBuss.GetBrand(brandId);
                if(brand == null)
                {
                    Console.WriteLine("Không tìm thấy nhãn hàng nào!");
                    Console.WriteLine("Bạn có muốn sửa tiếp không?(nhấn esc để thoát)");
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        Show();
                        break;
                    }
                }
                else
                {
                    Console.Write("Tên nhãn hàng: ");
                    string brandName = Console.ReadLine();
                    if(brandName != null)
                    {
                        brand.BrandName = brandName;
                    }
                    Console.WriteLine("Bạn có muốn sửa tiếp không?(nhấn esc để thoát)");
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        brandBuss.Save();
                        Show();
                        break;
                    }
                }
                    
            }
        }

        private void Delete()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("Mã nhãn hàng: ");
                string brandId = Console.ReadLine();
                Brand brand = brandBuss.GetBrand(brandId);
                if (brand == null)
                {
                    Console.WriteLine("Không tìm thấy nhãn hàng nào!");
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
                    brandBuss.Delete(brand.BrandId);
                    Console.Write("Đã xóa...");
                    Console.WriteLine("Bạn có muốn xóa tiếp không?(nhấn esc để thoát)");
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        brandBuss.Save();
                        Show();
                        break;
                    }
                }
            }
        }

        private void Search()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("Từ khóa tìm kiếm: ");
                string keyword = Console.ReadLine();
                List<Brand> brands = brandBuss.Search(keyword);
                Print(brands);
                Console.WriteLine("Bạn có muốn xóa tiếp không?(nhấn esc để thoát)");
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                   Show();
                   break;
                }
            }
        }

        private void Print(List<Brand> brands)
        {
            Console.Clear();
            Table table = new Table(90);
            table.PrintLine();
            table.PrintRow("ID", "Tên bộ môn");
            table.PrintLine();
            foreach (var sub in brands)
            {
                table.PrintRow(sub.BrandId, sub.BrandName);
            }
            table.PrintLine();
        }
    }
}
