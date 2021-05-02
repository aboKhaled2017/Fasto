using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fastdo.Core.Models
{
    public class BaseCustomer
    {
        public BaseCustomer()
        {
            TechQuestions = new List<TechnicalSupportQuestion>();
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Phone]
        public string PersPhone { get; set; }

        [Required]
        [Phone]
        public string LandlinePhone { get; set; }

        public string Address { get; set; }

        [Required]
        public byte AreaId { get; set; }
        [ForeignKey(nameof(AreaId))]
        public virtual Area Area { get; set; }

        [ForeignKey("Id")]
        public virtual AppUser User { get; set; }
        public virtual Pharmacy Pharmacy { get; set; }
        public virtual Stock Stock { get; set; }

        public virtual ICollection<TechnicalSupportQuestion> TechQuestions { get; set; }
    }
}
