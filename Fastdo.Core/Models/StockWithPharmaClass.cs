﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fastdo.Core.Models
{
    public partial class StockWithPharmaClass
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string StockId { get; set; }
        [ForeignKey("StockId")]
        [InverseProperty("PharmasClasses")]
        public virtual Stock Stock { get; set; }
        public string ClassName { get; set; }
        public int? Discount { get; set; } = null;
        public virtual ICollection<PharmacyInStock> GoinedPharmacies { get; set; } = new HashSet<PharmacyInStock>();
    }
}
