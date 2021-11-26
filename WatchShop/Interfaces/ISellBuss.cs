using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.Entity;
namespace WatchShop.Interfaces
{
    //quản lý bán hàng
    public interface ISellBuss
    {
        List<Order> GetOrders();
        List<Order> Search(string keyword);
        Order GetOrder(string orderID);
        void Add(Order order);
        void Delete(string orderId);
        void Update(Order order);

        List<OrderDetailResult> GetOrderDetails(string orderId);
        bool AddDetail(OrderDetail orderDetail, Watch watch);
        OrderDetailResult GetOrderDetail(string orderDetailId);
        void DeleteDetail(string detailId);
        bool UpdateDetail(OrderDetail orderDetail);

        Watch GetWatch(string watchId);

        void Save();
    }
}
