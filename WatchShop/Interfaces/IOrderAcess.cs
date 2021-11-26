using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.Entity;
namespace WatchShop.Interfaces
{
    public interface IOrderAcess
    {
        List<Order> GetOrders();
        List<Order> Search(string keyword);
        Order GetOrder(string orderID);
        void Add(Order order);
        void Delete(Order order);
        void Update(Order order);
        void Save();
    }
}
