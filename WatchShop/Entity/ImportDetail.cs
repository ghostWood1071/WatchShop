using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchShop.Entity
{
    public class ImportDetail
    {
        //mã chi tiết hóa đơn nhập
        public string DetailId { get; set; }
        //mã hóa đơn nhập
        public string ImportId { get; set; }
        //mã đồng hồ
        public string WatchId { get; set; }
        //số lượng nhập
        public int Quantity { get; set; }
        //giá nhập
        public float ImportPrice { get; set; }
        //giá bán
        public float SalePrice { get; set; }
        //đơn giá = ImportPricexQuantity
        public float UnitPrice { get; set; }
    }
}
