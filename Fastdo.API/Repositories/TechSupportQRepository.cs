using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fastdo.Core;
using Fastdo.Core.Models;
using Fastdo.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.API.Repositories
{
    public class TechSupportQRepository : Repository<TechnicalSupportQuestion>, ITechSupportQRepository
    {
        public TechSupportQRepository(SysDbContext context, IMapper mapper) : base(context, mapper)
        {
        }


        public async Task<PagedList<GetTechSupportMessageViewModel>> GetAllQuestionsOfCustomers(TechSupportMessResourceParameters _params)
        {
            var data = Where(q=>q.UserType!=Core.Enums.EUserType.Admin)
                 .OrderByDescending(q=>q.CreatedAt)
                 .ProjectTo<GetTechSupportMessageViewModel>(_mapper.ConfigurationProvider);
            return await PagedList<GetTechSupportMessageViewModel>.CreateAsync(data, _params);
        }

        public async Task<GetTechSupportMessageViewModel> GetQuestionOfUser(Guid questionId)
        {
            return await Where(q => q.Id == questionId)
                  .ProjectTo<GetTechSupportMessageViewModel>(_mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync();
        }

        public TechnicalSupportQuestion RespondOnQuestionFromTechSupport(RespondOnQTechSupportViewModel model)
        {
            var obj = model.GetModel(_mapper);
            obj.SenderId = UserId;
            Add(obj);
            return obj;
        }

        public TechnicalSupportQuestion SendQuestiontoTechSupport(SendTechSupportViewModel model)
        {
            var obj = model.GetModel(_mapper);
            obj.SenderId = UserId;
            Add(obj);
            return obj;
        }

        public async Task<PagedList<GetTechSupportMessageViewModel>> GetAllQuestionsOfUser(Guid userId, TechSupportMessResourceParameters _params)
        {
            var data = Where(q => q.SenderId == userId.ToString())
                 .OrderByDescending(q => q.CreatedAt)
                 .ProjectTo<GetTechSupportMessageViewModel>(_mapper.ConfigurationProvider);
            return await PagedList<GetTechSupportMessageViewModel>.CreateAsync(data, _params);
        }

        public async Task<PagedList<GetTechSupportMessageViewModel>> GetNotSeenQuestionsOfUser(Guid userId, TechSupportMessResourceParameters _params)
        {
            var data = Where(q => q.SenderId == userId.ToString())
                .OrderByDescending(q => q.CreatedAt)
                .ProjectTo<GetTechSupportMessageViewModel>(_mapper.ConfigurationProvider);
            return await PagedList<GetTechSupportMessageViewModel>.CreateAsync(data, _params);
        }

        public TechnicalSupportQuestion MarkQuestionAsSeen(Guid questionId)
        {
            var obj = GetById(questionId);
            obj.SeenAt = DateTime.Now;
            UpdateFields(obj, e => e.SeenAt);
            return obj;
        }
    }
}
