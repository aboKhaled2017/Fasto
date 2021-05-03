using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.API.InitSeeds.Helpers;
using Fastdo.Core.ViewModels;
using Fastdo.Core.ViewModels.Stocks;
using astdo.Core.ViewModels.Pharmacies;
using Newtonsoft.Json;
using Fastdo.Core.Interfaces;
using Fastdo.Core.Enums;
using Fastdo.Core.ViewModels.TechSupport;

namespace Fastdo.API.Mappings
{
    public class MappingProfile : Profile, IMappingProfile
    {
        public MappingProfile()
        {
            CreateMap<PharmacierObjectSeeder, Pharmacy>();

            CreateMap<AppUser, PharmacyClientResponseModel>();
            CreateMap<Pharmacy, PharmacyClientResponseModel>()
                .ForMember(dest => dest.UserType, o => o.MapFrom(s => UserType.pharmacier))
                .ForMember(d => d.Name, f => f.MapFrom(e => e.Customer.Name))
                .ForMember(d => d.LandlinePhone, f => f.MapFrom(e => e.Customer.LandlinePhone))
                .ForMember(d => d.PersPhone, f => f.MapFrom(e => e.Customer.PersPhone))
                .ForMember(d=>d.Address,f=>f.MapFrom(e=>e.Customer.Address));
            CreateMap<AppUser, StockClientResponseModel>();
            CreateMap<Stock,   StockClientResponseModel>()
                .ForMember(d=>d.PharmasClasses,o=>o.Ignore())
                .ForMember(dest => dest.UserType, o => o.MapFrom(s => UserType.stocker))
                .ForMember(d => d.Name, f => f.MapFrom(e => e.Customer.Name))
                .ForMember(d => d.LandlinePhone, f => f.MapFrom(e => e.Customer.LandlinePhone))
                .ForMember(d => d.PersPhone, f => f.MapFrom(e => e.Customer.PersPhone))
                .ForMember(d => d.Address, f => f.MapFrom(e => e.Customer.Address));

            CreateMap<AppUser, AdministratorClientResponseModel>();
            CreateMap<Admin, AdministratorClientResponseModel>();


            CreateMap<PharmacyClientRegisterModel, Pharmacy>()
                .ForMember(dest => dest.MgrName, o => o.MapFrom(src => src.MgrName))
                .ForMember(dest => dest.OwnerName, o => o.MapFrom(src => src.OwnerName))
                .ForMember(m => m.Customer,f=> f.MapFrom(c=>new BaseCustomer {
                    AreaId=c.AreaId,
                    Address=c.Address,
                    LandlinePhone=c.LinePhone,
                    PersPhone=c.PersPhone,
                    Name=c.Name,
                }));
                
            CreateMap<StockClientRegisterModel, Stock>()
                .ForMember(dest => dest.MgrName, o => o.MapFrom(src => src.MgrName))
                .ForMember(dest => dest.OwnerName, o => o.MapFrom(src => src.OwnerName))
                .ForMember(m => m.Customer, f => f.MapFrom(c => new BaseCustomer
                {
                    AreaId = c.AreaId,
                    Address = c.Address,
                    LandlinePhone = c.LinePhone,
                    PersPhone = c.PersPhone,
                    Name = c.Name,
                }));

            CreateMap<Phr_RegisterModel_Contacts, Pharmacy>();
            CreateMap<Phr_RegisterModel_Contacts, Stock>();
            CreateMap<Phr_Contacts_Update, Pharmacy>();
            CreateMap<Stk_Contacts_Update, Stock>();

            CreateMap<ComplainToAddModel, Complain>();

            CreateMap<AddLzDrugModel, LzDrug>();
            CreateMap<LzDrug, LzDrugModel_BM>();
            CreateMap<UpdateLzDrugModel, LzDrug>();

            CreateMap<LzDrugRequest,LzDrgRequest_ForUpdate_BM>();
            CreateMap<LzDrgRequest_ForUpdate_BM, LzDrugRequest>();

            CreateMap<LzDrugRequest, LzDrgRequest_ForUpdate_BM>();
            CreateMap<LzDrgRequest_ForUpdate_BM, LzDrugRequest>();

            CreateMap<Pharmacy, Pharmacy_Update_ADM_Model>();
            CreateMap<Pharmacy_Update_ADM_Model, Pharmacy>();

            CreateMap<Stock_Update_ADM_Model, Stock>();
            CreateMap<Stock, Stock_Update_ADM_Model>();

            CreateMap<PharmacyInStock, HandlePharmaRequestModel>()
                .ForMember(dest => dest.Status, o => o.MapFrom(s => s.PharmacyReqStatus));
            CreateMap<HandlePharmaRequestModel, PharmacyInStock>()
                .ForMember(dest => dest.PharmacyReqStatus, o => o.MapFrom(s => s.Status));


            /*CreateMap<LzDrug, LzDrugCard_Info_BM>();
            CreateMap<Pharmacy,LzDrugCard_Info_BM>()
                .ForMember(dest=>dest.PharmName)*/

            CreateMap<TechnicalSupportQuestion, GetTechSupportMessageViewModel>();
            CreateMap<RespondOnQTechSupportViewModel, TechnicalSupportQuestion>()
                .ForMember(m=>m.CustomerId,f=>f.Ignore())
                .ForMember(m => m.AdminId, f => f.MapFrom(e => e.AdminId))
                .ForMember(m => m.Message, f => f.MapFrom(e => e.Response));
            CreateMap<SendTechSupportViewModel, TechnicalSupportQuestion>()
                 .ForMember(m => m.CustomerId, f => f.MapFrom(e => e.CustomerId))
               .ForMember(m => m.Message, f => f.MapFrom(e => e.Question));
            CreateMap<TechnicalSupportQuestion, GetTechSupportMessageWithDetailsViewModel>()
                .ForMember(m => m.SenderId, f => f.MapFrom(e => e.CustomerId))
                .ForMember(m => m.SenderName, f => f.MapFrom(e => e.Customer != null ? e.Customer.Name : null))
                .ForMember(m => m.SenderAddress, f => f.MapFrom(e => e.Customer != null ? $"{e.Customer.Area.Name} / {e.Customer.Area.SuperArea.Name}" : null))
                .ForMember(m => m.Responses, f => f.MapFrom(e => e.Responses.Select(r => new GetRelatedAdminResponseViewModel { 
                  CreatedAt=r.CreatedAt,
                  Id=r.Id,
                  Message=r.Message
                 })
                ));
            
            CreateMap<TechnicalSupportQuestion, GetCustomerQuestionWithAdminResponsesViewModel>()
                .ForMember(m => m.Responses, f => f.MapFrom(e => e.Responses.Select(r => new AdminResponseOnCustomerViewModel
                {
                    CreatedAt = r.CreatedAt,
                    Id = r.Id,
                    AdminId=r.AdminId,
                    SeenAt=r.SeenAt,
                    Message = r.Message
                })
                ));
        }
    }
}
