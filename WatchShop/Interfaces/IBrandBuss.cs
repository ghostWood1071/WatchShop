using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.Entity;

namespace WatchShop.Interfaces
{
    //Quản lý nhãn hàng
    public interface IBrandBuss
    {
        List<Brand> GetBrands();
        List<Brand> Search(string keyword);
        Brand GetBrand(string brandID);
        void Add(Brand brand);
        void Delete(string brandId);
        void Update(Brand brand);
        void Save();
    }
}
