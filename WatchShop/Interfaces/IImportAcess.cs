using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.Entity;
namespace WatchShop.Interfaces
{
    public interface IImportAcess
    {
        List<Import> GetImports();
        List<Import> Search(string keyword);
        Import GetImport(string importID);
        void Add(Import import);
        void Delete(Import import);
        void Update(Import import);
        void Save();
    }
}
