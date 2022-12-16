using AutoMapper;
using Fastdo.API.Services.Auth;
using Fastdo.Core;
using Fastdo.Core.Models;
using Fastdo.Core.Services;
using Fastdo.Core.Utilities;
using Fastdo.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fastdo.API.Controllers.Adminer
{
    [Route("api/admins/stocks", Name = "AdminStocks")]
    [ApiController]
    [Authorize(Policy = "ControlOnStocksPagePolicy")]
    public class AdminStocksController : MainAdminController
    {
        public AdminStocksController(UserManager<AppUser> userManager, IEmailSender emailSender, IAccountService accountService, IMapper mapper, ITransactionService transactionService, IUnitOfWork unitOfWork) : base(userManager, emailSender, accountService, mapper, transactionService, unitOfWork)
        {
        }


        #region override methods from parent class
        [ApiExplorerSettings(IgnoreApi = true)]
        public override string Create_BMs_ResourceUri(IResourceParameters _params, ResourceUriType resourceUriType, string routeName)
        {
            var _cardParams = _params as StockResourceParameters;
            switch (resourceUriType)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(routeName,
                    new StockResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber - 1,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S,
                        Status = _cardParams.Status,
                        OrderBy = _cardParams.OrderBy
                    });
                case ResourceUriType.NextPage:
                    return Url.Link(routeName,
                    new StockResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber + 1,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S,
                        Status = _cardParams.Status,
                        OrderBy = _cardParams.OrderBy
                    });
                default:
                    return Url.Link(routeName,
                    new StockResourceParameters
                    {
                        PageNumber = _cardParams.PageNumber,
                        PageSize = _cardParams.PageSize,
                        S = _cardParams.S,
                        Status = _cardParams.Status,
                        OrderBy = _cardParams.OrderBy
                    });
            }
        }


        #endregion

        #region get
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStockByIdForAdmin([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();
            var stk = await _unitOfWork.StockRepository.Get_StockModel_ADM(id);
            if (stk == null)
                return NotFound();
            return Ok(stk);
        }
        [HttpGet(Name = "Get_PageOfStocks_ADM")]
        public async Task<IActionResult> GetPageOfStocksForAdmin([FromQuery] StockResourceParameters _params)
        {
            var stocks = await _unitOfWork.StockRepository.Get_PageOf_StockModels_ADM(_params);
            var paginationMetaData = new PaginationMetaDataGenerator<Get_PageOf_Stocks_ADMModel, StockResourceParameters>(
                stocks, "Get_PageOfStocks_ADM", _params, Create_BMs_ResourceUri
                ).Generate();
            Response.Headers.Add(Variables.X_PaginationHeader, paginationMetaData);
            return Ok(stocks);
        }
        #endregion

        #region delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStockForAdmin([FromRoute] string id)
        {
            var stk = await _unitOfWork.StockRepository.GetByIdAsync(id);
            if (stk == null)
                return NotFound();
            _unitOfWork.StockRepository.Remove(stk);
            _unitOfWork.Save();
            return NoContent();
        }
        #endregion

        #region Patch
        [HttpPatch("{id}")]
        public async Task<IActionResult> HandleStockRequestForAdmin([FromRoute] string id, [FromBody] JsonPatchDocument<Stock_Update_ADM_Model> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();
            var stk = await _unitOfWork.StockRepository.GetByIdAsync(id);
            if (stk == null)
                return NotFound();
            var requestToPatch = _mapper.Map<Stock_Update_ADM_Model>(stk);
            patchDoc.ApplyTo(requestToPatch);
            //ad validation
            _mapper.Map(requestToPatch, stk);
            var isSuccessfulluUpdated = await _unitOfWork.StockRepository.Patch_Apdate_ByAdmin(stk);
            if (!isSuccessfulluUpdated)
                return StatusCode(500, BasicUtility.MakeError("لقد حدثت مشكلة اثناء معالجة طلبك , من فضلك حاول مرة اخرى"));
            return NoContent();
        }
        #endregion
    }
}