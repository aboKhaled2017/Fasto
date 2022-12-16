using AutoMapper;
using Fastdo.API.Services.Auth;
using Fastdo.Core;
using Fastdo.Core.Models;
using Fastdo.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fastdo.API.Controllers.Adminer
{
    [Route("api/admin", Name = "Admin")]
    [ApiController]
    public class AdminIndexController : MainAdminController
    {
        public AdminIndexController(UserManager<AppUser> userManager, IEmailSender emailSender, IAccountService accountService, IMapper mapper, ITransactionService transactionService, IUnitOfWork unitOfWork) : base(userManager, emailSender, accountService, mapper, transactionService, unitOfWork)
        {
        }


        #region get
        [HttpGet("statis")]
        public async Task<IActionResult> GetGeneralStatisticsForAdmin()
        {
            return Ok(await _unitOfWork.AdminRepository.GetGeneralStatisOfSystem());
        }
        #endregion

    }
}