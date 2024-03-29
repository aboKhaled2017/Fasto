﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fastdo.Core.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.Core.Models
{
    public partial class LzDrug
    {
        public LzDrug()
        {
            this.RequestingPharms = new HashSet<LzDrugRequest>();
            this.ConsumeType = LzDrugConsumeType.burning;
            this.PriceType = LzDrugPriceState.oldP;
            this.UnitType = LzDrugUnitType.elba;
            Exchanged = false;

        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public LzDrugConsumeType ConsumeType { get; set; }
        [Required]
        [Range(0.0, 100.0)]
        public double Discount { get; set; }
        [Required]
        public DateTime ValideDate { get; set; }
        [Required]
        public LzDrugPriceState PriceType { get; set; } 
        [Required]
        public LzDrugUnitType UnitType { get; set; } 
        public string Desc { get; set; }
        public bool Exchanged { get; set; }
        [Required]
        public string PharmacyId { get; set; }
        [ForeignKey(nameof(PharmacyId))]
        public virtual Pharmacy Pharmacy { get; set; }
        [InverseProperty("LzDrug")]
        public virtual ICollection<LzDrugRequest> RequestingPharms { get; set; }
       
        public virtual ICollection<LzDrugLzDrugExchangeRequest> LzDrugLzDrugExchangeRequests { get; set; }

        [Required]
        public string Code { get; set; }
        [ForeignKey(nameof(Code))]
        public BaseDrug BaseDrug { get; set; }

    }
}
