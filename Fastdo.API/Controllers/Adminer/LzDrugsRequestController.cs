using AutoMapper;
using Fastdo.API.Services.Auth;
using Fastdo.Core;
using Fastdo.Core.Models;
using Fastdo.Core.Services;
using Fastdo.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fastdo.API.Controllers.Adminer
{
    [Route("api/admins/drgsReq", Name = "AdminLzDrugRequests")]
    [ApiController]
    [Authorize(Policy = "ControlOnDrugsRequestsPagePolicy")]
    public class AdminLzDrugsRequestController : MainAdminController
    {
        public AdminLzDrugsRequestController(UserManager<AppUser> userManager, IEmailSender emailSender, IAccountService accountService, IMapper mapper, ITransactionService transactionService, IUnitOfWork unitOfWork) : base(userManager, emailSender, accountService, mapper, transactionService, unitOfWork)
        {
        }


        #region override methods from parent class
        [ApiExplorerSettings(IgnoreApi = true)]
        public override string Create_BMs_ResourceUri(IResourceParameters _params, ResourceUriType resourceUriType, string routeName)
        {
            var _cardParams = _params as LzDrgReqResourceParameters;
            switch (resourceUriType)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(routeName,
                    new LzDrgReqResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber - 1,
                        PageSize = _cardParams.PageSize,
                        Status = _cardParams.Status,
                        Seen = _cardParams.Seen
                    }); ;
                case ResourceUriType.NextPage:
                    return Url.Link(routeName,
                    new LzDrgReqResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber + 1,
                        PageSize = _cardParams.PageSize,
                        Status = _cardParams.Status,
                        Seen = _cardParams.Seen
                    });
                default:
                    return Url.Link(routeName,
                    new LzDrgReqResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber,
                        PageSize = _cardParams.PageSize,
                        Status = _cardParams.Status,
                        Seen = _cardParams.Seen
                    });
            }
        }
        #endregion

        #region  get
        [HttpGet(Name = "GET_PageOf_LzDrgsRequests")]
        public async Task<IActionResult> GetPageOfLzDrgsRequests([FromQuery] LzDrgReqResourceParameters _params)
        {
            var requests = await _unitOfWork.LzDrgRequestsRepository.GET_PageOf_LzDrgsRequests(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<Show_LzDrgsReq_ADM_Model, LzDrgReqResourceParameters>(
                requests, "GET_PageOf_LzDrgsRequests", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(requests);
        }
        #endregion
    }
}