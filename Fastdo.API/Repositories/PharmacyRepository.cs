using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.Core.ViewModels;
using astdo.Core.ViewModels.Pharmacies;
using Fastdo.Core.Enums;
using System.Collections;
using Fastdo.Core.Repositories;
using Fastdo.Core;
using AutoMapper;
using Fastdo.Core.ViewModels.PharmaciesModels;

namespace Fastdo.API.Repositories
{
    public class PharmacyRepository:Repository<Pharmacy>,IPharmacyRepository
    {
        #region override functions
        public override IQueryable<Pharmacy> GetAll()
        {
            return base.GetAll().Include(e => e.Customer);
        }
        public override Pharmacy GetById<T>(T id)
        {
            return Entities.Include(e => e.Customer).FirstOrDefault(e => e.CustomerId == id.ToString());
        }
        public async override Task<Pharmacy> GetByIdAsync<T>(T id)
        {
            return await Entities.Include(e => e.Customer).FirstOrDefaultAsync(e => e.CustomerId == id.ToString());
        }
        #endregion
        public PharmacyRepository(SysDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public async Task<PagedList<Get_PageOf_Pharmacies_ADMModel>> Get_PageOf_PharmacyModels_ADM(PharmaciesResourceParameters _params)
        {

            var sourceData = Entities
            .OrderBy(d => d.Customer.Name)
            .Select(p => new Get_PageOf_Pharmacies_ADMModel
            {
                Id = p.CustomerId,
                MgrName = p.MgrName,
                Email=p.Customer.User.Email,
                Name = p.Customer.Name,
                OwnerName = p.OwnerName,
                PersPhone = p.Customer.PersPhone,
                LandlinePhone = p.Customer.LandlinePhone,
                LicenseImgSrc = p.LicenseImgSrc,
                CommercialRegImgSrc = p.CommercialRegImgSrc,
                Status = p.Status,
                Address = $"{p.Customer.Area.SuperArea.Name}/{p.Customer.Area.Name}",
                AreaId = p.Customer.AreaId,
                joinedStocksCount = p.GoinedStocks.Count,
                lzDrugsCount = p.LzDrugs.Count,
                requestedDrugsCount = p.RequestedLzDrugs.Count
            });
            if (!string.IsNullOrEmpty(_params.S))
            {
                var searchQueryForWhereClause = _params.S.Trim().ToLowerInvariant();
                sourceData = sourceData
                     .Where(d => d.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }
            if (_params.Status != null)
            {
                sourceData = sourceData
                     .Where(p=>p.Status==_params.Status);
            }
            return await PagedList<Get_PageOf_Pharmacies_ADMModel>.CreateAsync(sourceData, _params);
        }
        public async Task<Get_PageOf_Pharmacies_ADMModel> Get_PharmacyModel_ADM(string id)
        {
            return await 
                 Where(p=>p.CustomerId==id)
                .Select(p => new Get_PageOf_Pharmacies_ADMModel
            {
                Id=p.CustomerId,
                Email=p.Customer.User.Email,
                MgrName=p.MgrName,
                Name=p.Customer.Name,
                OwnerName=p.OwnerName,
                PersPhone=p.Customer.PersPhone,
                LandlinePhone=p.Customer.LandlinePhone,
                LicenseImgSrc=p.LicenseImgSrc,
                CommercialRegImgSrc=p.CommercialRegImgSrc,
                Status=p.Status,
                Address = $"{p.Customer.Area.SuperArea.Name}/{p.Customer.Area.Name}",
                AreaId =p.Customer.AreaId,
                joinedStocksCount=p.GoinedStocks.Count,
                lzDrugsCount=p.LzDrugs.Count,
                requestedDrugsCount=p.RequestedLzDrugs.Count               
                })
               .SingleOrDefaultAsync();
        }
       
        public async Task<bool> UpdateAsync(Pharmacy pharmacy)
        {
            _context.Entry(pharmacy).State = EntityState.Modified;
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task Delete(Pharmacy pharm)
        {                   
            _context.LzDrugRequests.RemoveRange(
                _context.LzDrugRequests.Where(r => pharm.CustomerId == r.PharmacyId || pharm.CustomerId == r.LzDrug.PharmacyId)
            );
            await _context.SaveChangesAsync();            
            _context.Users.Remove(await _context.Users.FindAsync(pharm.CustomerId));
        }
        public void UpdatePhone(Pharmacy pharmacy)
        {
            UpdateFields(pharmacy, prop => prop.Customer.PersPhone);
        }
        public void UpdateName(Pharmacy pharmacy)
        {
            var c = pharmacy.Customer;
            UpdateFields(c, prop => prop.Name);
        }
        public void UpdateContacts(Pharmacy pharmacy)
        {
            UpdateFields(pharmacy,
                prop => prop.Customer.LandlinePhone,
                prop => prop.Customer.Address);
        }
        public async Task<bool> Patch_Apdate_ByAdmin(Pharmacy pharm)
        {
            return await UpdateFieldsAsync_And_Save(pharm,prop => prop.Status);
        }

        //will be moved from here/not suitable
        public async Task<List<ShowSentRequetsToStockByPharmacyModel>> GetSentRequestsToStocks()
        {
          return await _context.PharmaciesInStocks
                 .Where(r => r.PharmacyId == UserId && r.PharmacyReqStatus!=PharmacyRequestStatus.Accepted)
                .Select(r => new ShowSentRequetsToStockByPharmacyModel
                {
                    StockId = r.StockClass.StockId,
                    Seen = r.Seen,
                    Status = r.PharmacyReqStatus,
                    Address = $"{r.StockClass.Stock.Customer.Area.SuperArea.Name}/{r.StockClass.Stock.Customer.Area.Name}",
                    AddressInDetails=r.StockClass.Stock.Customer.Address,
                    LandLinePhone=r.StockClass.Stock.Customer.LandlinePhone,
                    Name=r.StockClass.Stock.Customer.Name,
                    PersPhone=r.StockClass.Stock.Customer.PersPhone
                }).ToListAsync();
        }

        //will be moved from here/not suitable
        public async Task<IEnumerable<ShowJoinedStocksOfPharmaModel>> GetUserJoinedStocks()
        {
            return await _context.PharmaciesInStocks
                .Where(s => 
                   s.PharmacyId == UserId &&
                   (s.PharmacyReqStatus == PharmacyRequestStatus.Accepted || s.PharmacyReqStatus==PharmacyRequestStatus.Disabled))
                .Select(s => new ShowJoinedStocksOfPharmaModel{ 
                 StockId=s.StockClass.StockId,
                 Address = $"{s.StockClass.Stock.Customer.Area.SuperArea.Name} / {s.StockClass.Stock.Customer.Area.Name}",
                 AddressInDetails=s.StockClass.Stock.Customer.Address,
                 Name =s.StockClass.Stock.Customer.Name,
                 PhoneNumber=s.StockClass.Stock.Customer.PersPhone,
                 LandeLinePhone=s.StockClass.Stock.Customer.LandlinePhone
                })
                .ToListAsync();
        }

        //will be moved from here/not suitable
        public async Task<List<List<string>>> GetPharmaClassesOfJoinedStocks(string pharmaId)
        {
                 return await _context.PharmaciesInStocks
                .Where(r => r.PharmacyId == pharmaId)
                .Select(r => new List<string> { r.StockClass.StockId, r.Pharmacy.JoinedStocks.SingleOrDefault(s=>s.StockClass.StockId==r.StockClass.StockId).StockClass.ClassName??string.Empty})
                .ToListAsync();
        }

        public string getAddress(string customerID)
        {
            return Entities
               .Where(e => e.CustomerId == customerID)
               .Select(e => $"{e.Customer.Area.Name} / {e.Customer.Area.SuperArea.Name}")
               .FirstOrDefault();
        }

        public async Task<IReadOnlyList<GetPharmaSmallDataViewModel>> GetAllPharmaciesWithShortData()
        {
            return await GetAll()
                .Select(e => new GetPharmaSmallDataViewModel
                {
                    Id=e.CustomerId,
                    Name=e.Customer.Name,
                    Address=$"{e.Customer.Area.Name} / {e.Customer.Area.SuperArea.Name}",
                    DrugsCount=e.LzDrugs.Count
                })
                .ToListAsync();
        }
    }
}
