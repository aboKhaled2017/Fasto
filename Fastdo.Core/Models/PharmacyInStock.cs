using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fastdo.Core.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.Core.Models
{
    public partial class PharmacyInStock
    {
        public PharmacyInStock()
        {
            PharmacyReqStatus = 0;
            Seen = false;
        }
        [Key, Column(Order = 0)]
        public string PharmacyId { get; set; }
        [Key, Column(Order = 1)]
        public Guid StockClassId { get; set; }
        public virtual Pharmacy Pharmacy  { get; set; }
        public virtual StockWithPharmaClass StockClass { get; set; }
        public PharmacyRequestStatus PharmacyReqStatus { get; set; }
        public bool Seen { get; set; }
    }
    
}
