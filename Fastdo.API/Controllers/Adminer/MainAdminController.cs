using AutoMapper;
using Fastdo.API.Services.Auth;
using Fastdo.Core;
using Fastdo.Core.Models;
using Fastdo.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Fastdo.API.Controllers.Adminer
{
    [Authorize(Policy = "AdministratorPolicy")]
    public class MainAdminController : SharedAPIController
    {
        public MainAdminController(IAccountService accountService, IMapper mapper, UserManager<AppUser> userManager) : base(accountService, mapper, userManager)
        {
        }

        public MainAdminController(UserManager<AppUser> userManager, IEmailSender emailSender, IAccountService accountService, IMapper mapper, ITransactionService transactionService, IUnitOfWork unitOfWork) : base(userManager, emailSender, accountService, mapper, transactionService, unitOfWork)
        {
        }
    }
}