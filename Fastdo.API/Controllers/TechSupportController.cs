using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fastdo.Core.ViewModels;
using Fastdo.API.Repositories;
using Fastdo.API.Services.Auth;
using Fastdo.Core;
using Fastdo.Core.Utilities;
using Fastdo.Core.Services.Auth;
using Microsoft.AspNetCore.Identity.UI.Services;
using Fastdo.Core.Services;
using Fastdo.API.Hubs;
using Fastdo.API.Utilities;
using Fastdo.Core.ViewModels.TechSupport;

namespace Fastdo.API.Controllers
{
    [Route("api/techsupport")]
    [ApiController]
    [Authorize]
    public class TechSupportMessagingController : SharedAPIController
    {
        private readonly ITechSupportMessageService _messageService;

        public TechSupportMessagingController(UserManager<AppUser> userManager, IEmailSender emailSender, 
            IAccountService accountService, IMapper mapper, ITransactionService transactionService, 
            IUnitOfWork unitOfWork, ITechSupportMessageService messageService) : base(userManager, emailSender, accountService, mapper, transactionService, unitOfWork)
        {
            this._messageService = messageService;
        }
        
        #region get | add | post | delete request
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessage(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var obj =await _unitOfWork.TechSupportQRepository.GetQuestionOfCustomer(id);
            return Ok(obj);
        }

        [HttpPost]
        public IActionResult SendMessageToTechSupport([FromBody]SendTechSupportViewModel model)
        {
            if (!ModelState.IsValid)
                return new Core.UnprocessableEntityObjectResult(ModelState);
            if (!UserRepoUtility.IsValidUserId(_unitOfWork, model.UserType, model.CustomerId))
            {
                return BadRequest();
            }
            var obj = _unitOfWork.TechSupportQRepository.SendQuestiontoTechSupport(model);
            _unitOfWork.Save();
            _messageService.NotifySystemSupportWithQuestion(obj,User.Claims);
            return Ok(new { id=obj.Id});
        }
              
        #endregion


        #region get List  
       
        [HttpGet(Name = "GetAllQuesOfUser")]
        public async Task<IActionResult> GetAllMessagesOfUser([FromQuery] TechSupportMessResourceParameters _params)
        {
            var messages = await _unitOfWork.TechSupportQRepository.GetAllQuestionsOfCustomer(GetUserId(), _params);
            var paginationMetaData = new PaginationMetaDataGenerator<GetCustomerQuestionWithAdminResponsesViewModel, TechSupportMessResourceParameters>(
                messages, "GetAllQuesOfUser", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(messages);
        }
        
        [HttpGet("notseen", Name = "GetAllNotSeenQuesOfUser")]
        public async Task<IActionResult> GetAllNotSeenMessagesOfUser([FromQuery] TechSupportMessResourceParameters _params)
        {
            var messages = await _unitOfWork
                   .TechSupportQRepository
                   .GetNotSeenQuestionsOfCustomer(GetUserId(), _params);
            var paginationMetaData = new PaginationMetaDataGenerator<GetCustomerQuestionWithAdminResponsesViewModel, TechSupportMessResourceParameters>(
                messages, "GetAllNotSeenQuesOfUser", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(messages);
        }

        [HttpGet("notresponded", Name = "GetAllNotRespondedQuesOfUser")]
        public async Task<IActionResult> GetAllNotRespondedMessagesOfUser([FromQuery] TechSupportMessResourceParameters _params)
        {
            var messages = await _unitOfWork
                   .TechSupportQRepository
                   .GetNotRespondedQuestionsOfCustomer(GetUserId(), _params);
            var paginationMetaData = new PaginationMetaDataGenerator<GetCustomerQuestionWithAdminResponsesViewModel, TechSupportMessResourceParameters>(
                messages, "GetAllNotRespondedQuesOfUser", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(messages);
        }

        [HttpGet("responded", Name = "GetAllRespondedQuesOfUser")]
        public async Task<IActionResult> GetAllRespondedMessagesOfUser([FromQuery] TechSupportMessResourceParameters _params)
        {
            var messages = await _unitOfWork
                   .TechSupportQRepository
                   .GetRespondedQuestionsOfCustomer(GetUserId(), _params);
            var paginationMetaData = new PaginationMetaDataGenerator<GetCustomerQuestionWithAdminResponsesViewModel, TechSupportMessResourceParameters>(
                messages, "GetAllRespondedQuesOfUser", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(messages);
        }
        #endregion
    }
}
