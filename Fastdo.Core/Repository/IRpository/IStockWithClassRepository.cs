using Fastdo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.Core;
using Fastdo.Core.ViewModels.Stocks;
using Fastdo.Core.ViewModels;
using Fastdo.Core.ViewModels.StockClasseModels;

namespace Fastdo.Core.Repositories
{
    public interface IStockWithClassRepository:IRepository<StockWithPharmaClass>
    {
        StockWithPharmaClass AddNewClass(string newClass);

        void RemoveClass(DeleteStockClassForPharmaModel model, Action<object> SendError = null);

        Task UpdateClass(UpdateStockClassForPharmaModel model);

        bool HasClassName(string className);
        Task<IReadOnlyList<GetStockClassViewModel>> GetStockClasses();
        int ClassesCount();
        void UpdateClassDiscount(UpdateStockClassDiscountModel model);
        bool HasClass(Guid getNewClassId);
        void AssignAnotherClassForPharmacy(AssignAnotherClassForPharmacyModel model, Action<dynamic> onError);
    }
}
