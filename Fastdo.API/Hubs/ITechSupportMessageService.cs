using Fastdo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fastdo.API.Hubs
{
    public interface ITechSupportMessageService
    {
        Task NotifyCustomerWithQuestionResponse(TechnicalSupportQuestion response, string customerId);
        Task NotifyCustomerWithQuestionSeen(TechnicalSupportQuestion seenQues, string customerId);
        Task NotifySystemSupportWithQuestion(GetTechSupportMessageWithDetailsViewModel question);
        Task NotifySystemSupportWithQuestion(TechnicalSupportQuestion question,IEnumerable<Claim> claims);
        Task OnCustomerAddNewQuestionMessage(SendTechSupportViewModel messageModel,ClaimsPrincipal principal);
        //Task SendAllQuestionsToCustomer(SendTechSupportViewModel messageModel, ClaimsPrincipal principal);
    }
}
