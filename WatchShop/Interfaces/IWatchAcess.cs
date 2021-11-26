using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.Entity;
namespace WatchShop.Interfaces
{
    public interface IWatchAcess
    {
        List<Watch> GetWatches();
        List<Watch> GetWatches(string brandId);
        List<Watch> Search(string keyword);
        Watch GetWatch(string watchID);
        void Add(Watch watch);
        void Delete(Watch watch);
        void Update(Watch watch);
        void Save();
    }
}
