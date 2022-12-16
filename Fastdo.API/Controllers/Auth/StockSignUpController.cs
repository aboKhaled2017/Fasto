using AutoMapper;
using Fastdo.API.Services;
using Fastdo.API.Services.Auth;
using Fastdo.Core.Models;
using Fastdo.Core.Services;
using Fastdo.Core.Utilities;
using Fastdo.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Fastdo.API.Controllers.Auth
{
    [Route("api/stk/signup")]
    [ApiController]
    [AllowAnonymous]
    public class StockSignUpController : SharedAPIController
    {
        public StockSignUpController(HandlingProofImgsServices proofImgsServices, IExecuterDelayer executerDelayer, UserManager<AppUser> userManager, IEmailSender emailSender, IAccountService accountService, IMapper mapper, ITransactionService transactionService, Core.IUnitOfWork unitOfWork) : base(userManager, emailSender, accountService, mapper, transactionService, unitOfWork)
        {
            _handlingProofImgsServices = proofImgsServices;
            _executerDelayer = executerDelayer;
        }
        #region constructor and properties
        private HandlingProofImgsServices _handlingProofImgsServices { get; }
        public IExecuterDelayer _executerDelayer { get; }



        #endregion

        #region main signup
        [HttpPost]
        public async Task<IActionResult> SignUpForStock([FromForm] StockClientRegisterModel model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            SigningStockClientInResponseModel response = null;
            string ErrorMessage = null;
            try
            {
                var _stock = _mapper.Map<Stock>(model);
                _transactionService.Begin();
                response = await _accountService.SignUpStockAsync(
                    (_stock, model.Email, model.PersPhone, model.Password),
                    error =>
                    {
                        _transactionService.RollBackChanges().End();
                        ErrorMessage = error;
                    },
                    (stock, OnFinishAdding) =>
                    {
                        _stock.CustomerId = stock.CustomerId;
                        var savingImgsResponse = _handlingProofImgsServices
                            .SaveStockProofImgs(model.LicenseImg, model.CommerialRegImg, _stock.CustomerId);
                        if (!savingImgsResponse.Status)
                        {
                            _transactionService.RollBackChanges().End();
                            ErrorMessage = savingImgsResponse.errorMess;
                        }
                        _stock.LicenseImgSrc = savingImgsResponse.LicenseImgPath;
                        _stock.CommercialRegImgSrc = savingImgsResponse.CommertialRegImgPath;
                        _unitOfWork.StockRepository.AddAsync(_stock).Wait();
                        _unitOfWork.Save();
                        OnFinishAdding.Invoke();
                    },
                    () =>
                    {
                        _transactionService.CommitChanges().End();
                    }) as SigningStockClientInResponseModel;
                if (ErrorMessage != null || response == null)
                {
                    return BadRequest(BasicUtility.MakeError(ErrorMessage ?? "لقد فشلت عملية التسجيل,حاول مرة اخرى"));
                }

            }
            catch (Exception ex)
            {
                _transactionService.RollBackChanges().End();
                return BadRequest(new { err = ex.Message });
            }
            return Ok(response);

        }
        #endregion

        #region signup steps
        [HttpPost("step1")]
        public IActionResult SignUpStep1ForStock(Phr_RegisterModel_Identity model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            return Ok();
        }
        [HttpPost("step2")]
        //[Consumes("")]
        public IActionResult SignUpStep2ForStock([FromForm] Phr_RegisterModel_Proof model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            return Ok();
        }
        [HttpPost("step3")]
        public IActionResult SignUpStep3ForStock(Phr_RegisterModel_Contacts model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            return Ok();
        }
        [HttpPost("step4")]
        public IActionResult SignUpStep4ForStock(Phr_RegisterModel_Account model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            return Ok();
        }

        #endregion
    }
}