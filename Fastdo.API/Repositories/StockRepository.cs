using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.Core.ViewModels;
using Fastdo.Core.ViewModels.Stocks.Models;
using Fastdo.Core.ViewModels.Stocks;
using Fastdo.API.Services;
using EFCore.BulkExtensions;
using Fastdo.Core.Enums;
using Namotion.Reflection;
using Newtonsoft.Json;
using astdo.Core.ViewModels.Pharmacies;
using Fastdo.Core.Repositories;
using Fastdo.Core;
using Fastdo.Core.Utilities;
using AutoMapper;

namespace Fastdo.API.Repositories
{
    public class StockRepository:Repository<Stock>,IStockRepository
    {
        public StockRepository(SysDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        #region override functions
        public override IQueryable<Stock> GetAll()
        {
            return base.GetAll().Include(e => e.Customer);
        }
        public override Stock GetById<T>(T id)
        {
            return Entities.Include(e => e.Customer).FirstOrDefault(e => e.CustomerId == id.ToString());
        }
        public async override Task<Stock> GetByIdAsync<T>(T id)
        {
            return await Entities.Include(e => e.Customer).FirstOrDefaultAsync(e => e.CustomerId == id.ToString());
        }
        #endregion
       
        public override async Task AddAsync(Stock model)
        {
            model.PharmasClasses = new List<StockWithPharmaClass>() { new StockWithPharmaClass { ClassName = "الافتراضى" } };
            await base.AddAsync(model);
            await SaveAsync();
        }
        public IQueryable GetAllAsync()
        {
            return Entities;
        }
        public async Task<PagedList<GetPageOfSearchedStocks>> GetPageOfSearchedStocks(IStockSearchResourceParameters _params)
        {
            var originalData = GetAll()
            .OrderBy(d => d.Customer.Name)
            .Where(s => s.Customer.User.EmailConfirmed && !s.GoinedPharmacies.Any(g=>g.PharmacyId==UserId));
            
            if (!string.IsNullOrEmpty(_params.S))
            {
                var searchQueryForWhereClause = _params.S.Trim().ToLowerInvariant();
                originalData = originalData
                     .Where(d => d.Customer.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            if (_params.AreaIds != null && _params.AreaIds.Count() != 0)
            {
                originalData = originalData
                .Where(d => _params.AreaIds.Any(aid => aid == d.Customer.AreaId));
            }
            else if (_params.CityIds != null && _params.CityIds.Count() != 0)
            {
                originalData = originalData
                .Where(d => _params.CityIds.Any(cid => cid == d.Customer.Area.SuperAreaId));
            }

            var selectedData= originalData
                .Select(p => new GetPageOfSearchedStocks
                {
                    Id = p.CustomerId,
                    Name = p.Customer.Name,
                    PersPhone = p.Customer.PersPhone,
                    LandlinePhone = p.Customer.LandlinePhone,
                    AddressInDetails = p.Customer.Address,
                    Address = $"{p.Customer.Area.SuperArea.Name}/{p.Customer.Area.Name}",
                    AreaId = p.Customer.AreaId,
                    joinedPharmesCount = p.GoinedPharmacies.Count,
                    drugsCount = p.SDrugs.Count
                });
            return await PagedList<GetPageOfSearchedStocks>.CreateAsync(selectedData, _params);
        }
        public async Task<bool> UpdateAsync(Stock stock)
        {
            _context.Entry(stock).State = EntityState.Modified;
            return await SaveAsync();
        }
        public async Task<PagedList<Get_PageOf_Stocks_ADMModel>> Get_PageOf_StockModels_ADM(StockResourceParameters _params)
        {
            var sourceData =GetAll()
            .OrderBy(d => d.Customer.Name)
            .Select(p => new Get_PageOf_Stocks_ADMModel
            {
                Id = p.CustomerId,
                Email=p.Customer.User.Email,
                MgrName = p.MgrName,
                Name = p.Customer.Name,
                OwnerName = p.OwnerName,
                PersPhone = p.Customer.PersPhone,
                LandlinePhone = p.Customer.LandlinePhone,
                LicenseImgSrc = p.LicenseImgSrc,
                CommercialRegImgSrc = p.CommercialRegImgSrc,
                Status = p.Status,
                Address = $"{p.Customer.Area.SuperArea.Name}/{p.Customer.Area.Name}",
                AreaId = p.Customer.AreaId,
                joinedPharmesCount = p.GoinedPharmacies.Count,
                drugsCount=p.SDrugs.Count
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
                     .Where(p => p.Status == _params.Status);
            }
            return await PagedList<Get_PageOf_Stocks_ADMModel>.CreateAsync(sourceData, _params);
        }

        public async Task<Get_PageOf_Stocks_ADMModel> Get_StockModel_ADM(string id)
        {
            return await GetAll()
                .Where(p => p.CustomerId == id)
                .Select(p => new Get_PageOf_Stocks_ADMModel
                {
                    Id = p.CustomerId,
                    MgrName = p.MgrName,
                    Name = p.Customer.Name,
                    OwnerName = p.OwnerName,
                    PersPhone = p.Customer.PersPhone,
                    LandlinePhone = p.Customer.LandlinePhone,
                    LicenseImgSrc = p.LicenseImgSrc,
                    CommercialRegImgSrc = p.CommercialRegImgSrc,
                    Status = p.Status,
                    Address = p.Customer.Address,
                    AreaId = p.Customer.AreaId,
                    joinedPharmesCount = p.GoinedPharmacies.Count,
                })
               .SingleOrDefaultAsync();
        }

        public async Task Delete(Stock stk)
        {
            _context.PharmaciesInStocks.RemoveRange(
                _unitOfWork.PharmacyInStkRepository
                .GetAll()
                .Where(ps => ps.StockId == stk.CustomerId));
            await SaveAsync();
            _context.Users.Remove(_context.Users.Find(stk.CustomerId));
        }
        public void UpdatePhone(Stock stock)
        {
            UpdateFields(stock, prop => prop.Customer.PersPhone);
        }
        public void UpdateName(Stock stock)
        {
            UpdateFields(stock, prop => prop.Customer.Name);
        }
        public void UpdateContacts(Stock stock)
        {
            UpdateFields(stock,
                prop => prop.Customer.LandlinePhone,
                prop => prop.Customer.Address);
        }
        public async Task<bool> Patch_Apdate_ByAdmin(Stock stk)
        {
            return await UpdateFieldsAsync_And_Save(stk, prop => prop.Status);
        }


        public async Task<PagedList<ShowJoinRequestToStkModel>> GetJoinRequestsPharmas(PharmaReqsResourceParameters _params)
        {
            var originalData = _unitOfWork.PharmacyInStkRepository
                .GetAll()
                 .Where(r => 
                 r.StockId == UserId &&
                 r.PharmacyReqStatus!=PharmacyRequestStatus.Accepted&&
                 r.PharmacyReqStatus!=PharmacyRequestStatus.Disabled);                
            if (!string.IsNullOrEmpty(_params.S))
            {
                var searchQueryForWhereClause = _params.S.Trim().ToLowerInvariant();
                originalData = originalData
                     .Where(d => d.Pharmacy.Customer.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }
            if (_params.Status != null)
            {
                originalData = originalData
                     .Where(p => p.PharmacyReqStatus == _params.Status);
            }
            var data = originalData
                .Select(r => new ShowJoinRequestToStkModel
                {
                    Pharma=new ShowJoinRequestToStk_pharmaDataModel
                    {
                        Id = r.PharmacyId,
                        Name = r.Pharmacy.Customer.Name,
                        AddressInDetails = r.Pharmacy.Customer.Address,
                        Address = $"{r.Pharmacy.Customer.Area.Name} / {r.Pharmacy.Customer.Area.SuperArea.Name??"غير محدد"}",
                        PhoneNumber = r.Pharmacy.Customer.PersPhone,
                        LandlinePhone = r.Pharmacy.Customer.LandlinePhone,
                    },
                    Seen = r.Seen,
                    Status = r.PharmacyReqStatus,
                    PharmaClass = r.Pharmacy.StocksClasses
                    .SingleOrDefault(s => s.StockClass.StockId == r.StockId).StockClass.ClassName,

                });
            return await PagedList<ShowJoinRequestToStkModel>.CreateAsync(data, _params);
        }
        public async Task<PagedList<ShowJoinedPharmaToStkModel>> GetJoinedPharmas(PharmaReqsResourceParameters _params)
        {
            var originalData = _unitOfWork.PharmacyInStkRepository.GetAll()
                 .Where(r =>
                 r.StockId == UserId &&
                 (r.PharmacyReqStatus==PharmacyRequestStatus.Accepted||r.PharmacyReqStatus==PharmacyRequestStatus.Disabled));
            if (!string.IsNullOrEmpty(_params.S))
            {
                var searchQueryForWhereClause = _params.S.Trim().ToLowerInvariant();
                originalData = originalData
                     .Where(d => d.Pharmacy.Customer.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }
            if (_params.PharmaClass != null)
            {
                originalData = originalData
                     .Where(p => p.Pharmacy.StocksClasses.Any(sc => sc.StockClass.StockId == p.StockId && sc.StockClass.ClassName == _params.PharmaClass));
            }
            if (_params.Status != null)
            {
                originalData = originalData
                     .Where(p => p.PharmacyReqStatus==_params.Status);
            }
            var b = await originalData.ToListAsync();
            var data = originalData
                .Select(r => new ShowJoinedPharmaToStkModel
                {
                    Pharma=new ShowJoinRequestToStk_pharmaDataModel {
                        Id = r.PharmacyId,
                        Name = r.Pharmacy.Customer.Name,
                        AddressInDetails = r.Pharmacy.Customer.Address,
                        Address = $"{r.Pharmacy.Customer.Area.Name} / {r.Pharmacy.Customer.Area.SuperArea.Name ?? "غير محدد"}",
                        PhoneNumber = r.Pharmacy.Customer.PersPhone,
                        LandlinePhone = r.Pharmacy.Customer.LandlinePhone
                    },
                    PharmaClassId = r.Pharmacy.StocksClasses
                                                  .SingleOrDefault(s => s.StockClass.StockId == r.StockId).StockClassId,
                    Status=r.PharmacyReqStatus
                });       
            return await PagedList<ShowJoinedPharmaToStkModel>.CreateAsync(data, _params);
        }
        public async Task<bool> DeletePharmacyRequest(string PharmaId)
        {
            if (!await _unitOfWork.PharmacyInStkRepository.GetAll()
                .AnyAsync(s => s.StockId == UserId && s.PharmacyId == PharmaId))
                return false;
            _unitOfWork.PharmacyInStkRepository
                 .Remove(
                await _unitOfWork.PharmacyInStkRepository.GetAll()
                .SingleOrDefaultAsync(r => r.PharmacyId == PharmaId && r.StockId == UserId));
            return await SaveAsync();
        }
        public async Task HandlePharmacyRequest(string pharmaId,Action<PharmacyInStock>OnRequestFounded)
        {
            var request =await _context.PharmaciesInStocks.SingleOrDefaultAsync(r=>r.StockId==UserId&&r.PharmacyId==pharmaId);
            if (request == null) throw new Exception("request is not found");
            OnRequestFounded(request);
             _unitOfWork.PharmacyInStkRepository.PatchUpdateRequest(request);
        }

        public async Task<List<StockClassWithPharmaCountsModel>> GetStockClassesOfJoinedPharmas(string stockId)
        {
            return await _unitOfWork.StockWithClassRepository
                .Where(s => s.StockId == stockId)
                .Select(s => new StockClassWithPharmaCountsModel
                {
                    Id=s.Id,
                    Name = s.ClassName,
                    Count = s.PharmaciesOfThatClass.Count
                }).ToListAsync();

        }

        public List<StockClassWithPharmaCountsModel> GetStockClassesOfJoinedPharmas(Stock stock)
        {
           return stock.PharmasClasses
                .Select(s => new StockClassWithPharmaCountsModel
                {
                    Id=s.Id,
                    Name = s.ClassName,
                    Count = s.PharmaciesOfThatClass.Count
                }).ToList();
        }

        public async Task<bool> MakeRequestToStock(string stockId)
        {
            if (!GetAll().Any(s => s.CustomerId == stockId && !s.GoinedPharmacies.Any(p => p.PharmacyId == UserId)))
                return false;
            _unitOfWork.PharmacyInStkRepository.Add(new PharmacyInStock
            {
                PharmacyId = UserId,
                StockId = stockId
            });

            var pharmaClassId = await _unitOfWork.StockWithClassRepository
                .Where(s => s.StockId == stockId)
                .Select(s => s.Id).FirstOrDefaultAsync();
            await SaveAsync();
            if (pharmaClassId == Guid.Empty)
            {
                pharmaClassId = Guid.NewGuid();
                var stkWithClass = new StockWithPharmaClass { ClassName = "default", Id = pharmaClassId, StockId = stockId };
                _unitOfWork.StockWithClassRepository.Add(stkWithClass);
                await SaveAsync();
            }
            _unitOfWork.PharmacyInStkClassRepository.Add(new PharmacyInStockClass
            {
                PharmacyId = UserId,
                StockClassId = pharmaClassId
            });
            return await SaveAsync();
        }
        public async Task<bool> CancelRequestToStock(string stockId)
        {
            var request =await _unitOfWork.PharmacyInStkRepository
                .GetAll()
                .SingleOrDefaultAsync(s => s.PharmacyId == UserId && s.StockId == stockId);
            if (request == null) return false;
            _unitOfWork.PharmacyInStkClassRepository
                .Where(p => p.PharmacyId == UserId && p.StockClass.StockId == stockId).BatchDelete();
            _unitOfWork.PharmacyInStkRepository.Remove(request);

            return await SaveAsync();
        }

        public async Task HandleStkDrugsPackageRequest_ForStock(Guid packageId, Action<dynamic> onProcess, Action<dynamic> onError)
        {
            var request =await _unitOfWork.StkDrgInPackagesReqsRepository.GetAll()
                .FirstOrDefaultAsync(e=>e.Package.Id==packageId && e.StkDrug.StockId==UserId);
            if (request == null)
            {
                onError(BasicUtility.MakeError("هذا الطلب غير موجود"));
                return;
            }
            onProcess(request);
            _context.Entry(request).State = EntityState.Modified;
            await SaveAsync();
        }

        public async Task<PagedList<StkDrugsPackageReqModel>> GetStkDrugsPackageRequests(StkDrugsPackageReqResourceParmaters _params)
        {
            var originalData = _unitOfWork.StkDrgInPackagesReqsRepository
                 .Where(r =>
                 r.StkDrug.StockId==UserId &&
                 r.Status!=StkDrugPackageRequestStatus.Completed);

            if (_params.Status != null)
            {
                originalData = originalData
                     .Where(p => p.Status == _params.Status);
            }
            var data = originalData
                .Select(r => new StkDrugsPackageReqModel
                {
                    StkPackageId = r.Id,
                    PackageId = r.Package.Id,
                    Status = r.Status,
                    CreatedAt = r.Package.CreateAt,
                    Pharma = new StkDrugsPackageReqModel_PharmaData
                    {
                        Id = r.Package.PharmacyId,
                        Name = r.Package.Pharmacy.Customer.Name,
                        Address = $"{r.Package.Pharmacy.Customer.Area.Name} / {r.Package.Pharmacy.Customer.Area.SuperArea.Name}",
                        AddressInDetails = r.Package.Pharmacy.Customer.Address,
                        LandLinePhone = r.Package.Pharmacy.Customer.LandlinePhone,
                        PhoneNumber = r.Package.Pharmacy.Customer.PersPhone
                    },
                    Drugs = r.Package.PackageDrugs.Select(d => new StkDrugsPackageReqModel_DrugData
                    {
                        Id = d.StkDrugId,
                        Name = d.StkDrug.Name,
                        Quantity = d.Quantity
                    })
                });
            return await PagedList<StkDrugsPackageReqModel>.CreateAsync(data, _params);
        }

        public async Task<IList<StockNameWithIdModel>> GetAllStocksNames()
        {
            return await GetAll()
                .Select(s => new StockNameWithIdModel
            {
                Id = s.CustomerId,
                Name = s.Customer.Name
            }).ToListAsync();
        }

        public string getAddress(string customerID)
        {
            return Entities
                .Where(e=>e.CustomerId==customerID)
                .Select(e => $"{e.Customer.Area.Name} / {e.Customer.Area.SuperArea.Name}")
                .FirstOrDefault();
        }
    }
}
