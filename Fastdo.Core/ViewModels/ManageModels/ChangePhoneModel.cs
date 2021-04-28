﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.Core.Enums;
using Fastdo.Core.Utilities;

namespace Fastdo.Core.ViewModels
{
    public class ChangePhoneModel
    {
        [RegularExpression("^((010)|(011)|(012)|(015)|(017))[0-9]{8}$", ErrorMessage = "رقم هاتف غير صالح")]
        [CheckIfUserPropValueIsExixtsOnUpdate("NewPhone",UserPropertyType.phone)]
        [Required(ErrorMessage ="رقم الهاتف الجديد مطلوب")]
        public string NewPhone { get; set; }
    }
}
