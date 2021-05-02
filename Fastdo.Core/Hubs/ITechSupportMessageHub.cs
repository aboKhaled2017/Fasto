using Fastdo.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fastdo.Core.Hubs
{
    public interface ITechSupportMessageHub
    {
        Task onResponseForQuestion(TechnicalSupportQuestion question, TechnicalSupportQuestion response);
        Task onQuestionSeen(TechnicalSupportQuestion question);
        Task onQuestionAdded(GetTechSupportMessageWithDetailsViewModel question);
    }
}
