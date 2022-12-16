using Fastdo.Core.Models;
using Fastdo.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fastdo.Core.Services.Auth
{
    public interface IBastrctAccountService
    {
        
        Task<ISigningResponseModel> GetSigningInResponseModelForAdministrator(AppUser user, Admin admin, string adminType);
        ISigningResponseModel GetSigningInResponseModelForPharmacy(AppUser user, Pharmacy pharmacy);
        ISigningResponseModel GetSigningInResponseModelForStock(AppUser user, Stock stock);

        Task<ISigningResponseModel> GetSigningInResponseModelForCurrentUser(AppUser user);
       Task<ISigningResponseModel> GetSigningInResponseModelForAdministrator(AppUser user, string adminType);
       Task<ISigningResponseModel> GetSigningInResponseModelForPharmacy(AppUser user);
       Task<ISigningResponseModel> GetSigningInResponseModelForStock(AppUser user);

       Task<ISigningResponseModel> SignUpPharmacyAsync((Pharmacy Pharmacy, string Email, string PersPhone, string Password) model, IExecuterDelayer executerDelayer);
       Task<ISigningResponseModel> SignUpStockAsync(
            (Stock Stock, string Email, string PersPhone, string Password) model,
            Action<string> ExecuteOnError,
            Action<Stock, Action> AddStockModelToRepo,
            Action OnFinsh);

       Task SendEmailConfirmationAsync(string email, string callbackUrl);
        Task<bool> AddSubNewAdmin(AddNewSubAdminModel model, Action<AppUser, Admin> onAddedSuccess);
        Task UpdateSubAdminPassword(AppUser user, UpdateSubAdminPasswordModel model);
        Task<IdentityResult> UpdateSubAdminUserName(AppUser user, UpdateSubAdminUserNameModel model);
        Task<IdentityResult> UpdateSubAdminPhoneNumber(AppUser user, UpdateSubAdminPhoneNumberModel model);
        Task<bool> UpdateSubAdmin(AppUser user, UpdateSubAdminModel model);
    }
}
