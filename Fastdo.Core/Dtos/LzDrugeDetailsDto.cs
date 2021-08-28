using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fastdo.Core.Dtos
{
  public  class LzDrugeDetailsDto
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public DateTime ValideDate { get; set; }
        public string Description { get; set; }
        public LzDrugRequestExchangeRequestStatus Status { get; set; }

    }
}
