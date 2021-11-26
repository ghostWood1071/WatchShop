using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchShop.Entity
{
    public class OrderDetail
    {
        //mã chi tiết hóa đơn mua
        public string DetailId { get; set; }
        //mã hóa đơn bán
        public string OrderId { get; set; }
        //mã đồng hồ
        public string WatchId { get; set; }
        //số lượng mua
        public int Quantity { get; set; }
        //đơn giá
        public float UnitPrice { get; set; }
        //phần trăm giảm giá: vd: giảm 5%
        public float Discount { get; set; }
    }
}
