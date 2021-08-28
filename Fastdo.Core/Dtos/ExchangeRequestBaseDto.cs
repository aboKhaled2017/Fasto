using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fastdo.Core.Dtos
{
   public class ExchangeRequestBaseDto
    {
        public Guid Id { get; set; }
        public LzDrugRequestExchangeRequestStatus Status { get; set; }

    }
}
