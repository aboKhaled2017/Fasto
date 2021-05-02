using AutoMapper;
using Fastdo.Core.Enums;
using Fastdo.Core.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fastdo.Core.Models
{
    public class SendTechSupportViewModel
    {
        public SendTechSupportViewModel(){
            CustomerId = BasicUtility.GetUserId();
            UserType = BasicUtility.GetUserType();
        }
        [JsonIgnore]
        public string CustomerId { get; set; }
        [JsonIgnore]
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
