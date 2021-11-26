using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.Interfaces;
using WatchShop.Entity;
namespace WatchShop.Bussiness
{
    // quản lý đồng hồ
    public class WatchBuss : IWatchBuss
    {
        private IWatchAcess watchAcess;
        private IBrandAcess brandAcess;

        public WatchBuss(IWatchAcess watchAcess, IBrandAcess brandAcess)
        {
            this.watchAcess = watchAcess;
            this.brandAcess = brandAcess;
           
        }

        public void Add(Watch watch)
        {
            watchAcess.Add(watch);
        }

        public void Delete(string watchId)
        {
            Watch watch = watchAcess.GetWatch(watchId);
            if (watch == null)
                return;
            watchAcess.Delete(watch);
        }

        public WatchResult GetWatch(string watchID)
        {
            Watch watch = watchAcess.GetWatch(watchID);
            if (watch == null)
                return null;
            Brand brand = brandAcess.GetBrand(watch.BrandId);
            return new WatchResult()
            {
                WatchId = watch.WatchId,
                BrandId = watch.BrandId,
                BrandName = brand.BrandName,
                Name = watch.Name,
                Origin = watch.Origin,
                Price = watch.Price,
                Type = watch.Type,
                Quantity = watch.Quantity
            };
        }

        public List<WatchResult> GetWatches()
        {
            List<Watch> watches = watchAcess.GetWatches();
            List<Brand> brands = brandAcess.GetBrands();
            return watches.Join(brands, watch => watch.BrandId, brand => brand.BrandId, (watch, brand) => new WatchResult
            {
                WatchId = watch.WatchId,
                BrandId = watch.BrandId,
                BrandName = brand.BrandName,
                Name = watch.Name,
                Origin = watch.Origin,
                Price = watch.Price,
                Type = watch.Type,
                Quantity = watch.Quantity
            }).ToList<WatchResult>();
        }

        public List<WatchResult> GetWatches(string brandId)
        {
            Brand brand = brandAcess.GetBrand(brandId);
            if (brand == null)
                return null;
            List<Watch> watches = watchAcess.GetWatches(brandId);
           return watches.Select(watch => new WatchResult()
            {
                WatchId = watch.WatchId,
                BrandId = watch.BrandId,
                BrandName = brand.BrandName,
                Name = watch.Name,
                Origin = watch.Origin,
                Price = watch.Price,
                Type = watch.Type,
                Quantity = watch.Quantity
            }).ToList();
        }

        public void Save()
        {
            watchAcess.Save();
        }

        public List<WatchResult> Search(string keyword)
        {
            List<Watch> watches = watchAcess.Search(keyword);
            List<Brand> brands = brandAcess.GetBrands();
            return watches.Join(brands, watch => watch.BrandId, brand => brand.BrandId, (watch, brand) => new WatchResult
            {
                WatchId = watch.WatchId,
                BrandId = watch.BrandId,
                BrandName = brand.BrandName,
                Name = watch.Name,
                Origin = watch.Origin,
                Price = watch.Price,
                Type = watch.Type,
                Quantity = watch.Quantity
            }).ToList<WatchResult>();
        }

        public void Update(Watch watch)
        {
            watchAcess.Update(watch);
        }
    }
}
