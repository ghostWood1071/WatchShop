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
    public class ImportDataAcess : IImportAcess
    {
        private string path;
        private List<Import> imports;
        private IDataHelper helper;

        public ImportDataAcess(string path, IDataHelper helper)
        {
            this.path = path;
            this.helper = helper;
            this.imports = new List<Import>();
        }
        public void Add(Import import)
        {
            imports.Add(import);
        }

        public void Delete(Import import)
        {
            imports.Remove(import);
        }

        public Import GetImport(string importID)
        {
            foreach (var i in imports)
                if (i.ImportID == importID)
                    return i;
            return null;
        }

        public List<Import> GetImports()
        {
            if (this.imports.Count == 0)
                this.imports = helper.Convert<Import>(helper.ReadFile(path));
            return this.imports;
        }

        public void Save()
        {
            helper.WriteFile<Import>(path,imports);
        }

        public List<Import> Search(string keyword)
        {
            List<Import> result = new List<Import>();
            foreach(var i in imports)
            {
                if (i.BrandID.Contains(keyword) || i.ImportID.Contains(keyword) || i.TotalPrice.ToString().Contains(keyword) || i.ImportDate.ToString("dd/MM/yyyy").Contains(keyword))
                    result.Add(i);
            }
            return result;
        }

        public void Update(Import import)
        {
            Import old = GetImport(import.ImportID);
            if (old == null)
                return;
            old.BrandID = import.BrandID;
            old.ImportDate = import.ImportDate;
            old.TotalPrice = import.TotalPrice;
        }
    }
}
