using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fastdo.Core.ViewModels.PhrDrgExchangeRequestsModels
{
   public class Received_Lz_DrgExchangeRequest
    {
        public Guid Id { get; set; }
        public Guid LzDrugId { get; set; }
        public string LzDrugName { get; set; }
        public LzDrugRequestStatus Status { get; set; }
        public string ReceivedFromPharmacyId { get; set; }
        public string ReceivedFromPharmacyName { get; set; }
    }
}
