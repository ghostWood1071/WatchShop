using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.Interfaces;
using WatchShop.Entity;
using ConsoleLib.UI;
using System.Threading;

namespace WatchShop.View
{
    class ImportView: View
    {
        private IImportBuss importBuss;
        
        public ImportView(IImportBuss importBuss, Stack<IView> views):base(views)
        {
            this.importBuss = importBuss;
            
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
                case 1: ShowImport(); break;
                case 2: Add(); break;
                case 3: Update(); break;
                case 4: Delete(); break;
                case 5: Search(); break;
                case 6: Back(); break;
            }
        }

        private void ShowImport()
        {
            ConsoleKeyInfo key;
            while (true)
            {
                Console.Clear();
                ImportResult import = InputImportId();
                if (import == null)
                    break;
                List<ImportDetailResult> details = importBuss.GetImportDetails(import.ImportID);
                PrintImport(import, details);
                Console.WriteLine("Bạn có muốn xem tiếp không?(esc để thoát)");
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    Show();
                    break;
                }
            }
        }

        private void Search()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Từ khóa tìm kiếm: ");
                string keyword = Console.ReadLine();
                List<ImportResult> result = importBuss.Search(keyword);
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
                ImportResult import = importBuss.GetImport(importId);
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
                    importBuss.Delete(import.ImportID);
                    Console.Write("Đã xóa...");
                    Console.WriteLine("Bạn có muốn xóa tiếp không?(nhấn esc để thoát)");
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        importBuss.Save();
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
                ImportResult import = InputImportId();
                if (import == null)
                    break;
                List<ImportDetailResult> details = importBuss.GetImportDetails(import.ImportID);
                PrintImport(import, details);
                Console.WriteLine("Bạn có muốn sửa thông tin đơn hàng này không?(esc để thoát)");
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    Show();
                    break;
                }
                else TypeUpdateDetails(details);

            }
        }
        
        private void Add()
        {
            while (true)
            {
                Console.Clear();
                string importId = TypeImportID();
                Brand brand = TypeBrand();
                float totalPrice = TypeDetails(importId, brand.BrandId);
                Import import = new Import()
                {
                    BrandID = brand.BrandId,
                    ImportDate = DateTime.Now,
                    ImportID = importId,
                    TotalPrice = totalPrice
                };
                importBuss.Add(import, brand);
                Console.WriteLine("Bạn có muốn nhập tiếp không?(nhấn esc để thoát)");
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    importBuss.Save();
                    Show();
                    break;
                }

            }
                    
        }

        private float TypeDetails(string importId, string brandId)
        {
            int i = 1;
            float totalPrice = 0;
            while (true)
            {
                Console.WriteLine($"Mặt hàng {i}: ");
                
                float salePrice = TypeNumber("Giá bán");
                float importPrice = TypeNumber("Giá nhập");
                int quantity = (int)TypeNumber("Số lượng");
                float unitPrice = quantity * importPrice;
                Watch watch = TypeWatch(brandId, salePrice, quantity);
                ImportDetail detail = new ImportDetail()
                {
                    SalePrice = salePrice,
                    ImportId = importId,
                    Quantity = quantity,
                    ImportPrice = importPrice,
                    WatchId = watch.WatchId,
                    UnitPrice = unitPrice
                };
                importBuss.AddDetail(detail, watch);
                i++;
                totalPrice += unitPrice;
                Console.WriteLine("Bạn có muốn nhập tiếp không?(nhấn esc để thoát)");
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                    break;
            }
            return totalPrice;
        }

        private float TypeNumber(string header, bool isUpdate = false)
        {
            while (true)
            {
                try
                {
                    Console.Write(header+": ");
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

        private string TypeImportID()
        {
            string id;
            while (true)
            {
                Console.Write("Mã đơn hàng: ");
                id = Console.ReadLine();
                if (id != "")
                {
                    ImportResult import = importBuss.GetImport(id);
                    if (import == null)
                        return id;
                    else 
                        Console.WriteLine("Mã nhãn hàng đã tồn tại!");
                }
                else
                    Console.WriteLine("Mã nhãn hàng không được để trống!");
            }
        }

        private ImportResult InputImportId()
        {
            string id;
            ConsoleKeyInfo key;
            while (true)
            {
                Console.Write("Mã đơn hàng: ");
                id = Console.ReadLine();
                if (id != "")
                {
                    ImportResult import = importBuss.GetImport(id);
                    if (import != null)
                        return import;
                    else
                    {
                        Console.WriteLine("Mã nhãn hàng không tồn tại!");
                        key = Console.ReadKey();
                        if (key.Key == ConsoleKey.Escape)
                        {
                            importBuss.Save();
                            Show();
                            return null;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Mã nhãn hàng không được để trống!");
                    key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        importBuss.Save();
                        Show();
                        return null;
                    }
                }
                
            }

        }

        private Brand TypeBrand()
        {
            string brandId;
            while (true)
            {
                Console.Write("Mã nhãn hàng: ");
                brandId = Console.ReadLine();
                if (brandId != "")
                {
                    Brand brand = importBuss.GetBrand(brandId);
                    if (brand == null)
                        return CreateBrand(brandId);
                    else return brand;
                }
                else
                {
                    Console.WriteLine("Mã nhãn hàng không được để trống!");
                    Thread.Sleep(300);
                }
            }
        }

        private Watch TypeWatch(string brandId, float price, int qunatity)
        {
            string watchId;
            while (true)
            {
                Console.Write("Mã mặt hàng: ");
                watchId = Console.ReadLine();
                if (watchId != "")
                {
                    Watch watch = importBuss.GetWatch(watchId);
                    if (watch != null)
                        return watch;
                    else return CreateWatch(watchId, brandId, price, qunatity);
                }
                else
                {
                    Console.WriteLine("Mã nhãn hàng không được để trống!");
                    
                }
            }
        }

        private Watch CreateWatch(string watchId,string brandId, float price, int qunatity)
        {
            string name, origin, type;
            while (true)
            {
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
                    Quantity = qunatity,
                    Type = type == "1" ? true : false,
                    WatchId = watchId
                };
                return watch;
                
            }
        }
        
        private Brand CreateBrand(string id)
        {
            while (true)
            {
                Console.Write("Tên nhãn hàng: ");
                string name = Console.ReadLine();
                if (name != null)
                    return new Brand()
                    {
                        BrandName = name,
                        BrandId = id
                    };
                else
                    Console.WriteLine("Tên nhãn hàng không được để trống");
            }
        }
        
        private void ShowAll()
        {
            while (true)
            {
                List<ImportResult> imports = importBuss.GetImports();
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
        
        private void Print(List<ImportResult> imports)
        {
            Console.Clear();
            Table table = new Table(100);
            table.PrintLine();
            table.PrintRow("Mã đơn hàng", "Nhà phân phối", "Ngày nhập", "Tổng giá");
            table.PrintLine();
            foreach (var sub in imports)
            {
                table.PrintRow(sub.ImportID, sub.BrandName, sub.ImportDate.ToString("dd-MM-yyyy"), sub.TotalPrice.ToString());
            }
            table.PrintLine();
        }
        
        private void Print(List<ImportDetailResult> imports)
        {
            Table table = new Table(100);
            table.PrintLine();
            table.PrintRow("Stt","Mặt hàng", "Số lượng", "Giá nhập", "Giá bán", "Đơn giá");
            table.PrintLine();
            for(int i = 0; i<imports.Count; i++)
            {
                table.PrintRow((i+1).ToString(),imports[i].WatchName, imports[i].Quantity.ToString(), imports[i].ImportPrice.ToString(), imports[i].SalePrice.ToString(), imports[i].UnitPrice.ToString());
            }
            table.PrintLine();
        }
        
        private Brand GetBrand()
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
                    Brand brand = importBuss.GetBrand(brandId);
                    if (brand != null)
                    {
                        Console.WriteLine("Mã sản nhãn hàng đã tồn tại");
                        Thread.Sleep(300);
                        Console.Clear();
                    }
                    else return new Brand
                    {
                        BrandId = brandId,
                        BrandName = brandName
                    };
                }
                else
                {
                    Console.WriteLine("Mã nhãn hàng và tên nhãn hàng không được để trống");
                }

            }
        }
        
        private void TypeUpdateDetails(List<ImportDetailResult> details)
        {
            int i;
            while (true)
            {
                while (true)
                {
                    try
                    {
                        Console.WriteLine($"Chọn mặt hàng bạn muốn sửa (1->{details.Count}):");
                        i = int.Parse(Console.ReadLine()) -1;
                        if (i < 0 || i > details.Count - 1)
                            Console.WriteLine($"bạn phải nhập số từ (1->{details.Count})");
                        else break;
                    }
                    catch
                    {
                        Console.WriteLine($"bạn phải nhập số từ (1->{details.Count})");
                    }

                }
                ImportDetailResult detail = details[i];
                int quantity = (int)TypeNumber("Số lượng", true);
                float salePrice = TypeNumber("Giá bán", true);
                float importPrice = TypeNumber("Giá nhập", true);
                detail.Quantity = quantity > 0 ? quantity : detail.Quantity;
                detail.SalePrice = salePrice > 0 ? salePrice : detail.SalePrice;
                detail.ImportPrice = importPrice > 0 ? importPrice : detail.ImportPrice;
                importBuss.UpdateDetail(detail);
                Console.WriteLine("Bạn có muốn sửa thông tin mặt hàng tiếp không?(nhấn esc để thoát)");
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                    break;
            }
        }
        
        private void PrintImport(ImportResult import, List<ImportDetailResult> details)
        {
            Console.Clear();
            Console.WriteLine("Thông tin cũ");
            Console.WriteLine($"Nhãn hàng: {import.BrandName}");
            Console.WriteLine($"Ngày nhập: {import.ImportDate.ToString("dd/MM/yyyy")}");
            Console.WriteLine($"Tổng giá: {import.TotalPrice}");
            Console.WriteLine("Các mặt hàng được nhập:");
            Print(details);
            
        }
    }
}
