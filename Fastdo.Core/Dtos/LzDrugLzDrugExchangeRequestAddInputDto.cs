using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fastdo.Core.Dtos
{
  public  class LzDrugLzDrugExchangeRequestAddInputDto
    {
      [Required(ErrorMessage ="اختر الراكد من فضلك")]
        public List<Guid> LzDrugsIds { get; set; }
      
    }
}
