using AutoMapper;
using Fastdo.API.Hubs;
using Fastdo.API.Services.Auth;
using Fastdo.Core;
using Fastdo.Core.Models;
using Fastdo.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Fastdo.API.Controllers.Adminer
{
    [Route("api/admins/techsupport")]
    [ApiController]
    //[Authorize(Policy = "ControlOnDrugsRequestsPagePolicy")]
    public class AdminTecSupportController : MainAdminController
    {
        private readonly ITechSupportMessageService _messageService;

        public AdminTecSupportController(UserManager<AppUser> userManager, IEmailSender emailSender,
            IAccountService accountService, IMapper mapper, ITransactionService transactionService,
            IUnitOfWork unitOfWork, ITechSupportMessageService messageService)
            : base(userManager, emailSender, accountService, mapper, transactionService, unitOfWork)
        {
            _messageService = messageService;
        }

        #region get | add | post | delete request
        [HttpPut("{id}")]
        public IActionResult markMessageAsSeenAt(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();
            var q = _unitOfWork.TechSupportQRepository.MarkQuestionAsSeen(id);
            _unitOfWork.Save();
            _messageService.NotifyCustomerWithQuestionSeen(q, q.CustomerId);
            return NoContent();
        }

        [HttpPost]
        public IActionResult respondMessageOnCustomer([FromBody] RespondOnQTechSupportViewModel model)
        {
            if (!ModelState.IsValid)
                return new Core.UnprocessableEntityObjectResult(ModelState);
            var q = _unitOfWork.TechSupportQRepository.RespondOnQuestionFromTechSupport(model);
            _unitOfWork.Save();
            _messageService.NotifyCustomerWithQuestionResponse(q, model.CustomerId);
            return NoContent();
        }

        #endregion

        #region get List 
        [HttpGet(Name = "GetAllQuesOfCustomers")]
        public async Task<IActionResult> GetAllMessageOfCustomers([FromQuery] TechSupportMessResourceParameters _params)
        {
            var messages = await _unitOfWork.TechSupportQRepository.GetAllQuestionOfCustomersWithDetails(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<GetTechSupportMessageWithDetailsViewModel, TechSupportMessResourceParameters>(
                messages, "GetAllQuesOfCustomers", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(messages);
        }

        [HttpGet("notseen", Name = "GetAllNotSeenQuesOfCustomers")]
        public async Task<IActionResult> GetAllNotSeenMessageOfCustomers([FromQuery] TechSupportMessResourceParameters _params)
        {
            var messages = await _unitOfWork.TechSupportQRepository.GetAllNotSeenQuestionOfCustomersWithDetails(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<GetTechSupportMessageWithDetailsViewModel, TechSupportMessResourceParameters>(
                messages, "GetAllNotSeenQuesOfCustomers", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(messages);
        }

        [HttpGet("notresponded", Name = "GetAllNotRespondedQuesOfCustomers")]
        public async Task<IActionResult> GetAllNotRespondedMessageOfCustomers([FromQuery] TechSupportMessResourceParameters _params)
        {
            var messages = await _unitOfWork.TechSupportQRepository.GetAllNotRespondedQuestionOfCustomersWithDetails(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<GetTechSupportMessageWithDetailsViewModel, TechSupportMessResourceParameters>(
                messages, "GetAllNotRespondedQuesOfCustomers", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(messages);
        }

        [HttpGet("responded", Name = "GetAllRespondedQuesOfCustomers")]
        public async Task<IActionResult> GetAllRespondedMessageOfCustomers([FromQuery] TechSupportMessResourceParameters _params)
        {
            var messages = await _unitOfWork.TechSupportQRepository.GetAllRespondedQuestionOfCustomersWithDetails(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<GetTechSupportMessageWithDetailsViewModel, TechSupportMessResourceParameters>(
                messages, "GetAllRespondedQuesOfCustomers", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(messages);
        }
        #endregion
    }
}