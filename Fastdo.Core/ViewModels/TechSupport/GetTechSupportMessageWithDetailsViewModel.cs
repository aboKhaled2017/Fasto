using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fastdo.Core.Models
{
    public class GetTechSupportMessageWithDetailsViewModel
    {
        public GetTechSupportMessageWithDetailsViewModel()
        {
           
           
        }
        public Guid Id { get; set; }
        [Required]
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderAddress { get; set; }
        public Guid? RelatedToId { get; set; }
        [Required]
        public EUserType UserType { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Message { get; set; }
        public DateTime? SeenAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
