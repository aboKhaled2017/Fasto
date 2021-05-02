using Fastdo.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Fastdo.Core.Models
{
    public class TechnicalSupportQuestion
    {
        public TechnicalSupportQuestion()
        {
            CreatedAt = DateTime.Now;
            Responses = new List<TechnicalSupportQuestion>();
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string CustomerId { get; set; }
        public string AdminId { get; set; }
        public Guid? RelatedToId { get; set; }
        [Required]
        public EUserType UserType { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string  Message { get; set; }
        public DateTime? SeenAt { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(RelatedToId))]
        public virtual TechnicalSupportQuestion ResponseOn { get; set; }
        [ForeignKey(nameof(AdminId))]
        public virtual Admin Admin { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public virtual BaseCustomer Customer { get; set; }
        public virtual ICollection<TechnicalSupportQuestion> Responses { get; set; }
    }
}
