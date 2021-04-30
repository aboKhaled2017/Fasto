using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fastdo.Core;
using Fastdo.Core.Enums;
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

        #region get add put single object
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
        public TechnicalSupportQuestion MarkQuestionAsSeen(Guid questionId)
        {
            var obj = GetById(questionId);
            obj.SeenAt = DateTime.Now;
            UpdateFields(obj, e => e.SeenAt);
            return obj;
        }
        #endregion

        #region listof [notSeen|noResponded|All] questions of User[Admin|Pharma|Stock]
        public async Task<PagedList<GetTechSupportMessageViewModel>> GetAllQuestionsOfUser(string userId, TechSupportMessResourceParameters _params)
        {
            var data = Where(q => q.SenderId == userId)
                 .OrderByDescending(q => q.CreatedAt)
                 .ProjectTo<GetTechSupportMessageViewModel>(_mapper.ConfigurationProvider);
            return await PagedList<GetTechSupportMessageViewModel>.CreateAsync(data, _params);
        }
        public async Task<PagedList<GetTechSupportMessageViewModel>> GetNotSeenQuestionsOfUser(string userId, TechSupportMessResourceParameters _params)
        {
            var data = Where(q => q.SenderId == userId && q.SeenAt==null)
                .OrderByDescending(q => q.CreatedAt)
                .ProjectTo<GetTechSupportMessageViewModel>(_mapper.ConfigurationProvider);
            return await PagedList<GetTechSupportMessageViewModel>.CreateAsync(data, _params);
        }
        public async Task<PagedList<GetTechSupportMessageViewModel>> GetNotRespondedQuestionsOfUser(string userId, TechSupportMessResourceParameters _params)
        {
            var data = Where(q => q.SenderId == userId && !q.Responses.Any())
               .OrderByDescending(q => q.CreatedAt)
               .ProjectTo<GetTechSupportMessageViewModel>(_mapper.ConfigurationProvider);
            return await PagedList<GetTechSupportMessageViewModel>.CreateAsync(data, _params);
        }
        #endregion

        private GetTechSupportMessageWithDetailsViewModel _mapToModel(TechnicalSupportQuestion q)
        {

            return new GetTechSupportMessageWithDetailsViewModel
            {
                Id = q.Id,
                CreatedAt = q.CreatedAt,
                Message = q.Message,
                UserType = q.UserType,
                RelatedToId = q.RelatedToId,
                SeenAt = q.SeenAt,
            };
        }
        private GetTechSupportMessageWithDetailsViewModel _mapToModel(Pharmacy p, TechnicalSupportQuestion q)
        {
            
            return new GetTechSupportMessageWithDetailsViewModel
            {
                Id = q.Id,
                CreatedAt = q.CreatedAt,
                Message = q.Message,
                UserType = q.UserType,
                RelatedToId = q.RelatedToId,
                SeenAt = q.SeenAt,
                SenderId = q.SenderId,
                SenderName = p.Name,
                SenderAddress=$"{p.Area?.Name??""} / {p.Area?.SuperArea?.Name??""}"
            };
        }
        private GetTechSupportMessageWithDetailsViewModel _mapToModel(Stock p, TechnicalSupportQuestion q)
        {

            return new GetTechSupportMessageWithDetailsViewModel
            {
                Id = q.Id,
                CreatedAt = q.CreatedAt,
                Message = q.Message,
                UserType = q.UserType,
                RelatedToId = q.RelatedToId,
                SeenAt = q.SeenAt,
                SenderId = q.SenderId,
                SenderName = p.Name,
                SenderAddress = $"{p.Area?.Name ?? ""} / {p.Area?.SuperArea?.Name ?? ""}"
            };
        }

        #region listOf question details [All|Responded|NotSeen|NotResponded]
        public async Task<PagedList<GetTechSupportMessageWithDetailsViewModel>> GetAllQuestionOfCustomersWithDetails(TechSupportMessResourceParameters _params)
        {
            var _pharmaQues = _unitOfWork.PharmacyRepository.GetAll()
                .SelectMany(e => e.TechQuestions.Select(t => _mapToModel(e, t)));
            var _stockQues = _unitOfWork.StockRepository.GetAll()
                .SelectMany(e => e.TechQuestions.Select(t => _mapToModel(e, t)));

            var _all = _pharmaQues.Union(_stockQues)
                .OrderByDescending(e => e.CreatedAt);

            return await PagedList<GetTechSupportMessageWithDetailsViewModel>.CreateAsync(_all, _params);
        }

        public async Task<PagedList<GetTechSupportMessageWithDetailsViewModel>> GetAllRespondedQuestionOfCustomersWithDetails(TechSupportMessResourceParameters _params)
        {
            var data = Where(q => q.UserType != EUserType.Admin && q.Responses.Any())
                .OrderByDescending(q => q.CreatedAt)
                .Select(q => _mapToModel(q));
            return await PagedList<GetTechSupportMessageWithDetailsViewModel>.CreateAsync(data, _params);
        }

        public async Task<PagedList<GetTechSupportMessageWithDetailsViewModel>> GetAllNotSeenQuestionOfCustomersWithDetails(TechSupportMessResourceParameters _params)
        {
            var data = Where(q => q.UserType != EUserType.Admin && q.SeenAt==null)
                .OrderByDescending(q => q.CreatedAt)
                .Select(q => _mapToModel(q));
            return await PagedList<GetTechSupportMessageWithDetailsViewModel>.CreateAsync(data, _params);
        }

        public async Task<PagedList<GetTechSupportMessageWithDetailsViewModel>> GetAllNotRespondedQuestionOfCustomersWithDetails(TechSupportMessResourceParameters _params)
        {
            var data = Where(q => q.UserType != EUserType.Admin && !q.Responses.Any())
                .OrderByDescending(q => q.CreatedAt)
                .Select(q => _mapToModel(q));

            var _pharmaQues = _unitOfWork.PharmacyRepository.GetAll()
                .SelectMany(e => e.TechQuestions.Select(t => _mapToModel(e, t)));
            var _stockQues = _unitOfWork.StockRepository.GetAll()
                .SelectMany(e => e.TechQuestions.Select(t => _mapToModel(e, t)));

            var _all = _pharmaQues.Union(_stockQues)
                .OrderByDescending(e=>e.CreatedAt);

            return await PagedList<GetTechSupportMessageWithDetailsViewModel>.CreateAsync(data, _params);
        }
        #endregion

    }
}
