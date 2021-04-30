﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Fastdo.Core.Enums;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.Core.ViewModels;
using Fastdo.Core.Repositories;
using Fastdo.Core;
using AutoMapper;

namespace Fastdo.API.Repositories
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        /*public SysDbContext context
        {
            get { return context as SysDbContext; }
        }*/
        public AdminRepository(SysDbContext context,IMapper mapper) : base(context,mapper)
        {
        }
        public async Task<ShowAdminModel> GetAdminsShownModelById(string id)
        {
            return await GetAll().Select(a => new ShowAdminModel
            {
                Id = a.Id,
                Name = a.Name,
                SuperId = a.SuperAdminId,
                UserName = a.User.UserName,
                PhoneNumber = a.User.PhoneNumber,
                Type = _context.UserClaims.FirstOrDefault(c => c.UserId == a.Id && c.ClaimType == Variables.AdminClaimsTypes.AdminType).ClaimValue,
                Priviligs = _context.UserClaims.FirstOrDefault(c => c.UserId == a.Id && c.ClaimType == Variables.AdminClaimsTypes.Priviligs).ClaimValue
            })
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
        }
        public override void Add(Admin model)
        {
            base.Add(model);
            _context.AdminHistoryEntries.Add(new AdminHistory
            {
                IssuerId = UserId,
                OperationType = "Insert",
                ToId = model.Id,
                Desc = $"the user {UserName} with id {UserId} inserted new admin [{model.Name}]"
            });
        }
 
        public async Task<StatisShowModel> GetGeneralStatisOfSystem()
        {           
            //var queryResult = _unitOfWork.PharmacyRepository.GetAll().Select(d=>new StatisShowModel
            //{ 
            //    pharmacies=new PharmaciesStatis
            //    {
            //     total=_context.Pharmacies.Count(),
            //     disabled= _context.Pharmacies.Count(p=>p.Status==PharmacyRequestStatus.Disabled),
            //     pending = _context.Pharmacies.Count(p => p.Status == PharmacyRequestStatus.Pending),
            //     accepted = _context.Pharmacies.Count(p => p.Status == PharmacyRequestStatus.Accepted),
            //     rejected = _context.Pharmacies.Count(p => p.Status == PharmacyRequestStatus.Rejected)
            //    },
            //    stocks = new StocksStatis
            //    {
            //        total = _context.Stocks.Count(),
            //        disabled = _context.Stocks.Count(p => p.Status == StockRequestStatus.Disabled),
            //        pending = _context.Stocks.Count(p => p.Status == StockRequestStatus.Pending),
            //        accepted = _context.Stocks.Count(p => p.Status == StockRequestStatus.Accepted),
            //        rejected = _context.Stocks.Count(p => p.Status == StockRequestStatus.Rejected)
            //    },
            //    requests=new RequestsStatis
            //    {
            //        total = _context.LzDrugRequests.Count(),
            //        done = _context.LzDrugRequests.Count(s=>s.Status==LzDrugRequestStatus.Completed),
            //        pending = _context.LzDrugRequests.Count(s => s.Status == LzDrugRequestStatus.Pending),
            //        cancel = _context.LzDrugRequests.Count(s => s.Status == LzDrugRequestStatus.Rejected)
            //    },
            //    lzDrugs=new LzDrugsStatis
            //    {
            //        total=_context.LzDrugs.Count()
            //    }
            //});
            //return await queryResult.FirstAsync();
            var pharmaStatis = new PharmaciesStatis
            {
                total = _unitOfWork.PharmacyRepository.GetAll().Count(),
                disabled = _unitOfWork.PharmacyRepository.GetAll().Count(p => p.Status == PharmacyRequestStatus.Disabled),
                pending = _unitOfWork.PharmacyRepository.GetAll().Count(p => p.Status == PharmacyRequestStatus.Pending),
                accepted = _unitOfWork.PharmacyRepository.GetAll().Count(p => p.Status == PharmacyRequestStatus.Accepted),
                rejected = _unitOfWork.PharmacyRepository.GetAll().Count(p => p.Status == PharmacyRequestStatus.Rejected)
            };
            var stockStatis = new StocksStatis
            {
                total = _unitOfWork.StockRepository.GetAll().Count(),
                disabled = _unitOfWork.StockRepository.GetAll().Count(p => p.Status == StockRequestStatus.Disabled),
                pending = _unitOfWork.StockRepository.GetAll().Count(p => p.Status == StockRequestStatus.Pending),
                accepted = _unitOfWork.StockRepository.GetAll().Count(p => p.Status == StockRequestStatus.Accepted),
                rejected = _unitOfWork.StockRepository.GetAll().Count(p => p.Status == StockRequestStatus.Rejected)
            };
            var requests = new RequestsStatis
            {
                total = _unitOfWork.LzDrgRequestsRepository.GetAll().Count(),
                done = _unitOfWork.LzDrgRequestsRepository.GetAll().Count(s => s.Status == LzDrugRequestStatus.Completed),
                pending = _unitOfWork.LzDrgRequestsRepository.GetAll().Count(s => s.Status == LzDrugRequestStatus.Pending),
                cancel = _unitOfWork.LzDrgRequestsRepository.GetAll().Count(s => s.Status == LzDrugRequestStatus.Rejected)
            };
            var lzDrugs = new LzDrugsStatis
            {
                total = _unitOfWork.LzDrugRepository.GetAll().Count()
            };
            var _d = new StatisShowModel
            {
                pharmacies = pharmaStatis,
                lzDrugs = lzDrugs,
                requests = requests,
                stocks = stockStatis
            };

            return _d;
        }
        public void Update(Admin admin)
        {
            _context.AdminHistoryEntries.Add(new AdminHistory
            {
                IssuerId = UserId,
                OperationType = "Update",
                ToId = admin.Id,
                Desc = $"the user {UserName} with id {UserId} Updated admin [{admin.Name}]"
            });
            _context.Entry(admin).State = EntityState.Modified;

        }

        public override void Remove(Admin admin)
        {
            var subAdmins = GetAll().Where(a => a.SuperAdminId == admin.Id).ToList();
            foreach (var _subAdmin in subAdmins)
            {
                _subAdmin.SuperAdminId = admin.SuperAdminId;
                UpdateFields(_subAdmin, a => a.SuperAdminId);               
            }
            if (subAdmins.Count > 0) Save();
            _context.AdminHistoryEntries.Add(new AdminHistory
            {
                IssuerId = UserId,
                OperationType = "Delete",
                ToId = admin.Id,
                Desc = $"the user {UserName} with id {UserId} deleted  admin [{admin.Name}]"
            });
            base.Remove(admin);
            Save();
            _context.Users.Remove(_context.Users.Find(admin.Id));
        }

        public async Task<PagedList<ShowAdminModel>> GET_PageOfAdminers_ShowModels_ADM(AdminersResourceParameters _params)
        {
            var data = GetAll().Select(a => new ShowAdminModel
            {
                Id = a.Id,
                Name = a.Name,
                SuperId = a.SuperAdminId,
                UserName = a.User.UserName,
                PhoneNumber = a.User.PhoneNumber,
                Type = _context.UserClaims.FirstOrDefault(c => c.UserId == a.Id && c.ClaimType == Variables.AdminClaimsTypes.AdminType).ClaimValue,
                Priviligs = _context.UserClaims.FirstOrDefault(c => c.UserId == a.Id && c.ClaimType == Variables.AdminClaimsTypes.Priviligs).ClaimValue
            });
            if (_params.AdminType != null)
                data = data.Where(d => d.Type ==_params.AdminType);
            if (!string.IsNullOrEmpty(_params.S))
            {
                var searchQueryForWhereClause = _params.S.Trim().ToLowerInvariant();
                data = data
                     .Where(d => d.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }
            return await PagedList<ShowAdminModel>.CreateAsync(data, _params);
        }
    }
}
