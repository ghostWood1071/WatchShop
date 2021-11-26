using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.Entity;
namespace WatchShop.Interfaces
{
    public interface IImportDetailAcess
    {
        List<ImportDetail> GetImportDetails();
        List<ImportDetail> GetImportDetails(string importID);
        List<ImportDetail> Search(string keyword);
        ImportDetail GetImportDetail(string importDetailID);
        void Add(ImportDetail importDetail);
        void Delete(ImportDetail importDetail);
        void Update(ImportDetail importDetail);
        void Save();
    }
}
