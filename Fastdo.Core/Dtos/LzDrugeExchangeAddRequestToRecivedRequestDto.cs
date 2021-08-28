using System;
using System.Collections.Generic;
using System.Text;

namespace Fastdo.Core.Dtos
{
   public class LzDrugeExchangeAddRequestToRecivedRequestDto
    {
        public List<Guid> LzDrugsIds { get; set; }
        public Guid RequestIReceviedId { get; set; }

    }
}
