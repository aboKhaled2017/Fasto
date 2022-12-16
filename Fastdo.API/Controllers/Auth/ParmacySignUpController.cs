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
    [Route("api/ph/signup")]
    [ApiController]
    [AllowAnonymous]
    public class ParmacySignUpController : SharedAPIController
    {
        public ParmacySignUpController(HandlingProofImgsServices proofImgsServices, IExecuterDelayer executerDelayer, UserManager<AppUser> userManager, IEmailSender emailSender, IAccountService accountService, IMapper mapper, ITransactionService transactionService, Core.IUnitOfWork unitOfWork) : base(userManager, emailSender, accountService, mapper, transactionService, unitOfWork)
        {
            _executerDelayer = executerDelayer;
            _handlingProofImgsServices = proofImgsServices;
        }
        #region constructor and properties
        private HandlingProofImgsServices _handlingProofImgsServices { get; }
        public IExecuterDelayer _executerDelayer { get; }



        #endregion

        #region main signup

        [HttpPost]
        public async Task<IActionResult> SignUpForPharmacy([FromForm] PharmacyClientRegisterModel model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            SigningPharmacyClientInResponseModel response = null;
            try
            {
                var _pharmacy = _mapper.Map<Pharmacy>(model);
                _transactionService.Begin();
                response = await _accountService.SignUpPharmacyAsync((_pharmacy, model.Email, model.PersPhone, model.Password), _executerDelayer) as SigningPharmacyClientInResponseModel;
                if (response == null)
                {
                    _transactionService.RollBackChanges().End();
                    return BadRequest(BasicUtility.MakeError("لقد فشلت عملية التسجيل,حاول مرة اخرى"));
                }
                _pharmacy.CustomerId = response.user.Id;
                var savingImgsResponse = _handlingProofImgsServices
                    .SavePharmacyProofImgs(model.LicenseImg, model.CommerialRegImg, _pharmacy.CustomerId);
                if (!savingImgsResponse.Status)
                {
                    _transactionService.RollBackChanges().End();
                    return BadRequest(BasicUtility.MakeError($"{savingImgsResponse.errorMess}"));
                }
                _pharmacy.LicenseImgSrc = savingImgsResponse.LicenseImgPath;
                _pharmacy.CommercialRegImgSrc = savingImgsResponse.CommertialRegImgPath;
                await _unitOfWork.PharmacyRepository.AddAsync(_pharmacy);
                _unitOfWork.Save();
                _executerDelayer.Execute();
                _transactionService.CommitChanges().End();

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
        public IActionResult SignUpStep1ForPharmacy(Phr_RegisterModel_Identity model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            return Ok();
        }
        [HttpPost("step2")]
        //[Consumes("")]
        public IActionResult SignUpStep2ForPharmacy([FromForm] Phr_RegisterModel_Proof model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            return Ok();
        }
        [HttpPost("step3")]
        public IActionResult SignUpStep3ForPharmacy(Phr_RegisterModel_Contacts model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            return Ok();
        }
        [HttpPost("step4")]
        public IActionResult SignUpStep4ForPharmacy(Phr_RegisterModel_Account model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            return Ok();
        }

        #endregion

    }
}