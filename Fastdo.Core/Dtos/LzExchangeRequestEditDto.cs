using System;
using System.Collections.Generic;
using System.Text;

namespace Fastdo.Core.Dtos
{
  public  class LzExchangeRequestEditDto
    {
        public Guid Id { get; set; }
        public List<Guid> LzDrugsIds { get; set; }

    }
}
