using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchShop.Entity
{
    public class Order
    {
        //mã hóa đơn
        public string OrderID { get; set; }
        //ngày mua
        public DateTime OrderDate { get; set; }
        //thông tin khách hàng
        public string Customer { get; set; }
        //tổng tiền
        public float TotalPrice { get; set; }
    }
}
