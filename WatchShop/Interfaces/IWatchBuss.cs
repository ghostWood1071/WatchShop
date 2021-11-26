using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.Entity;
namespace WatchShop.Interfaces
{
    //Quản lý đồng hồ
    public interface IWatchBuss
    {
        List<WatchResult> GetWatches();
        List<WatchResult> GetWatches(string brandId);
        List<WatchResult> Search(string keyword);
        WatchResult GetWatch(string watchID);
        void Add(Watch watch);
        void Delete(string watchId);
        void Update(Watch watch);
        void Save();
    }
}
