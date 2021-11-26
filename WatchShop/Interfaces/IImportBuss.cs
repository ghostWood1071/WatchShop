using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.Entity;
namespace WatchShop.Interfaces
{
    //Quản lý nhập hàng
    public interface IImportBuss
    {
        List<ImportResult> GetImports();
        List<ImportResult> Search(string keyword);
        ImportResult GetImport(string importID);
        void Add(Import import, Brand brand);
        void Delete(string importId);
        void Update(Import import);

        List<ImportDetailResult> GetImportDetails(string importId);
        void AddDetail(ImportDetail importDetail, Watch watch );
        ImportDetailResult GetImportDetail(string importDetailId);
        void DeleteDetail(string detailId);
        void UpdateDetail(ImportDetail importDetail);

        Watch GetWatch(string watchId);

        Brand GetBrand(string brandId);

        void Save();
    }
}
