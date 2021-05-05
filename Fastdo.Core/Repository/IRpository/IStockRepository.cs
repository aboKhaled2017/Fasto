﻿using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.Core.ViewModels;
using Fastdo.Core;
using Fastdo.Core.ViewModels.Stocks;
using astdo.Core.ViewModels.Pharmacies;

namespace Fastdo.Core.Repositories
{
    public interface IStockRepository:IRepository<Stock>
    {
        string getAddress(string customerID);
        Task<List<StockClassWithPharmaCountsModel>> GetStockClassesOfJoinedPharmas(string stockId);
        List<StockClassWithPharmaCountsModel> GetStockClassesOfJoinedPharmas(Stock stock);
        Task<PagedList<ShowJoinRequestToStkModel>> GetJoinRequestsPharmas(PharmaReqsResourceParameters _params);
        Task<PagedList<ShowJoinedPharmaToStkModel>> GetJoinedPharmas(PharmaReqsResourceParameters _params);
        Task HandlePharmacyRequest(string pharmaId, Action<PharmacyInStock> OnRequestFounded);
        Task<bool> DeletePharmacyRequest(string PharmaId);
        Task<bool> MakeRequestToStock(string stockId);
        Task<PagedList<GetPageOfSearchedStocks>> GetPageOfSearchedStocks(IStockSearchResourceParameters _params);
       
         Task<PagedList<Get_PageOf_Stocks_ADMModel>> Get_PageOf_StockModels_ADM(StockResourceParameters _params);
         Task<Get_PageOf_Stocks_ADMModel> Get_StockModel_ADM(string id);
         Task<bool> UpdateAsync(Stock stock);

         void UpdatePhone(Stock stock);
         void UpdateName(Stock stock);
         void UpdateContacts(Stock stock);
        Task<bool> Patch_Apdate_ByAdmin(Stock stk);
    
        Task<bool> CancelRequestToStock(string stockId);
        Task HandleStkDrugsPackageRequest_ForStock(Guid packageId, Action<dynamic> onProcess, Action<dynamic> onError);
        Task<PagedList<StkDrugsPackageReqModel>> GetStkDrugsPackageRequests(StkDrugsPackageReqResourceParmaters _params);
        Task<IList<StockNameWithIdModel>> GetAllStocksNames();
    }
}
