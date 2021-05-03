using AutoMapper;
using Fastdo.API.Utilities;
using Fastdo.Core;
using Fastdo.Core.Hubs;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fastdo.API.Hubs
{
    public class TechSupportMessageService : ITechSupportMessageService
    {
        private readonly IHubContext<TechSupportMessagingHub, ITechSupportMessageHub> hubContext;
        private readonly IMapper _mappr;

        public IUnitOfWork _UnitOfWork { get; }

        public TechSupportMessageService(IHubContext<TechSupportMessagingHub, ITechSupportMessageHub> hubContext,IUnitOfWork unitOfWork,IMapper mappr)
        {
            this.hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this._mappr = mappr ?? throw new ArgumentNullException(nameof(mappr));
        }

        #region notify customers
        public async Task NotifyCustomerWithQuestionResponse(TechnicalSupportQuestion response,string customerId)
        {
            var qvm = _mappr.Map<GetTechSupportMessageViewModel>(response);
            await hubContext.Clients.User(customerId).onResponseForQuestion(qvm);
        }
        public async Task NotifyCustomerWithQuestionSeen(TechnicalSupportQuestion q,string customerId)
        {
            //var connectionIds = TechSupportMessagingHub.GetUserConnections(q.CustomerId);
            await hubContext.Clients.User(customerId).onQuestionSeen(q);
        }
        #endregion
        #region notify admin support
        public async Task NotifySystemSupportWithQuestion(GetTechSupportMessageWithDetailsViewModel question)
        {
            var usersIds = _UnitOfWork.AdminRepository.GetAll().Select(a => a.Id);
            //var connectionIds = TechSupportMessagingHub.GetUserConnections(usersIds);
            await hubContext.Clients.Users(usersIds.ToList()).onQuestionAdded(question);
        }

        public async Task NotifySystemSupportWithQuestion(TechnicalSupportQuestion question, IEnumerable<Claim> claims)
        {
            var q = _mappr.Map<GetTechSupportMessageWithDetailsViewModel>(question);
            q.SenderName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            q.SenderAddress = question.UserType == Core.Enums.EUserType.Pharmacy
                ? _UnitOfWork.PharmacyRepository.getAddress(q.SenderId)
                : _UnitOfWork.StockRepository.getAddress(q.SenderId);
            await NotifySystemSupportWithQuestion(q);
        }

        public async Task OnCustomerAddNewQuestionMessage(SendTechSupportViewModel messageModel,ClaimsPrincipal principal)
        {
            if (UserRepoUtility.IsValidUserId(_UnitOfWork, messageModel.UserType, messageModel.CustomerId))
            {
                var obj = _UnitOfWork.TechSupportQRepository.SendQuestiontoTechSupport(messageModel);
                _UnitOfWork.Save();
               await NotifySystemSupportWithQuestion(obj, principal.Claims);
            }
        }
        #endregion
    }
}
