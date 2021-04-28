using Fastdo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.API.Hubs
{
    public interface ITechSupportMessageService
    {
        Task NotifyCustomerWithQuestionResponse(TechnicalSupportQuestion response);
       Task NotifySystemSupportWithQuestion(TechnicalSupportQuestion question);
        
    }
}
