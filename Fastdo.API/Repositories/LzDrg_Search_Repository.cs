﻿using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.Core.ViewModels;
using Fastdo.API.Services;
using Fastdo.Core.Repositories;
using Fastdo.Core.Services;
using Fastdo.Core;
using AutoMapper;

namespace Fastdo.API.Repositories
{
    //all class will be moved from here/not suitable
    public class LzDrg_Search_Repository : Repository<LzDrug>, ILzDrg_Search_Repository
    {
        private IpropertyMappingService _propertyMappingService;
        public LzDrg_Search_Repository(SysDbContext context,IpropertyMappingService propertyMappingService, IMapper mapper) : base(context, mapper)
        {
            _propertyMappingService = propertyMappingService;
        }
        
        public async Task<PagedList<LzDrugCard_Info_BM>> Get_All_LzDrug_Cards_BMs(ILzDrg_Card_Info_BM_ResourceParameters _params)
        {
            var generalQuerableData_BeforePaging = GetAll()
                
                .Where(d => d.PharmacyId != UserId);
                
            if (!string.IsNullOrEmpty(_params.S))
            {
                var searchQueryForWhereClause = _params.S.Trim().ToLowerInvariant();
                generalQuerableData_BeforePaging = generalQuerableData_BeforePaging
                     .Where(d => d.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }
            if (_params.ValidBefore != default(DateTime))
            {
                generalQuerableData_BeforePaging = generalQuerableData_BeforePaging
                   .Where(d =>
                        (d.ValideDate.Year>_params.ValidBefore.Year)
                        ||
                        (d.ValideDate.Year==_params.ValidBefore.Year && d.ValideDate.Month>_params.ValidBefore.Month)
                          );
            }
            if(!string.IsNullOrEmpty(_params.PhramId))
            {
                generalQuerableData_BeforePaging = generalQuerableData_BeforePaging
                    .Where(d => d.PharmacyId == _params.PhramId);
            }
            else
            {
                if(_params.AreaIds!=null && _params.AreaIds.Count() != 0)
                {
                    generalQuerableData_BeforePaging = generalQuerableData_BeforePaging
                    .Where(d =>_params.AreaIds.Any(aid=>aid== d.Pharmacy.AreaId));
                }
                else if (_params.CityIds!=null&&_params.CityIds.Count()!= 0)
                {
                    generalQuerableData_BeforePaging = generalQuerableData_BeforePaging
                    .Where(d => _params.CityIds.Any(cid=>cid==d.Pharmacy.Area.SuperAreaId));
                }

            }
            var PagedData=generalQuerableData_BeforePaging
                .Select(d => new LzDrugCard_Info_BM
                {
                    Id = d.Id,
                    Name = d.Name,
                    Desc = d.Desc,
                    Discount = d.Discount,
                    PharmacyId = d.PharmacyId,
                    Price = d.Price,
                    PriceType = d.PriceType,
                    Quantity = d.Quantity,
                    Type = d.Type,
                    UnitType = d.UnitType,
                    ValideDate = d.ValideDate.Year+"-"+d.ValideDate.Month,
                    PharmName = d.Pharmacy.Name,
                    PharmLocation = d.Pharmacy.Area.SuperArea.Name + "/" + d.Pharmacy.Area.Name,
                    RequestsCount = d.RequestingPharms.Count,
                    IsMadeRequest = (d.RequestingPharms.Count > 0 && d.RequestingPharms.Any(r => r.PharmacyId == UserId)),
                    Status =
                      (d.RequestingPharms.Count > 0 && d.RequestingPharms.Any(r => r.PharmacyId == UserId))
                    ? d.RequestingPharms.FirstOrDefault(r => r.PharmacyId == UserId).Status
                    : 0,
                    RequestId =
                      (d.RequestingPharms.Count > 0 && d.RequestingPharms.Any(r => r.PharmacyId == UserId))
                    ? d.RequestingPharms.FirstOrDefault(r => r.PharmacyId == UserId).Id
                    : Guid.Empty
                });
            PagedData = PagedData
                .ApplySort(_params.OrderBy,
                  _propertyMappingService.GetPropertyMapping<LzDrugCard_Info_BM, LzDrug>());
            return await PagedList<LzDrugCard_Info_BM>.CreateAsync(PagedData, _params);
        }
    }
}
