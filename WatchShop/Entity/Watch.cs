using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchShop.Entity
{
    public class Watch
    {
        //mã nhãn hàng
        public string BrandId { get; set; }
        //mã đồng hồ
        public string WatchId { get; set; }
        //tên
        public string Name { get; set; }
        //giá tiền
        public float Price { get; set; }
        //xuất xứ
        public string Origin { get; set; }
        //Số lượng tồn
        public int Quantity { get; set; }
        //cho nam hay cho nữ: true = nam, false = nữ
        public bool Type { get; set; }

    }
}
