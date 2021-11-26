using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.Interfaces;
using WatchShop.Entity;
namespace WatchShop.Bussiness
{
    public class BrandBuss : IBrandBuss
    {
        private IBrandAcess brandAcess;
        private IWatchAcess watchAcess;

        public BrandBuss(IBrandAcess brandAcess, IWatchAcess watchAcess)
        {
            this.brandAcess = brandAcess;
            this.watchAcess = watchAcess;
        }
        public void Add(Brand brand)
        {
            brandAcess.Add(brand);
        }

        public void Delete(string brandId)
        {
            Brand brand = brandAcess.GetBrand(brandId);
            if (brand == null)
                return;
            List<Watch> watches = watchAcess.GetWatches(brandId);
            foreach (var w in watches)
                watchAcess.Delete(w);
            brandAcess.Delete(brand);
        }

        public Brand GetBrand(string brandID)
        {
            return brandAcess.GetBrand(brandID);
        }

        public List<Brand> GetBrands()
        {
            return brandAcess.GetBrands();
        }

        public void Save()
        {
            brandAcess.Save();
        }

        public List<Brand> Search(string keyword)
        {
            return brandAcess.Search(keyword);
        }

        public void Update(Brand brand)
        {
            brandAcess.Update(brand);
        }
    }
}
