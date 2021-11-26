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
    public class ImportDetailDataAcess: IImportDetailAcess
    {
        private string path;
        private IDataHelper helper;
        private List<ImportDetail> importDetails;
        public ImportDetailDataAcess(string path, IDataHelper helper)
        {
            this.path = path;
            this.helper = helper;
            this.importDetails = new List<ImportDetail>();
        }

        public void Add(ImportDetail importDetail)
        {
            importDetails.Add(importDetail);
        }

        public void Delete(ImportDetail importDetail)
        {
            importDetails.Remove(importDetail);
        }

        public ImportDetail GetImportDetail(string importDetailID)
        {
            foreach (var i in importDetails)
                if (i.DetailId == importDetailID)
                    return i;
            return null;
        }

        public List<ImportDetail> GetImportDetails()
        {
            if (this.importDetails.Count == 0)
                this.importDetails = helper.Convert<ImportDetail>(helper.ReadFile(path));
            return importDetails;
        }

        public List<ImportDetail> GetImportDetails(string importID)
        {
            List<ImportDetail> result = new List<ImportDetail>();
            foreach (var i in importDetails)
                if (i.ImportId == importID)
                    result.Add(i);
            return result;
        }

        public void Save()
        {
            helper.WriteFile<ImportDetail>(path, importDetails);
        }

        public List<ImportDetail> Search(string keyword)
        {
            List<ImportDetail> result = new List<ImportDetail>();
            foreach(var i in importDetails)
            {
                if (i.ImportId.Contains(keyword) ||
                    i.ImportPrice.ToString().Contains(keyword) ||
                    i.Quantity.ToString().Contains(keyword) ||
                    i.UnitPrice.ToString().Contains(keyword) ||
                    i.SalePrice.ToString().Contains(keyword))
                    result.Add(i);
            }
            return result;
        }

        public void Update(ImportDetail importDetail)
        {
            ImportDetail old = GetImportDetail(importDetail.DetailId);
            if (old == null)
                return;
            old.ImportId = importDetail.ImportId;
            old.ImportPrice = importDetail.ImportPrice;
            old.Quantity = importDetail.Quantity;
            old.SalePrice = importDetail.SalePrice;
            old.UnitPrice = importDetail.UnitPrice;
            old.WatchId = importDetail.WatchId;
        }
    }
}
