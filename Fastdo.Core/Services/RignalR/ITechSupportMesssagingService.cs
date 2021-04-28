using Fastdo.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fastdo.Core.Services.RignalR
{
    public interface ITechSupportMesssagingService
    {
        void SendQuestionToAdminSupport(string userId, SendTechSupportViewModel q);
        void RespondOnQuestionFromAdminSupport(string userId, SendTechSupportViewModel q);
    }
}
