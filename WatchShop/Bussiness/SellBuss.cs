using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.Entity;
using WatchShop.Interfaces;
namespace WatchShop.Bussiness
{
    public class SellBuss : ISellBuss
    {
        private IWatchAcess watchAcess;
        private IOrderAcess orderAcess;
        private IOrderDetailAcess detailAcess;
        public SellBuss(IOrderAcess orderAcess, IOrderDetailAcess detailAcess, IWatchAcess watchAcess)
        {
            this.orderAcess = orderAcess;
            this.detailAcess = detailAcess;
            this.watchAcess = watchAcess;
        }
        public void Add(Order order)
        {
            orderAcess.Add(order);
        }

        public bool AddDetail(OrderDetail orderDetail, Watch watch)
        {
            orderDetail.DetailId = Guid.NewGuid().ToString();
            if (watch.Quantity < orderDetail.Quantity)
                return false;
            watch.Quantity -= orderDetail.Quantity;
            detailAcess.Add(orderDetail);
            return true;
        }

        public void Delete(string orderId)
        {
            Order order = orderAcess.GetOrder(orderId);
            if (order == null)
                return;
            List<OrderDetail> details = detailAcess.GetOrderDetails(orderId);
            for(int i = 0; i<details.Count; i++)
            {
                Watch watch = watchAcess.GetWatch(details[i].WatchId);
                if (watch != null)
                    watch.Quantity += details[i].Quantity;
                detailAcess.Delete(details[i]);
            }
            orderAcess.Delete(order);
            
        }

        public void DeleteDetail(string detailId)
        {
            OrderDetail detail = detailAcess.GetOrderDetail(detailId);
            if (detail == null)
                return;
            Watch watch = watchAcess.GetWatch(detail.WatchId);
            if (watch != null)
                watch.Quantity += detail.Quantity;
            detailAcess.Delete(detail);
        }

        public Order GetOrder(string orderID)
        {
            return orderAcess.GetOrder(orderID);
        }

        public OrderDetailResult GetOrderDetail(string orderDetailId)
        {
            OrderDetail detail = detailAcess.GetOrderDetail(orderDetailId);
            if (detail == null)
                return null;
            Watch watch = watchAcess.GetWatch(detail.DetailId);
            return new OrderDetailResult
            {
                WatchId = detail.WatchId,
                WatchName = watch == null ? "" : watch.Name,
                DetailId = detail.DetailId,
                Discount = detail.Discount,
                OrderId = detail.OrderId,
                Quantity = detail.Quantity,
                UnitPrice = detail.UnitPrice
            };
        }

        public List<OrderDetailResult> GetOrderDetails(string orderId)
        {
            List<OrderDetail> details = detailAcess.GetOrderDetails(orderId);
            List<Watch> watches = watchAcess.GetWatches();
            var query = from d in details
                        join w in watches on d.WatchId equals w.WatchId
                        into gj
                        from c in gj.DefaultIfEmpty()
                        select new OrderDetailResult
                        {
                            DetailId = d.DetailId,
                            Discount = d.Discount,
                            OrderId = d.OrderId,
                            Quantity = d.Quantity,
                            UnitPrice = d.UnitPrice,
                            WatchId = d.WatchId,
                            WatchName = c == null ? "" : c.Name
                        };
            return query.ToList();
        }

        public List<Order> GetOrders()
        {
            return orderAcess.GetOrders();
        }

        public Watch GetWatch(string watchId)
        {
            return watchAcess.GetWatch(watchId);
        }

        public void Save()
        {
            watchAcess.Save();
            detailAcess.Save();
            orderAcess.Save();
        }

        public List<Order> Search(string keyword)
        {
            return orderAcess.Search(keyword);
        }

        public void Update(Order order)
        {
            orderAcess.Update(order);
        }

        public bool UpdateDetail(OrderDetail orderDetail)
        {
            Order order = orderAcess.GetOrder(orderDetail.OrderId);
            Watch watch = watchAcess.GetWatch(orderDetail.WatchId);
            OrderDetail detail = detailAcess.GetOrderDetail(orderDetail.DetailId);
            if (orderDetail.Quantity - detail.Quantity > watch.Quantity)
                return false;
            watch.Quantity -= (orderDetail.Quantity - detail.Quantity);
            order.TotalPrice -= (orderDetail.UnitPrice - detail.UnitPrice);
            detail.UnitPrice = orderDetail.Quantity * watch.Price * (1 - orderDetail.Discount);
            detailAcess.Update(orderDetail);
            return true;
        }
    }
}
