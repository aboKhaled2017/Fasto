using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fastdo.Core;
using Fastdo.Core.Enums;
using Fastdo.Core.Models;
using Fastdo.Core.Repositories;
using Fastdo.Core.ViewModels.TechSupport;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fastdo.API.Repositories
{
    public class TechSupportQRepository : Repository<TechnicalSupportQuestion>, ITechSupportQRepository
    {
        public TechSupportQRepository(SysDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        #region get add put single object
        public async Task<GetCustomerQuestionWithAdminResponsesViewModel> GetQuestionOfCustomer(Guid questionId)
        {
            return await Where(q => q.Id == questionId)
                  .ProjectTo<GetCustomerQuestionWithAdminResponsesViewModel>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync();
        }
        public TechnicalSupportQuestion RespondOnQuestionFromTechSupport(RespondOnQTechSupportViewModel model)
        {
            var obj = model.GetModel(_mapper);
            MarkQuestionAsSeen(obj.RelatedToId.Value);
            Add(obj);
            return obj;
        }
        public TechnicalSupportQuestion SendQuestiontoTechSupport(SendTechSupportViewModel model)
        {
            var obj = model.GetModel(_mapper);
            obj.CustomerId = UserId;
            Add(obj);
            return obj;
        }
        public TechnicalSupportQuestion MarkQuestionAsSeen(Guid questionId)
        {
            var obj = GetById(questionId);
            if(obj.SeenAt is null) obj.SeenAt= DateTime.Now;
            UpdateFields(obj, e => e.SeenAt);
            return obj;
        }
        #endregion

        #region listof [notSeen|noResponded|All] questions of User[Admin|Pharma|Stock]
        public async Task<PagedList<GetCustomerQuestionWithAdminResponsesViewModel>> GetAllQuestionsOfCustomer(string userId, TechSupportMessResourceParameters _params)
        {
            var data = Where(q => q.CustomerId == userId)
                 .OrderByDescending(q => q.CreatedAt)
                 .ProjectTo<GetCustomerQuestionWithAdminResponsesViewModel>(_mapper.ConfigurationProvider);
            return await PagedList<GetCustomerQuestionWithAdminResponsesViewModel>.CreateAsync(data, _params);
        }
        public async Task<PagedList<GetCustomerQuestionWithAdminResponsesViewModel>> GetNotSeenQuestionsOfCustomer(string userId, TechSupportMessResourceParameters _params)
        {
            var data = Where(q => q.CustomerId == userId && q.SeenAt==null)
                .OrderByDescending(q => q.CreatedAt)
                .ProjectTo<GetCustomerQuestionWithAdminResponsesViewModel>(_mapper.ConfigurationProvider);
            return await PagedList<GetCustomerQuestionWithAdminResponsesViewModel>.CreateAsync(data, _params);
        }
        public async Task<PagedList<GetCustomerQuestionWithAdminResponsesViewModel>> GetNotRespondedQuestionsOfCustomer(string userId, TechSupportMessResourceParameters _params)
        {
            var data = Where(q => q.CustomerId == userId && !q.Responses.Any())
               .OrderByDescending(q => q.CreatedAt)
               .ProjectTo<GetCustomerQuestionWithAdminResponsesViewModel>(_mapper.ConfigurationProvider);
            return await PagedList<GetCustomerQuestionWithAdminResponsesViewModel>.CreateAsync(data, _params);
        }
        public async Task<PagedList<GetCustomerQuestionWithAdminResponsesViewModel>> GetRespondedQuestionsOfCustomer(string userId, TechSupportMessResourceParameters _params)
        {
            var data = Where(q => q.CustomerId == userId && q.Responses.Any())
               .OrderByDescending(q => q.CreatedAt)
               .ProjectTo<GetCustomerQuestionWithAdminResponsesViewModel>(_mapper.ConfigurationProvider);
            return await PagedList<GetCustomerQuestionWithAdminResponsesViewModel>.CreateAsync(data, _params);
        }
        #endregion

        #region listOf question details [All|Responded|NotSeen|NotResponded] requested by admin
        public async Task<PagedList<GetTechSupportMessageWithDetailsViewModel>> GetAllQuestionOfCustomersWithDetails(TechSupportMessResourceParameters _params)
        {

            var data = Where(q => q.UserType != EUserType.Admin)
                .Include(e=>e.Responses)
                .ProjectTo<GetTechSupportMessageWithDetailsViewModel>(_mapper.ConfigurationProvider)
                .OrderByDescending(e => e.CreatedAt);

            return await PagedList<GetTechSupportMessageWithDetailsViewModel>.CreateAsync(data, _params);
        }

        public async Task<PagedList<GetTechSupportMessageWithDetailsViewModel>> GetAllRespondedQuestionOfCustomersWithDetails(TechSupportMessResourceParameters _params)
        {
            var data = Where(q => q.UserType != EUserType.Admin && q.Responses.Any())
                .Include(e=>e.Responses)
                .ProjectTo<GetTechSupportMessageWithDetailsViewModel>(_mapper.ConfigurationProvider)
                .OrderByDescending(q => q.CreatedAt);
            return await PagedList<GetTechSupportMessageWithDetailsViewModel>.CreateAsync(data, _params);
        }

        public async Task<PagedList<GetTechSupportMessageWithDetailsViewModel>> GetAllNotSeenQuestionOfCustomersWithDetails(TechSupportMessResourceParameters _params)
        {
            var data = Where(q => q.UserType != EUserType.Admin && q.SeenAt == null)
                .ProjectTo<GetTechSupportMessageWithDetailsViewModel>(_mapper.ConfigurationProvider)
                .OrderByDescending(q => q.CreatedAt);
            return await PagedList<GetTechSupportMessageWithDetailsViewModel>.CreateAsync(data, _params);
        }

        public async Task<PagedList<GetTechSupportMessageWithDetailsViewModel>> GetAllNotRespondedQuestionOfCustomersWithDetails(TechSupportMessResourceParameters _params)
        {
            var data = Where(q => q.UserType != EUserType.Admin && !q.Responses.Any())
                .ProjectTo<GetTechSupportMessageWithDetailsViewModel>(_mapper.ConfigurationProvider)
                .OrderByDescending(q => q.CreatedAt);

            return await PagedList<GetTechSupportMessageWithDetailsViewModel>.CreateAsync(data, _params);
        }

        #endregion

    }
}
