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
        Task NotifyCustomerWithQuestionResponse(TechnicalSupportQuestion response);
        Task NotifyCustomerWithQuestionSeen(TechnicalSupportQuestion seenQues);
        Task NotifySystemSupportWithQuestion(GetTechSupportMessageWithDetailsViewModel question);
        Task NotifySystemSupportWithQuestion(TechnicalSupportQuestion question,IEnumerable<Claim> claims);
    }
}
