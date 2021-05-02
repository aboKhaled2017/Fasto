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
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderAddress { get; set; }
        public Guid? RelatedToId { get; set; }
        public EUserType UserType { get; set; }
        public string Message { get; set; }
        public DateTime? SeenAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<GetRelatedAdminResponseViewModel> Responses { get; set; }
    }
    public class GetRelatedAdminResponseViewModel
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
