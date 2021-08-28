using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fastdo.Core.Models
{
  public class LzDrugExchangeRequest
    {
        public LzDrugExchangeRequest()
        {
            Status = LzDrugRequestExchangeRequestStatus.Pending;
           
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } 
        public LzDrugRequestExchangeRequestStatus Status { get; set; }
        [Required]
        [Display(Name = "PharmacyRequested")]
        public string PharmacyId { get; set; }
        [ForeignKey("PharmacyId")]
        [InverseProperty("RequestedexchangedLzDrugs")]
        public virtual Pharmacy Pharmacy { get; set; }

        public virtual ICollection<LzDrugLzDrugExchangeRequest> LzDrugLzDrugExchangeRequests { get; set; }


    }
}
