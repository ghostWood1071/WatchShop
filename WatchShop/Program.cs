using System;
using WatchShop.Entity;
using WatchShop.Bussiness;
using WatchShop.DataAcess;
using WatchShop.Interfaces;
using ConsoleLib.Interfaces;
using ConsoleLib.Data;
using ConsoleLib.UI;
using WatchShop.View;
using System.Collections.Generic;
namespace WatchShop
{
    class Program
    {
        static void Main(string[] args)
        {
            //khai báo thư viện hỗ trợ đọc ghi file
            IDataHelper helper = new DataHelper();

            //khai báo tầng data acess;
            IBrandAcess brandAcess = new BrandDataAcess("brand.txt", helper);
            IImportAcess importAcess = new ImportDataAcess("import.txt", helper);
            IImportDetailAcess importDetailAcess = new ImportDetailDataAcess("import_detail.txt", helper);
            IOrderAcess orderAcess = new OrderDataAcess("order.txt", helper);
            IOrderDetailAcess detailAcess = new OrderDetailDataAcess("order_detail.txt", helper);
            IWatchAcess watchAcess = new WatchDataAcess("watch.txt", helper);

            //đọc dữ liệu từ file
            brandAcess.GetBrands();
            importAcess.GetImports();
            importDetailAcess.GetImportDetails();
            orderAcess.GetOrders();
            detailAcess.GetOrderDetails();
            watchAcess.GetWatches();

            //khai báo tầng bussiness
            IBrandBuss brandBuss = new BrandBuss(brandAcess, watchAcess); // chức năng quản lý nhãn hàng
            IWatchBuss watchBuss = new WatchBuss(watchAcess, brandAcess); // chức năng quản lý đồng hồ 
            IImportBuss importBuss = new ImportBuss(importAcess, importDetailAcess, watchAcess, brandAcess); // chức năng nhập hàng
            ISellBuss sellBuss = new SellBuss(orderAcess, detailAcess, watchAcess); // chức năng bán hàng
            IStatisticBuss statisticBuss = new StatisBuss(orderAcess, importAcess, detailAcess, importDetailAcess); // chức năng thống kê;

            //Khai báo tầng view
            Stack<IView> pages = new Stack<IView>();
            Homeview mainMenu = new Homeview(new IView[]
            {
                new BrandView(brandBuss, pages),
                new WatchView(watchBuss, brandBuss, pages),
                new ImportView(importBuss, pages),
                new Sellview(sellBuss, pages),
                new StatisView(statisticBuss, pages)
            }, pages);
            mainMenu.Show();
        }
    }
}
