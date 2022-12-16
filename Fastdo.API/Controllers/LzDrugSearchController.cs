﻿using AutoMapper;
using Fastdo.API.Services.Auth;
using Fastdo.Core;
using Fastdo.Core.Models;
using Fastdo.Core.Services;
using Fastdo.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fastdo.API.Controllers
{
    [Route("api/lzdrug/search")]
    [ApiController]
    [Authorize(Policy = "PharmacyPolicy")]
    public class LzDrugSearchController : SharedAPIController
    {
        public IpropertyMappingService _propertyMappingService { get; }
        public LzDrugSearchController(UserManager<AppUser> userManager, IpropertyMappingService mappingService, IEmailSender emailSender, IAccountService accountService, IMapper mapper, ITransactionService transactionService, IUnitOfWork unitOfWork) : base(userManager, emailSender, accountService, mapper, transactionService, unitOfWork)
        {
            _propertyMappingService = mappingService;
        }

        #region override methods from parent class
        [ApiExplorerSettings(IgnoreApi = true)]
        public override string Create_BMs_ResourceUri(IResourceParameters _params, ResourceUriType resourceUriType, string routeName)
        {
            var _cardParams = _params as LzDrg_Card_Info_BM_ResourceParameters;
            switch (resourceUriType)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(routeName,
                    new LzDrg_Card_Info_BM_ResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber - 1,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S,
                        PhramId = _cardParams.PhramId,
                        AreaIds = _cardParams.AreaIds,
                        CityIds = _cardParams.CityIds,
                        ValidBefore = _cardParams.ValidBefore,
                        OrderBy = _cardParams.OrderBy,
                        InPharmasIds = _cardParams.InPharmasIds,
                        Code = _cardParams.Code
                    });
                case ResourceUriType.NextPage:
                    return Url.Link(routeName,
                    new LzDrg_Card_Info_BM_ResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber + 1,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S,
                        PhramId = _cardParams.PhramId,
                        AreaIds = _cardParams.AreaIds,
                        CityIds = _cardParams.CityIds,
                        ValidBefore = _cardParams.ValidBefore,
                        OrderBy = _cardParams.OrderBy,
                        InPharmasIds = _cardParams.InPharmasIds,
                        Code = _cardParams.Code
                    });
                default:
                    return Url.Link(routeName,
                    new LzDrg_Card_Info_BM_ResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S,
                        PhramId = _cardParams.PhramId,
                        AreaIds = _cardParams.AreaIds,
                        CityIds = _cardParams.CityIds,
                        ValidBefore = _cardParams.ValidBefore,
                        OrderBy = _cardParams.OrderBy,
                        InPharmasIds = _cardParams.InPharmasIds,
                        Code = _cardParams.Code
                    });
            }
        }


        #endregion

        #region Get All LzDrugs Cards For Search
        [HttpGet(Name = "GetAll_LzDrug_CardInfo_BMs")]
        public async Task<IActionResult> GetPageOfSearchedLzDrugs([FromQuery] LzDrg_Card_Info_BM_ResourceParameters _params)
        {
            if (!_propertyMappingService.validMappingExistsFor<LzDrugCard_Info_BM, LzDrug>(_params.OrderBy))
                return BadRequest();
            var BM_Cards = await _unitOfWork.LzDrg_Search_Repository.Get_All_LzDrug_Cards_BMs(_params);
            BM_Cards.ForEach(BM_Card =>
            {
                BM_Card.RequestUrl = Url.Link("Add_LzDrug_Request_For_User", new { drugId = BM_Card.Id });
                BM_Card.Status = BM_Card.IsMadeRequest ? BM_Card.Status : null;
            });
            var paginationMetaData = new PaginationMetaDataGenerator<LzDrugCard_Info_BM, LzDrg_Card_Info_BM_ResourceParameters>(
                BM_Cards, "GetAll_LzDrug_CardInfo_BMs", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(BM_Cards);
        }

        #endregion

    }
}