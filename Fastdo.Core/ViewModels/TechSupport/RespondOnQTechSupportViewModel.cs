﻿using AutoMapper;
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
    public class RespondOnQTechSupportViewModel
    {
        public RespondOnQTechSupportViewModel()
        {
            AdminId = BasicUtility.GetUserId();
            UserType = BasicUtility.GetUserType();
        }
        [JsonIgnore]
        public string AdminId { get; set; }
        [JsonIgnore]
        public EUserType UserType { get; set; }
        [DataType(DataType.Text)]
        public string Response { get; set; }
        public Guid? RelatedToId { get; set; }
        [Required]
        public string CustomerId { get; set; }
        public TechnicalSupportQuestion GetModel(IMapper mapper)
        {
            return mapper.Map<TechnicalSupportQuestion>(this);
        }
    }
}
