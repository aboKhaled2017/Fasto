using Fastdo.Core.Dtos;
using Fastdo.Core.Models;
using Fastdo.Core.ViewModels;
using Fastdo.Core.ViewModels.PhrDrgExchangeRequestsModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fastdo.Core.Repository.IRpository
{
  public  interface ILzDrgRequestExchangeRepository : IRepository<LzDrugExchangeRequest>
    {
        //Requester
        Task<PagedList<Made_LzDrgeExchangeRequest_MB>> Get_AllRequests_I_Made(LzDrgReqResourceParameters _params);
    //    LzDrugExchangeRequest AddExchangeRequest(LzDrugLzDrugExchangeRequestAddInputDto lzDrugLzDrug);
        LzDrugExchangeRequest EditExchangeRequest(LzExchangeRequestEditDto lzDrugLzDrug);
        void DeleteExchangeRequest(Guid id);
        Task<Made_LzDrgeExchangeRequest_MB> Get_Request_I_MadeById(Guid id);

        LzDrugExchangeRequest AddExchangeRequest(LzDrugLzDrugExchangeRequestAddInputDto lzDrugLzDrug);
        // Receiver
        Task<PagedList<Made_LzDrgeExchangeRequest_MB>> Get_AllRequests_I_Received(LzDrgReqResourceParameters _params);
    //    LzDrugExchangeRequest AddExchangeRequestRecevied(LzDrugeExchangeAddRequestToRecivedRequestDto lzDrugLzDrug);
        LzDrugExchangeRequest UpdateDrugStatusINRequestIReceived(LzLzDrugExchangeReuestUpdateDurgeStatusInReceivedRequestDto lzDrugLzDrug);
            LzDrugExchangeRequest UpdateExchangeRequestStatusIReceived(ExchangeRequestBaseDto lzDrugLzDrug);

        //LzDrugExchangeRequest AddExchangeRequest(Guid drugId);
        //void Delete(LzDrugExchangeRequest lzDrugExchangeRequest);

        ////Receiver
        //Task<PagedList<Received_Lz_DrgExchangeRequest>> Get_AllRequests_I_Received(LzDrgReqResourceParameters _params);
        //Task<bool> Patch_Update_DrgExchangeRequestStatus_Sync(LzDrugExchangeRequest lzDrugExchangeRequest);

    }
}
