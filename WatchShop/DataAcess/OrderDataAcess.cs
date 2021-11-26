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
    public class OrderDataAcess: IOrderAcess
    {
        private string path;
        private List<Order> orders;
        private IDataHelper helper;

        public OrderDataAcess(string path, IDataHelper helper)
        {
            this.path = path;
            this.helper = helper;
            this.orders = new List<Order>();
        }

        public void Add(Order order)
        {
            orders.Add(order);
        }

        public void Delete(Order order)
        {
            orders.Remove(order);
        }

        public Order GetOrder(string orderID)
        {
            foreach (var o in orders)
                if (o.OrderID == orderID)
                    return o;
            return null;
        }

        public List<Order> GetOrders()
        {
            if (orders.Count == 0)
                orders = helper.Convert<Order>(helper.ReadFile(path));
            return orders;
        }

        public void Save()
        {
            helper.WriteFile<Order>(path, orders);
        }

        public List<Order> Search(string keyword)
        {
            List<Order> result = new List<Order>();
            foreach (var o in orders)
                if (o.Customer.Contains(keyword) ||
                    o.OrderDate.ToString("dd/MM/yyyy").Contains(keyword) ||
                    o.TotalPrice.ToString().Contains(keyword))
                    result.Add(o);
            return result;
        }

        public void Update(Order order)
        {
            Order old = GetOrder(order.OrderID);
            if (old == null)
                return;
            old.Customer = order.Customer;
            old.OrderDate = order.OrderDate;
            old.TotalPrice = order.TotalPrice;
        }
    }
}
