using Fastdo.Core.Models;
using Fastdo.Core.ViewModels.BaseDrug;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fastdo.Core.Repository.IRpository
{
    public interface IBaseDrugRepository:IRepository<BaseDrug>
    {
        Task<BaseDrugMetaDataViewModel> GetDrugMetaDataByCode(string code);
        Task<BaseDrugMetaDataViewModel> GetDrugMetaDataByCodeForPharmacy(string code);
    }
}
