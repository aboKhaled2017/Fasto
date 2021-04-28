using AutoMapper;
using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fastdo.Core.Models
{
    public class RespondOnQTechSupportViewModel
    {
        public RespondOnQTechSupportViewModel()
        {
           
           
        }
        [Required]
        public string SenderId { get; set; }
        [Required]
        public EUserType UserType { get; set; }
        [DataType(DataType.Text)]
        public string Response { get; set; }
        public Guid? RelatedToId { get; set; }
        public TechnicalSupportQuestion GetModel(IMapper mapper)
        {
            return mapper.Map<TechnicalSupportQuestion>(this);
        }
    }
}
