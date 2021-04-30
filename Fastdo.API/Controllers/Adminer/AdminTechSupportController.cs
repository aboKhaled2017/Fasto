using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fastdo.Core.ViewModels;
using Fastdo.API.Repositories;
using Fastdo.API.Services.Auth;
using Fastdo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using Fastdo.Core.Services.Auth;
using Fastdo.Core.Services;
using Fastdo.Core;

namespace Fastdo.API.Controllers.Adminer
{
    [Route("api/admins/techsupport")]
    [ApiController]
    //[Authorize(Policy = "ControlOnDrugsRequestsPagePolicy")]
    public class AdminTecSupportController : MainAdminController
    {
        public AdminTecSupportController(UserManager<AppUser> userManager, IEmailSender emailSender, IAccountService accountService, IMapper mapper, ITransactionService transactionService, IUnitOfWork unitOfWork) : base(userManager, emailSender, accountService, mapper, transactionService, unitOfWork)
        {
        }

        #region get | add | post | delete request
        [HttpPut("{id}")]
        public IActionResult markMessageAsSeenAt(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();
            _unitOfWork.TechSupportQRepository.MarkQuestionAsSeen(id);
            _unitOfWork.Save();
            return NoContent();
        }

        [HttpPost]
        public IActionResult respondMessageOnCustomer([FromBody]RespondOnQTechSupportViewModel model)
        {
            if (!ModelState.IsValid)
                return new Core.UnprocessableEntityObjectResult(ModelState);
            var resObj = _unitOfWork.TechSupportQRepository.RespondOnQuestionFromTechSupport(model);
            _unitOfWork.Save();
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