using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.Core.ViewModels;
using Fastdo.Core;

namespace Fastdo.Core.Repositories
{
    public interface ITechSupportQRepository: IRepository<TechnicalSupportQuestion>
    {
        TechnicalSupportQuestion SendQuestiontoTechSupport(SendTechSupportViewModel model);
        TechnicalSupportQuestion RespondOnQuestionFromTechSupport(RespondOnQTechSupportViewModel model);
        Task<PagedList<GetTechSupportMessageViewModel>> GetAllQuestionsOfCustomers(TechSupportMessResourceParameters _params);
        Task<PagedList<GetTechSupportMessageViewModel>> GetAllQuestionsOfUser(Guid userId, TechSupportMessResourceParameters _params);
        Task<PagedList<GetTechSupportMessageViewModel>> GetNotSeenQuestionsOfUser(Guid userId, TechSupportMessResourceParameters _params);
        Task<GetTechSupportMessageViewModel> GetQuestionOfUser(Guid questionId);
        TechnicalSupportQuestion MarkQuestionAsSeen(Guid questionId);
    }
}
