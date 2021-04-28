using AutoMapper;
using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fastdo.Core.Models
{
    public class SendTechSupportViewModel
    {
        public SendTechSupportViewModel()
        {
           
           
        }
        [Required]
        public string SenderId { get; set; }
        [Required]
        public EUserType UserType { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string  Question { get; set; }
        public TechnicalSupportQuestion GetModel(IMapper mapper)
        {
            return mapper.Map<TechnicalSupportQuestion>(this);
        }
    }
}
