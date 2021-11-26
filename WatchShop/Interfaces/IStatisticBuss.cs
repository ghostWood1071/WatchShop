using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.Entity;
namespace WatchShop.Interfaces
{
    public interface IStatisticBuss
    {
        List<StatisResult> Statis(int year);
    }
}
