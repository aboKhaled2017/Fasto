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
        [HttpPost("test")]
        public IActionResult test()
        {
            var obj = new TechnicalSupportQuestion
            {
                Id = Guid.NewGuid(),
                SenderId = GetUserId(),
                CreatedAt = DateTime.Now,
                Message = "please hep me sir cannot not send drug item",
                UserType = Core.Enums.EUserType.Pharmacy
            };
            _messageService.NotifySystemSupportWithQuestion(obj);
            return NoContent();
        }

        #region get | add | post | delete request
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessage(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();
            var obj =await _unitOfWork.TechSupportQRepository.GetQuestionOfUser(id);
            return Ok(obj);
        }

        [HttpPost]
        public IActionResult SendMessageToTechSupport([FromBody]SendTechSupportViewModel model)
        {
            if (!ModelState.IsValid)
                return new Core.UnprocessableEntityObjectResult(ModelState);
            if (!UserRepoUtility.IsValidUserId(_unitOfWork, model.UserType, model.SenderId))
            {
                return BadRequest();
            }
            var obj = _unitOfWork.TechSupportQRepository.SendQuestiontoTechSupport(model);
            _unitOfWork.Save();
            _messageService.NotifySystemSupportWithQuestion(obj);
            return NoContent();
        }
              
        #endregion

        #region get List 
       
       
        [HttpGet(Name = "GetAllQuesOfUser")]
        public async Task<IActionResult> GetAllMessagesOfUser([FromQuery] TechSupportMessResourceParameters _params)
        {
            var messages = await _unitOfWork.TechSupportQRepository.GetAllQuestionsOfUser(GetUserId(), _params);
            var paginationMetaData = new PaginationMetaDataGenerator<GetTechSupportMessageViewModel, TechSupportMessResourceParameters>(
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
                   .GetNotSeenQuestionsOfUser(GetUserId(), _params);
            var paginationMetaData = new PaginationMetaDataGenerator<GetTechSupportMessageViewModel, TechSupportMessResourceParameters>(
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
                   .GetNotRespondedQuestionsOfUser(GetUserId(), _params);
            var paginationMetaData = new PaginationMetaDataGenerator<GetTechSupportMessageViewModel, TechSupportMessResourceParameters>(
                messages, "GetAllNotRespondedQuesOfUser", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(messages);
        }
        #endregion
    }
}
