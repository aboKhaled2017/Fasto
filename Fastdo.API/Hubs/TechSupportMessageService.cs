using AutoMapper;
using Fastdo.Core;
using Fastdo.Core.Hubs;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task NotifyCustomerWithQuestionResponse(TechnicalSupportQuestion response)
        {
            var question = _UnitOfWork.TechSupportQRepository.GetById(response.RelatedToId);
            var connectionIds = TechSupportMessagingHub.GetUserConnections(question.SenderId);
            await hubContext.Clients.Clients(connectionIds).onResponseForQuestion(question, response);
        }
        public async Task NotifySystemSupportWithQuestion(TechnicalSupportQuestion question)
        {
            var usersIds = _UnitOfWork.AdminRepository.GetAll().Select(a => a.Id);
            var connectionIds = TechSupportMessagingHub.GetUserConnections(usersIds);
            await hubContext.Clients.Clients(connectionIds).onQuestionAdded(question);
        }

    }
}
