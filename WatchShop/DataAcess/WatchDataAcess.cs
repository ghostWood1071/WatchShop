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
    public class WatchDataAcess : IWatchAcess
    {
        private string path;
        private IDataHelper helper;
        private List<Watch> watches;
        public WatchDataAcess(string path, IDataHelper helper)
        {
            this.path = path;
            this.helper = helper;
            this.watches = new List<Watch>();
        }

        public void Add(Watch watch)
        {
            watches.Add(watch);
        }

        public void Delete(Watch watch)
        {
            watches.Remove(watch);
        }

        public Watch GetWatch(string watchID)
        {
            foreach (var w in watches)
                if (w.WatchId == watchID)
                    return w;
            return null;
        }

        public List<Watch> GetWatches()
        {
            if (watches.Count == 0)
                watches = helper.Convert<Watch>(helper.ReadFile(path));
            return watches;
        }

        public List<Watch> GetWatches(string brandId)
        {
            List<Watch> result = new List<Watch>();
            foreach (var w in watches)
                if (w.BrandId == brandId)
                    result.Add(w);
            return result;
        }

        public void Save()
        {
            helper.WriteFile<Watch>(path, watches);
        }

        public List<Watch> Search(string keyword)
        {
            List<Watch> result = new List<Watch>();
            foreach (var w in watches)
                if (w.Type.ToString().Contains(keyword) ||
                    w.Name.Contains(keyword) ||
                    w.Origin.Contains(keyword) ||
                    w.Price.ToString().Contains(keyword))
                    result.Add(w);
            return result;
        }

        public void Update(Watch watch)
        {
            Watch old = GetWatch(watch.WatchId);
            if (old == null)
                return;
            old.Type = watch.Type;
            old.Origin = watch.Origin;
            old.Price = watch.Price;
            old.Name = watch.Name;
            old.BrandId = watch.BrandId;
            old.Quantity = watch.Quantity;
        }
    }
}
