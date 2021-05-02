using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.Core.ViewModels;
using Fastdo.Core;
using Fastdo.Core.ViewModels.TechSupport;

namespace Fastdo.Core.Repositories
{
    public interface ITechSupportQRepository: IRepository<TechnicalSupportQuestion>
    {
        TechnicalSupportQuestion SendQuestiontoTechSupport(SendTechSupportViewModel model);
        TechnicalSupportQuestion RespondOnQuestionFromTechSupport(RespondOnQTechSupportViewModel model);


        Task<PagedList<GetCustomerQuestionWithAdminResponsesViewModel>> GetAllQuestionsOfCustomer(string userId, TechSupportMessResourceParameters _params);
        Task<PagedList<GetCustomerQuestionWithAdminResponsesViewModel>> GetNotSeenQuestionsOfCustomer(string userId, TechSupportMessResourceParameters _params);
        Task<PagedList<GetCustomerQuestionWithAdminResponsesViewModel>> GetNotRespondedQuestionsOfCustomer(string userId, TechSupportMessResourceParameters _params);
        Task<PagedList<GetCustomerQuestionWithAdminResponsesViewModel>> GetRespondedQuestionsOfCustomer(string userId, TechSupportMessResourceParameters _params);

        Task<GetCustomerQuestionWithAdminResponsesViewModel> GetQuestionOfCustomer(Guid questionId);
        TechnicalSupportQuestion MarkQuestionAsSeen(Guid questionId);
        Task<PagedList<GetTechSupportMessageWithDetailsViewModel>> GetAllQuestionOfCustomersWithDetails(TechSupportMessResourceParameters _params);
        Task<PagedList<GetTechSupportMessageWithDetailsViewModel>> GetAllRespondedQuestionOfCustomersWithDetails(TechSupportMessResourceParameters _params);
        Task<PagedList<GetTechSupportMessageWithDetailsViewModel>> GetAllNotSeenQuestionOfCustomersWithDetails(TechSupportMessResourceParameters _params);
        Task<PagedList<GetTechSupportMessageWithDetailsViewModel>> GetAllNotRespondedQuestionOfCustomersWithDetails(TechSupportMessResourceParameters _params);
        
    }
}
