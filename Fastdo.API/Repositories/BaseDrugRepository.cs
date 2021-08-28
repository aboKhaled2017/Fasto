using AutoMapper;
using Fastdo.Core.Models;
using Fastdo.Core.Repository.IRpository;
using Fastdo.Core.ViewModels.BaseDrug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.API.Repositories
{
    public class BaseDrugRepository : Repository<BaseDrug>,IBaseDrugRepository
    {
        private readonly IMapper mapper;

        public BaseDrugRepository(SysDbContext context, IMapper mapper) : base(context, mapper)
        {
            this.mapper = mapper;
        }

        public async Task<BaseDrugMetaDataViewModel> GetDrugMetaDataByCode(string code)
        {
            var drug =await GetByIdAsync(code);
            return _mapper.Map<BaseDrugMetaDataViewModel>(drug);
        }

        public async Task<BaseDrugMetaDataViewModel> GetDrugMetaDataByCodeForPharmacy(string code)
        {
            var drug = await GetByIdAsync(code);
            if (drug is null) return null; ;
            var data= _mapper.Map<BaseDrugMetaDataViewModel>(drug);
            var hasThisDrug = _unitOfWork.LzDrugRepository.Any(d => d.Code == code && d.PharmacyId == UserId);
            data.HasThisDrug = hasThisDrug;
            return data;
        }
    }
}
