using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.Interfaces;
using WatchShop.Entity;
namespace WatchShop.Bussiness
{
    public class StatisBuss : IStatisticBuss
    {
        private IOrderAcess orderAcess;
        private IImportAcess importAcess;
        private IOrderDetailAcess orderDetailAcess;
        private IImportDetailAcess imporDetailtAcess;
        public StatisBuss(IOrderAcess orderAcess, IImportAcess importAcess, IOrderDetailAcess orderDetailAcess, IImportDetailAcess imporDetailtAcess)
        {
            this.orderAcess = orderAcess;
            this.importAcess = importAcess;
            this.orderDetailAcess = orderDetailAcess;
            this.imporDetailtAcess = imporDetailtAcess;
        }
        public List<StatisResult> Statis(int year)
        {
            List<Order> orders = orderAcess.GetOrders();
            List<Import> imports = importAcess.GetImports();
            List<OrderDetail> orderDetails = orderDetailAcess.GetOrderDetails();
            List<ImportDetail> importDetails = imporDetailtAcess.GetImportDetails();

            var queryProfit = from d in orderDetails
                        join i in importDetails on d.WatchId equals i.WatchId
                        join o in orders on d.OrderId equals o.OrderID
                        where o.OrderDate.Year == year
                        group new { 
                                        UnitPrice = d.UnitPrice, 
                                        ImportPrice = i.UnitPrice 
                                  } 
                        by o.OrderDate.Month
                        into t
                        select new SellResult
                        {
                            Month = t.Key,
                            Sell = t.Sum(x=>x.UnitPrice),
                            Profit = t.Sum(x=>x.UnitPrice)-t.Sum(x=>x.ImportPrice)
                        };
            var queryImport = from d in importDetails
                              join i in imports on d.ImportId equals i.ImportID
                              where i.ImportDate.Year == year
                              group d.UnitPrice by i.ImportDate.Month
                              into t
                              select new SpendResult
                              {
                                  Month = t.Key,
                                  Spend = t.Sum()
                              };
            List < SellResult > sellResults = queryProfit.ToList();
            List<SpendResult> spendResults = queryImport.ToList();

            var query = from sell in sellResults
                        join spend in spendResults
                        on sell.Month equals spend.Month
                        select new StatisResult
                        {
                            Month = sell.Month,
                            Profit = sell.Profit,
                            Sell = sell.Sell,
                            Spend = spend.Spend
                        };
            return query.ToList();
        }
    }
}
