using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fastdo.Core.Enums;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Fastdo.Core.Models
{
    public partial class Stock
    {
        public Stock()
        {
            SDrugs = new HashSet<StkDrug>();
            Status = StockRequestStatus.Pending;
            
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ForeignKey(nameof(Customer))]
        public string CustomerId { get; set; }
        [Required]
        public string MgrName { get; set; }

        [Required]
        public string OwnerName { get; set; }

        [Required]
        public string LicenseImgSrc { get; set; }
        public StockRequestStatus Status { get; set; }

        [Required]
        public string CommercialRegImgSrc { get; set; }

        [InverseProperty("Stock")]
        public virtual ICollection<StockWithPharmaClass> PharmasClasses { get; set; }
        public virtual ICollection<StkDrug> SDrugs { get; set; }
        public virtual BaseCustomer Customer { get; set; }
    }
}
