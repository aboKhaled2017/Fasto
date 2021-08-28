using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fastdo.API.Repositories;
using Fastdo.API.Services;
using Fastdo.API.Services.Auth;
using Fastdo.Core.Services;
using Fastdo.Core;
using Fastdo.Core.Services.Auth;
using Fastdo.Core.ViewModels;

namespace Fastdo.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class IndexController : SharedAPIController
    {
        private readonly SysDbContext context;
        public IndexController(HandlingProofImgsServices proofImgsServices, IExecuterDelayer executerDelayer, UserManager<AppUser> userManager, IEmailSender emailSender, IAccountService accountService, IMapper mapper, ITransactionService transactionService, IUnitOfWork unitOfWork, SysDbContext context) : base(userManager, emailSender, accountService, mapper, transactionService, unitOfWork)
        {
            this.context = context;
            _executerDelayer = executerDelayer;
            _handlingProofImgsServices = proofImgsServices;
        }

      
        #region constructor and properties
        private HandlingProofImgsServices _handlingProofImgsServices { get; }
        public IExecuterDelayer _executerDelayer { get; }



        #endregion

        #region main signup

        private async void SignUpForPharmacy([FromForm] PharmacyClientRegisterModel model,string id,string license,string commercial)
        {
  
            try
            {
                var _pharmacy = _mapper.Map<Pharmacy>(model);
                _transactionService.Begin();

                _pharmacy.CustomerId = id;

                _pharmacy.LicenseImgSrc = license;
                _pharmacy.CommercialRegImgSrc = commercial;
                await _unitOfWork.PharmacyRepository.AddAsync(_pharmacy);
                _unitOfWork.Save();
                _transactionService.CommitChanges().End();

            }
            catch (Exception ex)
            {
                _transactionService.RollBackChanges().End();
                return;
            }
            return;
        }

        #endregion

        [HttpGet("execute")]
        public async Task<ActionResult> execute()
        {
            var db = new OtherContextTest();
            var data = db.LzDrugs.FromSql("select * from LzDrugs").ToList();
            var drugs = new List<LzDrug>();
            var str = "12345678";
            var i = 0;
            foreach (var d in data)
            {
                i++;
                drugs.Add(new LzDrug { 
                Id=d.Id,
                BaseDrug=new BaseDrug {
                Type=d.Type,
                Code= $"{str}{i}",
                Name=d.Name
                },
                Code=$"{str}{i}",
                ConsumeType=d.ConsumeType,
                Desc=d.Desc,
                Discount=d.Discount,
                Name=d.Name,
                PharmacyId=d.PharmacyId,
                Price=d.Price,
                PriceType=d.PriceType,
                Quantity=d.Quantity,
                Type=d.Type,
                UnitType=d.UnitType,
                ValideDate=d.ValideDate
                });
   
            }
            context.LzDrugs.AddRange(drugs);
            context.SaveChanges();
            return Ok();
        }
        #region get
        [HttpGet("areas/all")]
        public ActionResult getAllAreas()
        {

            return Ok(_unitOfWork.AreaRepository.GetAll().Select(a=>new { 
              a.Id,
              a.Name,
              a.SuperAreaId
            }));
        }
        [HttpGet("users/all")]
        public async Task<ActionResult> getAllUsers()
        {
            var p = Properties.EmailConfig;
            var stockUsers = (await _userManager
                .GetUsersInRoleAsync(Variables.stocker))
                .Select(u => new
                {
                    u.Id,
                    u.Email,
                    u.PhoneNumber,
                    u.EmailConfirmed
                }).ToList();
            var pharmacyUsers = (await _userManager
               .GetUsersInRoleAsync(Variables.pharmacier))
               .Select(u => new
               {
                   u.Id,
                   u.Email,
                   u.PhoneNumber,
                   u.EmailConfirmed
               }).ToList();
            return Ok(new { 
            stocks=stockUsers,
            pharmacies=pharmacyUsers
            });
        }
        [HttpGet("users/{userId}")]
        public async Task<ActionResult> getUserById(string userId)
        {

            var user = await _userManager.Users               
                .Select(u => new
                {
                    u.Id,
                    u.Email,
                    u.PhoneNumber,
                    u.EmailConfirmed
                }).FirstOrDefaultAsync(u=>u.Id==userId);
            return Ok(user);
        }

        [HttpGet("pharmacies/all")]
        public async Task<ActionResult> getAllPharmacies()
        {
            var data = _unitOfWork.PharmacyRepository.GetAll().Select(p => new
            {
                p.CustomerId,
                email = p.Customer.User.Email,
                drugsNum = p.LzDrugs.Count,
                drugsNames = p.LzDrugs.Select(d =>new {d.Id,d.Name})
            });
            return Ok(await data.ToListAsync());
                   
        }
        #endregion

     
    }
}