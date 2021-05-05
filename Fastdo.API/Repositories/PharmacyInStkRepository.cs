using AutoMapper;
using Fastdo.Core;
using Fastdo.Core.Enums;
using Fastdo.Core.Models;
using Fastdo.Core.Repositories;
using Fastdo.Core.ViewModels.Stocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.API.Repositories
{
    public class PharmacyInStkRepository : Repository<PharmacyInStock>, IPharmacyInStkRepository
    {
        public PharmacyInStkRepository(SysDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public void PatchUpdateRequest(PharmacyInStock request)
        {
            UpdateFields(request, prop => prop.Seen, prop => prop.PharmacyReqStatus);
        }
        public async Task<PagedList<ShowJoinedPharmaToStkModel>> GetJoinedPharmas(PharmaReqsResourceParameters _params)
        {
            var originalData = Where(r =>
                 r.StockId == UserId &&
                 (r.PharmacyReqStatus == PharmacyRequestStatus.Accepted || r.PharmacyReqStatus == PharmacyRequestStatus.Disabled));

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
                     .Where(p => p.PharmacyReqStatus == _params.Status);
            }

            var data = originalData
                .Select(r => new ShowJoinedPharmaToStkModel
                {
                    Pharma = new ShowJoinRequestToStk_pharmaDataModel
                    {
                        Id = r.PharmacyId,
                        Name = r.Pharmacy.Customer.Name,
                        AddressInDetails = r.Pharmacy.Customer.Address,
                        Address = $"{r.Pharmacy.Customer.Area.Name} / {r.Pharmacy.Customer.Area.SuperArea.Name ?? "غير محدد"}",
                        PhoneNumber = r.Pharmacy.Customer.PersPhone,
                        LandlinePhone = r.Pharmacy.Customer.LandlinePhone
                    },
                    PharmaClassId = r.Pharmacy.StocksClasses
                                                  .SingleOrDefault(s => s.StockClass.StockId == r.StockId).StockClassId,
                    Status = r.PharmacyReqStatus
                });
            return await PagedList<ShowJoinedPharmaToStkModel>.CreateAsync(data, _params);
        }
    }
}
