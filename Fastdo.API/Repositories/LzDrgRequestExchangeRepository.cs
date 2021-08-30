using AutoMapper;
using Fastdo.Core;
using Fastdo.Core.Dtos;
using Fastdo.Core.Enums;
using Fastdo.Core.Models;
using Fastdo.Core.Repositories;
using Fastdo.Core.Repository.IRpository;
using Fastdo.Core.ViewModels;
using Fastdo.Core.ViewModels.PhrDrgExchangeRequestsModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.API.Repositories
{
    public class LzDrgRequestExchangeRepository : Repository<LzDrugExchangeRequest>, ILzDrgRequestExchangeRepository
    {
        public LzDrgRequestExchangeRepository(SysDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
        #region Requester
        public LzDrugExchangeRequest AddExchangeRequest(LzDrugLzDrugExchangeRequestAddInputDto lzDrugLzDrug)
        {
            var exchangeRequest = new LzDrugExchangeRequest
            {
                PharmacyId = UserId,
                LzDrugLzDrugExchangeRequests = new List<LzDrugLzDrugExchangeRequest>()
            };
            foreach (var item in lzDrugLzDrug.LzDrugsIds )
            {
                exchangeRequest.LzDrugLzDrugExchangeRequests.Add(new LzDrugLzDrugExchangeRequest()
                {
                    LzDrugId=item
                });
            }

            _context.LzDrugExchangeRequests.Add(exchangeRequest);
            _context.SaveChanges();
            return exchangeRequest;
        }


        public void DeleteExchangeRequest(Guid id)
        {
            var entity = _context.LzDrugExchangeRequests
                 .Include(lz => lz.LzDrugLzDrugExchangeRequests)
                 .ThenInclude(ex => ex.LzDrug).ThenInclude(d => d.Pharmacy).ThenInclude(p => p.Customer)
                 .FirstOrDefault(lz => lz.Id == id);
          
            if (entity == null)
                throw new ArgumentNullException(nameof(_context.LzDrugExchangeRequests));
            _context.LzDrugExchangeRequests.Remove(entity);
            _context.SaveChanges();
        }

        public LzDrugExchangeRequest EditExchangeRequest(LzExchangeRequestEditDto lzDrugLzDrug)
        {
            var entity = _context.LzDrugExchangeRequests
               .Include(lz => lz.LzDrugLzDrugExchangeRequests)
               .ThenInclude(ex => ex.LzDrug).ThenInclude(d => d.Pharmacy).ThenInclude(p => p.Customer)
               .FirstOrDefault(lz => lz.Id == lzDrugLzDrug.Id);

            if (entity == null)
                throw new ArgumentNullException();
          
           
            if (lzDrugLzDrug.LzDrugsIds != null)
            {
                entity.LzDrugLzDrugExchangeRequests.Clear();
                entity.LzDrugLzDrugExchangeRequests = new List<LzDrugLzDrugExchangeRequest>();
                foreach (var item in lzDrugLzDrug.LzDrugsIds)
                {
                    entity.LzDrugLzDrugExchangeRequests.Add(new LzDrugLzDrugExchangeRequest()
                    {
                        LzDrugId = item
                    });
                }

            }
            _context.SaveChanges();
            return entity;
        }

        public async Task<PagedList<Made_LzDrgeExchangeRequest_MB>> Get_AllRequests_I_Made(LzDrgReqResourceParameters _params)
        {

            var data = GetAll().Include(c => c.LzDrugLzDrugExchangeRequests).ThenInclude(c => c.LzDrug)
                .Where(lz => lz.PharmacyId == UserId).Select(lz => new Made_LzDrgeExchangeRequest_MB
                {

                     Id = lz.Id,
                    RequesterPharmacyName = lz.Pharmacy.Customer.Name,
                    Status = lz.Status,
                    CountOfRequestedLzDrugs = lz.LzDrugLzDrugExchangeRequests.Count(),
                    ReceiverPharmacy = _context.Pharmacies.Include(c => c.Customer)
                    .Where(c => c.LzDrugs.Any(v => lz.LzDrugLzDrugExchangeRequests.Any(x => x.LzDrugId == v.Id)))
                   .FirstOrDefault().Customer.Name


                }) ;
           
            return await PagedList<Made_LzDrgeExchangeRequest_MB>.CreateAsync(data, _params);
        }

        public Task<Made_LzDrgeExchangeRequest_MB> Get_Request_I_MadeById(Guid id)
        {
            var data = _context.LzDrugExchangeRequests
              .Where(lz => lz.Id == id)
              .Select(lz => new Made_LzDrgeExchangeRequest_MB
              {

                  Id = lz.Id,
                  Status = lz.Status,
                  CountOfRequestedLzDrugs = lz.LzDrugLzDrugExchangeRequests.Count(),
                  ReceiverPharmacy = _context.Pharmacies.Include(c => c.Customer)
                    .Where(c => c.LzDrugs.Any(v => lz.LzDrugLzDrugExchangeRequests.Any(x => x.LzDrugExchangeRequestId == v.Id)))
                   .FirstOrDefault().Customer.Name,
                  RequesterPharmacyName=lz.Pharmacy.Customer.Name,
                  RequestedDrugs = _context.LzDrugs
                   .Where(c => c.LzDrugLzDrugExchangeRequests.Any(l => l.LzDrugExchangeRequestId == id))
                   .Select(d=>new LzDrugeDetailsDto
                   {
                       id=d.Id,
                       Name=d.Name,
                       Price=d.Price,
                       Type=d.Type,
                       ValideDate=d.ValideDate,
                       Description=d.Desc,
                       Status=lz.LzDrugLzDrugExchangeRequests.Where(x=>x.LzDrugId==d.Id).FirstOrDefault().Status
                   }).ToList()

              }).FirstOrDefaultAsync();
            return data;
        }
        #endregion
       
        #region Receiver
      
        public async Task<PagedList<Made_LzDrgeExchangeRequest_MB>> Get_AllRequests_I_Received(LzDrgReqResourceParameters _params)
        {
            var data = _context.LzDrugExchangeRequests
                .Include(lz => lz.LzDrugLzDrugExchangeRequests)
                .Where(lz => lz.LzDrugLzDrugExchangeRequests.Any(l=>l.LzDrug.PharmacyId == UserId)).Select(lz => new Made_LzDrgeExchangeRequest_MB
                {
                    
                    Id = lz.Id,
                   RequesterPharmacyName  = lz.Pharmacy.Customer.Name,
                    Status = lz.Status,
                    CountOfRequestedLzDrugs = lz.LzDrugLzDrugExchangeRequests.Count()
                    //ReceiverPharmacy =lz.Pharmacy.Customer.Name,
                    // LzDrugExchangeRequests = lz.LzDrugLzDrugExchangeRequests,


                });
            return await PagedList<Made_LzDrgeExchangeRequest_MB>.CreateAsync(data, _params);
        }
        public LzDrugExchangeRequest UpdateExchangeRequestStatusIReceived(ExchangeRequestBaseDto lzDrugLzDrug)
        {
            var entity = _context.LzDrugExchangeRequests
               .Include(lz => lz.LzDrugLzDrugExchangeRequests)
               .ThenInclude(ex => ex.LzDrug).ThenInclude(d => d.Pharmacy).ThenInclude(p => p.Customer)
               .Where(lz => lz.Id == lzDrugLzDrug.Id)
               .Where(lz => lz.LzDrugLzDrugExchangeRequests.Any(l => l.LzDrug.PharmacyId == UserId)).FirstOrDefault();

            if (entity == null)
                throw new ArgumentNullException();

             entity.Status = lzDrugLzDrug.Status;
            _context.SaveChanges();
            return entity;
        }
        public LzDrugExchangeRequest UpdateDrugStatusINRequestIReceived(LzLzDrugExchangeReuestUpdateDurgeStatusInReceivedRequestDto lzDrugLzDrug)
        {
            var entity = _context.LzDrugExchangeRequests.Include(c=>c.LzDrugLzDrugExchangeRequests)
                          .Where(lz => lz.LzDrugLzDrugExchangeRequests.Any(l => l.LzDrug.PharmacyId == UserId)).FirstOrDefault();

            if (entity == null)
                throw new ArgumentNullException();
            var drug = entity.LzDrugLzDrugExchangeRequests.FirstOrDefault(d => d.LzDrugId == lzDrugLzDrug.DrugId);
            if (drug == null)
                throw new ArgumentNullException();
            drug.Status = lzDrugLzDrug.DrugeStatus;
            if (drug.Status==LzDrugRequestExchangeRequestStatus.Accepted)
            {
                var drugindb = _context.LzDrugs.Find(drug.LzDrugId);
                drugindb.Exchanged = true; 

            }
          
            _context.SaveChanges();
            return entity;
         
        }

        public LzDrugExchangeRequest AddExchangeRequestRecevied(LzDrugeExchangeAddRequestToRecivedRequestDto lzDrugLzDrug)
        {
            var exchangeRequest = new LzDrugExchangeRequest
            {
                PharmacyId = UserId,
                LzDrugLzDrugExchangeRequests = new List<LzDrugLzDrugExchangeRequest>()
                
            };
            double sum = 0;
            foreach (var item in lzDrugLzDrug.LzDrugsIds)
            {
                exchangeRequest.LzDrugLzDrugExchangeRequests.Add(new LzDrugLzDrugExchangeRequest()
                {
                    LzDrugId = item
                });
                sum += _context.LzDrugs.Where(c => c.Id == item).Sum(c => c.Price);
            }

            var totaOrder = sum;
          var TolalRceivedOrder=  GetRequestTotalPrice(lzDrugLzDrug.RequestIReceviedId);
            if (totaOrder == TolalRceivedOrder||totaOrder== TolalRceivedOrder + (TolalRceivedOrder * 10) / 100 || totaOrder == TolalRceivedOrder - (TolalRceivedOrder * 10) / 100)
            {
                _context.LzDrugExchangeRequests.Add(exchangeRequest);
                _context.SaveChanges();
            }
               else
            {
                throw new Exception("يجب ان يكون اجمالى الطلب مساوى لاجمالى الطلب المستقبل بأقل او اكثر 10%");

            }


            return exchangeRequest;
        }
        private double GetRequestTotalPrice(Guid RequestId)
        {
            var entity = _context.LzDrugExchangeRequests
               .Include(lz => lz.LzDrugLzDrugExchangeRequests)
               .ThenInclude(ex => ex.LzDrug).ThenInclude(d => d.Pharmacy).ThenInclude(p => p.Customer)
               .Where(lz => lz.Id == RequestId)
               .Where(lz => lz.LzDrugLzDrugExchangeRequests.Any(l => l.LzDrug.PharmacyId == UserId)).FirstOrDefault();

            if (entity == null)
                throw new ArgumentNullException();

            var SumAcceptedDrugs = entity.LzDrugLzDrugExchangeRequests.Where(i => i.Status == LzDrugRequestExchangeRequestStatus.Accepted).Sum(i => i.LzDrug.Price);


            return SumAcceptedDrugs;

        }
        #endregion


    }
}

