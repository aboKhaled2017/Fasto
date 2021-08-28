using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fastdo.Core.Dtos
{
   public class LzLzDrugExchangeReuestUpdateDurgeStatusInReceivedRequestDto
    {
        public Guid RequestId { get; set; }
        public Guid DrugId { get; set; }
        public LzDrugRequestExchangeRequestStatus DrugeStatus { get; set; }
    }
}
