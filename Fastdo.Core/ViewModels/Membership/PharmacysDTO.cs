﻿
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.Core.Utilities;

namespace Fastdo.Core.ViewModels
{
    public class Phr_Contacts_Update 
    {
        [Required(ErrorMessage = "رقم التليفون الارضى مطلوب")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "رقم تليفون غير صالح")]
        public string LandlinePhone { get; set; }
        public string Address { get; set; }
    }
    public class Stk_Contacts_Update 
    {
        [Required(ErrorMessage = "رقم التليفون الارضى مطلوب")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "رقم تليفون غير صالح")]
        public string LandlinePhone { get; set; }
        public string Address { get; set; }
    }
}
