﻿using System;
using System.Collections.Generic;
using Fastdo.Core.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.Core.ViewModels
{
    public class LzDrugModel_BM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }       
        public string Type { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public LzDrugConsumeType ConsumeType { get; set; } 
        public double Discount { get; set; }
        public DateTime ValideDate { get; set; }
        public LzDrugPriceState PriceType { get; set; } 
        public LzDrugUnitType UnitType { get; set; }
        public string Desc { get; set; }
        public int RequestCount { get; set; }
    }
    public class LzDrugModel_BM_ForPharma
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public DateTime ValideDate { get; set; }
        public LzDrugPriceState PriceType { get; set; }
        public LzDrugUnitType UnitType { get; set; }
        public string Desc { get; set; }
        
            public string PharmacyId { get; set; }
            public string PharmacyName { get; set; }
    }
}
