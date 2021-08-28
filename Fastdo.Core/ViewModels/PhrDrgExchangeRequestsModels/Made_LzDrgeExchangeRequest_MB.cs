using Fastdo.Core.Dtos;
using Fastdo.Core.Enums;
using Fastdo.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fastdo.Core.ViewModels.PhrDrgExchangeRequestsModels
{
  public  class Made_LzDrgeExchangeRequest_MB
    {
        [Display(Name="رقم الطلب")]
        public Guid Id { get; set; }
        [Display(Name = "حالة الطلب")]
        public LzDrugRequestExchangeRequestStatus Status { get; set; }
        [Display(Name = "عدد الرواكد المطلبوب اسبدالها داخل  الطلب")]
        public int CountOfRequestedLzDrugs { get; set; }
        [Display(Name = "الصيدلية التى ارسل اليها الطلب")]
        public string ReceiverPharmacy { get; set; }
        [Display(Name = "الصيدلية التى انشأت الطلب")]
        public string RequesterPharmacyName { get; set; }
 //       public ICollection<LzDrugLzDrugExchangeRequest> LzDrugExchangeRequests { get; set; }
        [Display(Name = " الرواكد المطلوب اسبدالها داخل  الطلب")]
        public ICollection<LzDrugeDetailsDto> RequestedDrugs { get; set; }
    }
}
