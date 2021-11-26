using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.Entity;
using WatchShop.Interfaces;
using ConsoleLib.Interfaces;
namespace WatchShop.DataAcess
{
    public class OrderDetailDataAcess : IOrderDetailAcess
    {
        private string path;
        private List<OrderDetail> details;
        private IDataHelper helper;

        public OrderDetailDataAcess(string path, IDataHelper helper)
        {
            this.path = path;
            this.helper = helper;
            this.details = new List<OrderDetail>();
        }
        public void Add(OrderDetail orderDetail)
        {
            details.Add(orderDetail);
        }

        public void Delete(OrderDetail orderDetail)
        {
            details.Remove(orderDetail);
        }

        public OrderDetail GetOrderDetail(string orderDetailID)
        {
            foreach (var d in details)
                if (d.DetailId == orderDetailID)
                    return d;
            return null;
        }

        public List<OrderDetail> GetOrderDetails()
        {
            if (details.Count == 0)
                details = helper.Convert<OrderDetail>(helper.ReadFile(path));
            return details;
        }

        public List<OrderDetail> GetOrderDetails(string orderID)
        {
            List<OrderDetail> result = new List<OrderDetail>();
            foreach (var d in details)
                if (d.OrderId == orderID)
                    result.Add(d);
            return result;
        }

        public void Save()
        {
            helper.WriteFile<OrderDetail>(path, details);
        }

        public List<OrderDetail> Search(string keyword)
        {
            List<OrderDetail> result = new List<OrderDetail>();
            foreach (var d in details)
                if (d.DetailId.Contains(keyword) ||
                    d.Discount.ToString().Contains(keyword) ||
                    d.UnitPrice.ToString().Contains(keyword) ||
                    d.Quantity.ToString().Contains(keyword) ||
                    d.WatchId.Contains(keyword))
                    result.Add(d);
            return result;
        }

        public void Update(OrderDetail orderDetail)
        {
            OrderDetail old = GetOrderDetail(orderDetail.DetailId);
            if (old == null)
                return;
            old.Discount = orderDetail.Discount;
            old.OrderId = orderDetail.OrderId;
            old.Quantity = orderDetail.Quantity;
            old.UnitPrice = orderDetail.UnitPrice;
            old.WatchId = orderDetail.WatchId;
        }
    }
}
