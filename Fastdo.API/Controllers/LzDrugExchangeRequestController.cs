using AutoMapper;
using Fastdo.API.Services.Auth;
using Fastdo.Core;
using Fastdo.Core.Dtos;
using Fastdo.Core.Models;
using Fastdo.Core.Services;
using Fastdo.Core.Utilities;
using Fastdo.Core.ViewModels;
using Fastdo.Core.ViewModels.PhrDrgExchangeRequestsModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.API.Controllers
{
    [Route("api/LzDrugExchangeRequest")]
    [ApiController]
    [Authorize(Policy = "PharmacyPolicy")]
    public class LzDrugExchangeRequestController : SharedAPIController
    {
        public LzDrugExchangeRequestController(UserManager<AppUser> userManager, IEmailSender emailSender, IAccountService accountService, IMapper mapper, ITransactionService transactionService, IUnitOfWork unitOfWork) : base(userManager, emailSender, accountService, mapper, transactionService, unitOfWork)
        {
        }
        #region ExchangeRequester
        #region Get List Of  Requests
        [HttpGet]
        public async Task<IActionResult> GetAllExchangeRequestIMade([FromQuery] LzDrgReqResourceParameters _params)
        {
            var requests = await _unitOfWork.lzDrgRequestExchangeRepository.Get_AllRequests_I_Made(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<Made_LzDrgeExchangeRequest_MB, LzDrgReqResourceParameters>(
                requests, "GetAllExchangeRequestIMade", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(requests);
        }
        #endregion

        #region Get Single LzDrug Request
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLzDrgExchangeRequestDetails(Guid id)
        {
            var req = await _unitOfWork.lzDrgRequestExchangeRepository.Get_Request_I_MadeById(id);
            if (req == null)
                return NotFound();
            return Ok(req);
        }
        #endregion

        #region AddRequest
        [HttpPost]

        public IActionResult PostNewExchangeRequestIMade([FromBody] LzDrugLzDrugExchangeRequestAddInputDto lzDrugLzDrugExchangeRequestInput)
        {
            if (ModelState.IsValid)
                try
                {
                    var request = _unitOfWork.lzDrgRequestExchangeRepository.AddExchangeRequest(lzDrugLzDrugExchangeRequestInput);

                    return  Ok("Created Successfully");
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    // LOG
                    return StatusCode(500, BasicUtility.MakeError(message));
                }


            return BadRequest(ModelState);
        }
        #endregion

        #region PutRequest
        [HttpPut("{id:Guid}")]
        public IActionResult EditExchangeRequestIMade(Guid id, LzExchangeRequestEditDto lzDrugLzDrugExchangeRequestInput)
        {
            if (id != lzDrugLzDrugExchangeRequestInput.Id) return BadRequest();

            if (ModelState.IsValid)
                try
                {
                    return Ok(_unitOfWork.lzDrgRequestExchangeRepository.EditExchangeRequest(lzDrugLzDrugExchangeRequestInput));
                }

                catch (Exception ex)
                {
                    var message = ex.Message;

                    return StatusCode(500, BasicUtility.MakeError("حدثت مشكلة اثناء معالجة طلبك ,من فضلك حاول مرة اخرى"));
                }

            return BadRequest(ModelState);
        }

        #endregion

        #region DeleteRequestIMade
        [HttpDelete("{id:Guid}")]
        public ActionResult Delete(Guid id)
        {
            try
            {
                _unitOfWork.lzDrgRequestExchangeRepository.DeleteExchangeRequest(id);
                return Ok("Deleted");
            }

            catch (Exception ex)
            {
                // LOG
                var message = ex.Message;
                return StatusCode(500, BasicUtility.MakeError("حدثت مشكلة اثناء معالجة طلبك ,من فضلك حاول مرة اخرى"));
            }
        }
        #endregion

         #endregion
        #region ExchangeReceiver
        #region Get List Of  ReceivedRequests
        [HttpGet("Received", Name = "GetAllExchangeRequestIReceiveed")]
        public async Task<IActionResult> GetAllExchangeRequestIReceiveed([FromQuery] LzDrgReqResourceParameters _params)
        {
            var requests = await _unitOfWork.lzDrgRequestExchangeRepository.Get_AllRequests_I_Received(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<Made_LzDrgeExchangeRequest_MB, LzDrgReqResourceParameters>(
                requests, "GetAllExchangeRequestIReceiveed", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(requests);
        }
        #endregion

        #region UpdateRequestStatus
        [HttpPut("(UpatesExchangeRequestStatusIReceived){id:Guid}", Name = "UpatesExchangeRequestStatusIReceived")]
        public IActionResult UpatesExchangeRequestStatusIReceived(Guid id, ExchangeRequestBaseDto lzDrugLzDrugExchangeRequestInput)
        {
            if (id != lzDrugLzDrugExchangeRequestInput.Id) return BadRequest();

            if (ModelState.IsValid)
                try
                {
                    return Ok(_unitOfWork.lzDrgRequestExchangeRepository.UpdateExchangeRequestStatusIReceived(lzDrugLzDrugExchangeRequestInput));
                }

                catch (Exception ex)
                {
                    var message = ex.Message;

                    return StatusCode(500, BasicUtility.MakeError("حدثت مشكلة اثناء معالجة طلبك ,من فضلك حاول مرة اخرى"));
                }

            return BadRequest(ModelState);
        }

        #endregion
        #region UpdateDrugeStatusInReceivedRequest
        [HttpPut("(UpdateDrugStatusInRequestIReceived)/{id:Guid}", Name = "UpdateDrugStatusInRequestIReceived")]
        public IActionResult UpdateDrugStatusInRequestIReceived(Guid id, LzLzDrugExchangeReuestUpdateDurgeStatusInReceivedRequestDto lzDrugLzDrugExchangeRequestInput)
        {
            if (id != lzDrugLzDrugExchangeRequestInput.DrugId) return BadRequest();

            if (ModelState.IsValid)
                try
                {
                    return Ok(_unitOfWork.lzDrgRequestExchangeRepository.UpdateDrugStatusINRequestIReceived(lzDrugLzDrugExchangeRequestInput));
                }

                catch (Exception ex)
                {
                    var message = ex.Message;

                    return StatusCode(500, BasicUtility.MakeError("حدثت مشكلة اثناء معالجة طلبك ,من فضلك حاول مرة اخرى"));
                }

            return BadRequest(ModelState);
        }

        #endregion
        #region AddRequestToReceivedRequest
        [HttpPost("{id}")]
        public IActionResult PostNewExchangeRequestIReceived(Guid id, [FromBody] LzDrugeExchangeAddRequestToRecivedRequestDto lzDrugLzDrugExchangeRequestInput)
        {
            if (id != lzDrugLzDrugExchangeRequestInput.RequestIReceviedId) return BadRequest();

            if (ModelState.IsValid)
                try
                {
                    var request = _unitOfWork.lzDrgRequestExchangeRepository.AddExchangeRequestRecevied(lzDrugLzDrugExchangeRequestInput);

                    return  Ok("Created Successfully");
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    // LOG
                    return StatusCode(500, BasicUtility.MakeError("حدثت مشكلة اثناء معالجة طلبك ,من فضلك حاول مرة اخرى"));
                }


            return BadRequest(ModelState);
        }
        #endregion

        #endregion
    }
}
