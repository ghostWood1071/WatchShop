using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchShop.Entity
{
    public class Import
    {
        //mã hóa đơn nhập
        public string ImportID { get; set; }
        //hãng nhập
        public string BrandID { get; set; }
        //ngày nhập
        public DateTime ImportDate { get; set; }
        //tổng giá
        public float TotalPrice { get; set; }

    }
}
