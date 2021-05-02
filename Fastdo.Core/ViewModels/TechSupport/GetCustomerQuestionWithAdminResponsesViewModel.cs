using System;
using System.Collections.Generic;
using System.Text;

namespace Fastdo.Core.ViewModels.TechSupport
{
    public class GetCustomerQuestionWithAdminResponsesViewModel
    {
        public Guid Id { get; set; }    
        public Guid? RelatedToId { get; set; }
        public string Message { get; set; }
        public DateTime? SeenAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<AdminResponseOnCustomerViewModel> Responses { get; set; }
    }
    public class AdminResponseOnCustomerViewModel
    {
        public Guid Id { get; set; }
        public string AdminId { get; set; }
        public string Message { get; set; }
        public DateTime? SeenAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
