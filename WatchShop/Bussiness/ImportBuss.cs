using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.Interfaces;
using WatchShop.Entity;
namespace WatchShop.Bussiness
{
    public class ImportBuss : IImportBuss
    {
        private IWatchAcess watchAcess;
        private IImportAcess importAcess;
        private IImportDetailAcess detailAcess;
        private IBrandAcess brandAcess;

        public ImportBuss(IImportAcess importAcess, IImportDetailAcess detailAcess, IWatchAcess watchAcess, IBrandAcess brandAcess)
        {
            this.watchAcess = watchAcess;
            this.importAcess = importAcess;
            this.detailAcess = detailAcess;
            this.brandAcess = brandAcess;
        }
        public void Add(Import import, Brand brand)
        {
            Brand check = brandAcess.GetBrand(brand.BrandId);
            if (check == null)
                brandAcess.Add(brand);
            importAcess.Add(import);
        }

        public void AddDetail(ImportDetail importDetail, Watch watch)
        {
            importDetail.DetailId = Guid.NewGuid().ToString();
            Watch check = watchAcess.GetWatch(watch.WatchId);
            if(check == null)
            {
                watchAcess.Add(watch);
                detailAcess.Add(importDetail);
            }
            else
            {
                watch.Quantity += importDetail.Quantity;
                detailAcess.Add(importDetail);
            }
        }

        public void Delete(string importId)
        {
            Import import = importAcess.GetImport(importId);
            if (import == null)
                return;
            List<ImportDetail> details = detailAcess.GetImportDetails(import.ImportID);
            foreach (var d in details)
                detailAcess.Delete(d);
            importAcess.Delete(import);
        }

        public void DeleteDetail(string detailId)
        {
            ImportDetail detail = detailAcess.GetImportDetail(detailId);
            if (detail == null)
                return;
            Import import = importAcess.GetImport(detail.ImportId);
            import.TotalPrice -= detail.UnitPrice;
            detailAcess.Delete(detail);
        }

        public Brand GetBrand(string brandId)
        {
            return brandAcess.GetBrand(brandId);
        }

        public ImportResult GetImport(string importID)
        {
            Import import = importAcess.GetImport(importID);
            if (import == null)
                return null;
            Brand brand = brandAcess.GetBrand(import.BrandID);
            return new ImportResult
            {
                BrandID = brand==null?"":brand.BrandId,
                BrandName = brand == null ? "" : brand.BrandName,
                ImportDate = import.ImportDate,
                ImportID = import.ImportID,
                TotalPrice = import.TotalPrice
            };
        }

        public ImportDetailResult GetImportDetail(string importDetailId)
        {
            ImportDetail detail = detailAcess.GetImportDetail(importDetailId);
            if (detail == null)
                return null;
            Watch watch = GetWatch(detail.WatchId);
            return new ImportDetailResult
            {
                WatchName = watch.Name,
                DetailId = detail.DetailId,
                ImportId = detail.ImportId,
                ImportPrice = detail.ImportPrice,
                Quantity = detail.Quantity,
                SalePrice = detail.SalePrice,
                UnitPrice = detail.UnitPrice,
                WatchId = detail.WatchId
            };
        }

        public List<ImportDetailResult> GetImportDetails(string importId)
        {
            List<ImportDetail> details = detailAcess.GetImportDetails(importId);
            List<Watch> watches = watchAcess.GetWatches();
            var query = from detail in details
                        join watch in watches on detail.WatchId equals watch.WatchId
                        into gj
                        from d in gj.DefaultIfEmpty()
                        select new ImportDetailResult
                        {
                            DetailId = detail.DetailId,
                            ImportId = detail.ImportId,
                            ImportPrice = detail.ImportPrice,
                            Quantity = detail.Quantity,
                            SalePrice = detail.SalePrice,
                            UnitPrice = detail.UnitPrice,
                            WatchId = detail.WatchId,
                            WatchName = d == null ? "" : d.Name
                        };
            return query.ToList();
        }

        public List<ImportResult> GetImports()
        {
            List<Import> imports = importAcess.GetImports();
            List<Brand> brands = brandAcess.GetBrands();
            var query = from i in imports
                        join b in brands on i.BrandID equals b.BrandId
                        into gj
                        from im in gj.DefaultIfEmpty()
                        select new ImportResult
                        {
                            BrandID = i.BrandID,
                            BrandName = im == null ? "" : im.BrandName,
                            ImportDate = i.ImportDate,
                            ImportID = i.ImportID,
                            TotalPrice = i.TotalPrice
                        };
            return query.ToList();
        }

        public Watch GetWatch(string watchId)
        {
            return watchAcess.GetWatch(watchId);
        }

        public void Save()
        {
            brandAcess.Save();
            watchAcess.Save();
            detailAcess.Save();
            importAcess.Save();
        }

        public List<ImportResult> Search(string keyword)
        {
            List<Import> imports = importAcess.Search(keyword);
            List<Brand> brands = brandAcess.GetBrands();
            var query = from i in imports
                        join b in brands on i.BrandID equals b.BrandId
                        into gj
                        from im in gj.DefaultIfEmpty()
                        select new ImportResult
                        {
                            BrandID = i.BrandID,
                            BrandName = im == null ? "" : im.BrandName,
                            ImportDate = i.ImportDate,
                            ImportID = i.ImportID,
                            TotalPrice = i.TotalPrice
                        };
            return query.ToList();
        }

        public void Update(Import import)
        {
            importAcess.Update(import);
        }

        public void UpdateDetail(ImportDetail importDetail)
        {
            Import import = importAcess.GetImport(importDetail.ImportId);
            Watch watch = watchAcess.GetWatch(importDetail.WatchId);
            ImportDetail detail = detailAcess.GetImportDetail(importDetail.DetailId);
            import.TotalPrice -= (detail.UnitPrice- importDetail.UnitPrice);
            watch.Quantity -= (detail.Quantity - importDetail.Quantity);
            detail.UnitPrice = importDetail.Quantity * importDetail.ImportPrice;
            detailAcess.Update(importDetail);
        }
    }
}
