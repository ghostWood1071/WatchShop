using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.Entity;
namespace WatchShop.Interfaces
{
    public interface IOrderDetailAcess
    {
        List<OrderDetail> GetOrderDetails();
        List<OrderDetail> GetOrderDetails(string orderID);
        List<OrderDetail> Search(string keyword);
        OrderDetail GetOrderDetail(string orderDetailID);
        void Add(OrderDetail orderDetail);
        void Delete(OrderDetail orderDetail);
        void Update(OrderDetail orderDetail);
        void Save();
    }
}
