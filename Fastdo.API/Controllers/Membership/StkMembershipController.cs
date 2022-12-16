using AutoMapper;
using Fastdo.API.Services.Auth;
using Fastdo.Core.Models;
using Fastdo.Core.Services;
using Fastdo.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fastdo.API.Controllers
{
    [Route("api/stk/membership")]
    [ApiController]
    [Authorize(Policy = "StockPolicy")]
    public class StkMembershipController : SharedAPIController
    {
        public StkMembershipController(UserManager<AppUser> userManager, IEmailSender emailSender, IAccountService accountService, IMapper mapper, ITransactionService transactionService, Core.IUnitOfWork unitOfWork) : base(userManager, emailSender, accountService, mapper, transactionService, unitOfWork)
        {
        }


        #region patch
        [HttpPatch("name")]
        public async Task<IActionResult> UpdatePhNameForStockOfUser(UpdateStkNameModel model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var stock = await _unitOfWork.StockRepository.GetByIdAsync(_userManager.GetUserId(User));
            stock.Customer.Name = model.NewName.Trim();
            _unitOfWork.StockRepository.UpdateName(stock);
            _unitOfWork.Save();
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            var response = await _accountService.GetSigningInResponseModelForCurrentUser(user);
            return Ok(response);
        }

        [HttpPatch("contacts")]
        public async Task<IActionResult> UpdateContactsForStockOfUser(Stk_Contacts_Update model)
        {
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            var stock = await _unitOfWork.StockRepository.GetByIdAsync(_userManager.GetUserId(User));
            stock = _mapper.Map(model, stock);
            _unitOfWork.StockRepository.UpdateContacts(stock);
            _unitOfWork.Save();
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            var response = await _accountService.GetSigningInResponseModelForCurrentUser(user);
            return Ok(response);
        }
        #endregion
    }
}