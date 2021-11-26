using ConsoleLib.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.Interfaces;
using WatchShop.Entity;
namespace WatchShop.View
{
    public class Sellview: View
    {
        private ISellBuss sellBuss;
        public Sellview(ISellBuss sellBuss, Stack<IView> views) : base(views)
        {
            this.sellBuss = sellBuss;
            
        }

        public override void Show()
        {
            base.Show();
            MenuSelector menu = new MenuSelector(new string[]
            {
               "1.Hiển thị toàn bộ đơn hàng",
               "2.Hiển thị hóa đơn",
               "3.Thêm đơn hàng",
               "4.Cập nhật đơn hàng",
               "5.Xóa đơn hàng",
               "6.Tìm kiếm đơn hàng",
               "7.Thoát"
            }, "Quản lý nhập hàng");
            switch (menu.Select())
            {
                case 0: ShowAll(); break;
                case 1: ShowOrder(); break;
                case 2: Add(); break;
                case 3: Update(); break;
                case 4: Delete(); break;
                case 5: Search(); break;
                case 6: Back(); break;
            }
        }

        private void Search()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Từ khóa tìm kiếm: ");
                string keyword = Console.ReadLine();
                List<Order> result = sellBuss.Search(keyword);
                Print(result);
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
                Console.WriteLine("Mã đơn hàng: ");
                string importId = Console.ReadLine();
                Order import = sellBuss.GetOrder(importId);
                if (import == null)
                {
                    Console.WriteLine("Không tìm thấy đơn hàng nào!");
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
                    sellBuss.Delete(import.OrderID);
                    Console.Write("Đã xóa...");
                    Console.WriteLine("Bạn có muốn xóa tiếp không?(nhấn esc để thoát)");
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        sellBuss.Save();
                        Show();
                        break;
                    }
                }
            }
        }

        private void Update()
        {
            ConsoleKeyInfo key;
            while (true)
            {
                Console.Clear();
                Console.Write("Mã đơn hàng: ");
                string orderId = Console.ReadLine();
                Order order = sellBuss.GetOrder(orderId);
                if (order == null)
                {
                    Console.WriteLine("Không tìm thấy đơn hàng nào");
                    Console.WriteLine("Bạn có muốn xem tiếp không?(esc để thoát)");
                    key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        Show();
                        break;
                    }
                }
                else
                {
                    List<OrderDetailResult> details = sellBuss.GetOrderDetails(orderId);
                    PrintImport(order, details);
                    Console.WriteLine("Bạn có muốn sửa đơn hàng này không?(esc để thoát)");
                    key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        Show();
                        break;
                    }
                    else TypeUpdateDetails(details);
                    sellBuss.Save();
                }
            }
        }

        private void TypeUpdateDetails(List<OrderDetailResult> details)
        {
            int i;
            while (true)
            {
                while (true)
                {
                    try
                    {
                        Console.WriteLine($"Chọn mặt hàng bạn muốn sửa (1->{details.Count}):");
                        i = int.Parse(Console.ReadLine()) - 1;
                        if (i < 0 || i > details.Count - 1)
                            Console.WriteLine($"bạn phải nhập số từ (1->{details.Count})");
                        else break;
                    }
                    catch
                    {
                        Console.WriteLine($"bạn phải nhập số từ (1->{details.Count})");
                    }

                }
                OrderDetailResult detail = details[i];
                int quantity = (int)TypeNumber("Số lượng", true);
                float discount = TypeNumber("Giảm giá(%):", true);
                detail.Quantity = quantity > 0 ? quantity : detail.Quantity;
                detail.Discount = discount >= 0 ? discount: detail.Discount;
                sellBuss.UpdateDetail(detail);
                Console.WriteLine("Bạn có muốn sửa thông tin mặt hàng tiếp không?(nhấn esc để thoát)");
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                    break;
            }
        }

        private void Add()
        {
            while (true)
            {
                Console.Clear();
                string id = TypeOrderId();
                string custom = TypeCustom();
                Console.WriteLine("Thông tin các mặt hàng: ");
                float total = TypeDetails(id);
                Order order = new Order()
                {
                    Customer = custom,
                    OrderID = id,
                    OrderDate = DateTime.Now,
                    TotalPrice = total
                };
                sellBuss.Add(order);
                Console.WriteLine("Bạn có muốn nhập thêm đơn hàng nào không?(esc để thoát)");
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    sellBuss.Save();
                    Show();
                    break;
                }
            }
        }

        private void ShowOrder()
        {
            ConsoleKeyInfo key;
            while (true)
            {
                Console.Clear();
                Console.Write("Mã đơn hàng: ");
                string orderId = Console.ReadLine();
                Order order = sellBuss.GetOrder(orderId);
                if (order == null)
                {
                    Console.WriteLine("Không tìm thấy đơn hàng nào");
                    Console.WriteLine("Bạn có muốn xem tiếp không?(esc để thoát)");
                    key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        Show();
                        break;
                    }
                }
                else
                {
                    List<OrderDetailResult> details = sellBuss.GetOrderDetails(orderId);
                    PrintImport(order, details);
                    Console.WriteLine("Bạn có muốn xem tiếp không?(esc để thoát)");
                    key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        Show();
                        break;
                    }
                }
            }
        }
        
        private string TypeOrderId()
        {
            while (true)
            {
                Console.Write("Mã đơn hàng: ");
                string id = Console.ReadLine();
                Order order = sellBuss.GetOrder(id);
                if (order == null)
                    return id;
                else Console.WriteLine("Đơn hàng đã tồn tại");
            }
        }

        private string TypeCustom(bool isUpdate = false)
        {
            while (true)
            {
                Console.Write("Thông tin khách hàng: ");
                string info = Console.ReadLine();
                if (info == "")
                    if (isUpdate)
                        return null;
                    else Console.WriteLine("Thông tin khách hàng không được để trống!");
                else
                    return info;
            }
        }

        private Watch GetWatch()
        {
            while (true)
            {
                Console.Write("Mã mặt hàng: ");
                string id = Console.ReadLine();
                if (id == "")
                    Console.WriteLine("Mã mặt hàng không được để trống");
                else
                {
                    Watch watch = sellBuss.GetWatch(id);
                    if (watch == null)
                        Console.WriteLine("Không tìm thấy mặt hàng nào!");
                    else return watch;
                }
            }
        }

        private float TypeNumber(string header, bool isUpdate = false)
        {
            while (true)
            {
                try
                {
                    Console.Write(header + ": ");
                    float price = float.Parse(Console.ReadLine());
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

        private float TypeDetails(string orderId)
        {
            float total = 0;
            int i = 1;
            while (true)
            {
                Console.WriteLine($"Mặt hàng thứ {i}");
                Watch watch = GetWatch();
                int quantity = (int)TypeNumber("Số lượng");
                float discount = TypeNumber("Giảm giá(%)");
                float unitPrice = quantity * watch.Price * (1 - discount);
                OrderDetail detail = new OrderDetail
                {
                    Discount = discount,
                    OrderId = orderId,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                    WatchId = watch.WatchId
                };
                if(sellBuss.AddDetail(detail, watch))
                {
                    total += unitPrice;
                    i++;
                    Console.WriteLine("Bạn có muốn nhập thêm sản phẩm nào không?(esc để thoát)");
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                        return total;
                }
                else
                {
                    Console.WriteLine("Số lượng không đáp ứng");
                }
            }
        }

        private void ShowAll()
        {
            while (true)
            {
                List<Order> imports = sellBuss.GetOrders();
                Print(imports);
                Console.WriteLine("nhấn esc để thoát");
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    Show();
                    break;
                }
            }
        }

        private void Print(List<Order> orders)
        {
            Console.Clear();
            Table table = new Table(100);
            table.PrintLine();
            table.PrintRow("Mã đơn hàng", "Khách hàng", "Ngày bán", "Tổng giá");
            table.PrintLine();
            foreach (var sub in orders)
            {
                table.PrintRow(sub.OrderID, sub.Customer, sub.OrderDate.ToString("dd-MM-yyyy"), sub.TotalPrice.ToString());
            }
            table.PrintLine();
        }

        private void Print(List<OrderDetailResult> imports)
        {
            Table table = new Table(100);
            table.PrintLine();
            table.PrintRow("Stt", "Mặt hàng", "Số lượng", "Đơn giá");
            table.PrintLine();
            for (int i = 0; i < imports.Count; i++)
            {
                table.PrintRow((i + 1).ToString(), imports[i].WatchName, imports[i].Quantity.ToString(), imports[i].UnitPrice.ToString());
            }
            table.PrintLine();
        }

        private void PrintImport(Order order, List<OrderDetailResult> details)
        {
            Console.Clear();
            Console.WriteLine("Thông tin cũ");
            Console.WriteLine($"Khách hàng: {order.Customer}");
            Console.WriteLine($"Ngày bán: {order.OrderDate.ToString("dd/MM/yyyy")}");
            Console.WriteLine($"Tổng giá: {order.TotalPrice}");
            Console.WriteLine("Các mặt hàng được nhập:");
            Print(details);

        }
    }
}
