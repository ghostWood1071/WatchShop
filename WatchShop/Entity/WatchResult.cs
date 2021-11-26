using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchShop.Entity
{
    // cái này không liên quan gì đến những entity chính (những cái có đuôi là Result)
    //cái này dùng để lưu dữ liệu truy vấn từ nhiều file khác nhau VD:
    //lấy thông tin đồng hồ, nhưng muốn lấy thêm cả tên hãng nữa thì tạo ra cái này
    //vừa chứa dc thông tin của đồng hồ và cả hãng nữa. 
    public class WatchResult: Watch
    {
        public string BrandName { get; set; }
    }
}
