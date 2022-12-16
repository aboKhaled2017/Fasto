﻿using AutoMapper;
using Fastdo.API.Services.Auth;
using Fastdo.Core.Models;
using Fastdo.Core.Utilities;
using Fastdo.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Threading.Tasks;

namespace Fastdo.API.Controllers.Adminer
{
    [Route("api/admin/auth", Name = "AdminAuth")]
    [ApiController]
    public class AdminAuthController : MainAdminController
    {
        public AdminAuthController(IAccountService accountService, IMapper mapper, UserManager<AppUser> userManager) : base(accountService, mapper, userManager)
        {
        }


        #region ovveride methods
        [ApiExplorerSettings(IgnoreApi = true)]
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            _accountService.SetCurrentContext(
                actionContext.HttpContext,
                new UrlHelper(actionContext)
                );
        }
        #endregion


        #region signIn

        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> SignInAdminAsync(AdminLoginModel model)
        {
            if (model == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);
            try
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (await _userManager.UserIdentityExists(user, model.Password, model.AdminType))
                {
                    var response = await _accountService.GetSigningInResponseModelForAdministrator(user, model.AdminType);
                    return Ok(response);
                }
                return NotFound(BasicUtility.MakeError("اسم المستخدم او كلمة السر غير صحيحة"));
            }
            catch (Exception ex)
            {
                return BadRequest(BasicUtility.MakeError(ex.Message));
            }

        }
        #endregion


    }
}