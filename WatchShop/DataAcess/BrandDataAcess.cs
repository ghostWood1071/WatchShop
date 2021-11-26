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
    public class BrandDataAcess: IBrandAcess
    {
        private IDataHelper helper;
        private List<Brand> brands;
        private string path;
        public BrandDataAcess(string path, IDataHelper helper)
        {
            this.helper = helper;
            brands = new List<Brand>();
            this.path = path;
        }

        public void Add(Brand brand)
        {
            brands.Add(brand);
        }

        public void Delete(Brand brand)
        {
            brands.Remove(brand);
        }

        public Brand GetBrand(string brandID)
        {
            foreach (var b in brands)
                if (b.BrandId == brandID)
                    return b;
            return null;
        }

        public List<Brand> GetBrands()
        {
            if (brands.Count == 0)
                brands = helper.Convert<Brand>(helper.ReadFile(this.path));
            return brands;
        }

        public void Save()
        {
            helper.WriteFile<Brand>(this.path, brands);
        }

        public List<Brand> Search(string keyword)
        {
            List<Brand> result = new List<Brand>();
            foreach(var b in brands)
            {
                if (b.BrandName.Contains(keyword) || b.BrandId.Contains(keyword))
                    result.Add(b);
            }
            return result;
        }

        public void Update(Brand brand)
        {
            Brand old = GetBrand(brand.BrandId);
            if (old == null)
                return;
            old.BrandName = brand.BrandName;
        }
    }
}
