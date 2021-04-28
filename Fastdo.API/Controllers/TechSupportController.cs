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

        [HttpGet("customer", Name = "GetAllQuesOfCustomers")]
        public async Task<IActionResult> GetAllMessageOfUsers([FromQuery] TechSupportMessResourceParameters _params)
        {
            var messages = await _unitOfWork.TechSupportQRepository.GetAllQuestionsOfCustomers(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<GetTechSupportMessageViewModel, TechSupportMessResourceParameters>(
                messages, "GetAllQuesOfCustomers", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(messages);
        }

        [HttpGet("customer/{userId}", Name = "GetAllQuesOfUser")]
        public async Task<IActionResult> GetAllMessagesOfUser([FromQuery] TechSupportMessResourceParameters _params,[FromRoute]Guid userId)
        {
            var messages = await _unitOfWork.TechSupportQRepository.GetAllQuestionsOfUser(userId,_params);
            var paginationMetaData = new PaginationMetaDataGenerator<GetTechSupportMessageViewModel, TechSupportMessResourceParameters>(
                messages, "GetAllQuesOfUser", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(messages);
        }

        [HttpGet("{id}")]
        public IActionResult GetMessage(Guid id)
        {
            if (id == null || id == Guid.Empty) return BadRequest();
            var obj = _unitOfWork.TechSupportQRepository.GetQuestionOfUser(id);
            return Ok(obj);
        }

        [HttpPost("customer")]
        public IActionResult SendMessageToTechSupport(SendTechSupportViewModel model)
        {
            if (!ModelState.IsValid)
                return new Core.UnprocessableEntityObjectResult(ModelState);
            var obj= _unitOfWork.TechSupportQRepository.SendQuestiontoTechSupport(model);
            _unitOfWork.Save();
            _messageService.NotifySystemSupportWithQuestion(obj);
            return NoContent();
        }
        [HttpPost("admin")]
        public IActionResult respondMessageOnCustomer(RespondOnQTechSupportViewModel model)
        {
            if (!ModelState.IsValid)
                return new Core.UnprocessableEntityObjectResult(ModelState);
            var resObj=_unitOfWork.TechSupportQRepository.RespondOnQuestionFromTechSupport(model);
            _unitOfWork.Save();
            return NoContent();
        }
        [HttpPut("{id}")]
        public IActionResult markMessageAsSeenAt(Guid id)
        {
            if (id == null || id == Guid.Empty)
                return BadRequest();
            _unitOfWork.TechSupportQRepository.MarkQuestionAsSeen(id);
            _unitOfWork.Save();
            return NoContent();
        }

        //[AllowAnonymous]
        //public ActionResult SendMessageNotification(string User, string message)
        //{
        //    var connections = NotificationHub.GetUserConnections(User);

        //    if (connections != null)
        //    {
        //        foreach (var connection in connections)
        //        {
        //            // Notify the client to refresh the list of connections
        //            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
        //            hubContext.Clients.Clients(new[] { connection }).alert(message);
        //        }
        //    }

        //    return new HttpStatusCodeResult(HttpStatusCode.OK);
        //}
    }
}
