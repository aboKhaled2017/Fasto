using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fastdo.Core.Models
{
  public class LzDrugLzDrugExchangeRequest
    {
        public LzDrugLzDrugExchangeRequest()
        {
            Status = LzDrugRequestExchangeRequestStatus.Pending;
        }
        [Key]
        public int Id { get; set; }
        public Guid LzDrugId { get; set; }
        public LzDrug LzDrug { get; set; }
        public Guid LzDrugExchangeRequestId { get; set; }
        public LzDrugExchangeRequest LzDrugExchangeRequest { get; set; }
        public LzDrugRequestExchangeRequestStatus Status { get; set; }
    }
}
