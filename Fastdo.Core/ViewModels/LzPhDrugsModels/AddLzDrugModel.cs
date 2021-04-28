﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Fastdo.Core.Enums;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.Core.Utilities;

namespace Fastdo.Core.ViewModels
{
    public class AddLzDrugModel
    {
        [Required(ErrorMessage ="اسم الراكد مطلوب")]
        [StringLength(30,MinimumLength =2,ErrorMessage ="الاسم مابين 2 الى 30 حرف")]
        [RequireUniqueLzDrugName]
        public string Name { get; set; }
        [Required(ErrorMessage ="النوع مطلوب")]
        [StringLength(20,MinimumLength =3,ErrorMessage ="اسم النوع غير صحيح")]
        public string Type { get; set; }
        [Required(ErrorMessage ="الكمية مطلوبة")]
        [Range(typeof(int),"1", "2147483647",ErrorMessage ="رقم الكمية غير صحيح")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "السعر مطلوب")]
        [Range(1,double.MaxValue,ErrorMessage ="هذا الرقم غير صالح")]
        public double Price { get; set; }
        public LzDrugConsumeType ConsumeType { get; set; } = LzDrugConsumeType.burning;
        [Required(ErrorMessage ="نسبة الخصم مطلوبة")]
        [Range(0.0, 100.0,ErrorMessage ="هذا الرقم غير صالح")]
        public double Discount { get; set; }
        [Required(ErrorMessage ="تاريخ صلاحية الراكد مطلوب")]
        [ValidateDrugValidDate]
        public DateTime ValideDate { get; set; }
        public LzDrugPriceState PriceType { get; set; } = LzDrugPriceState.newP;
        [Required(ErrorMessage = "تحديد النوع مطلوب")]
        public LzDrugUnitType UnitType { get; set; } = LzDrugUnitType.elba;
        [Required(ErrorMessage = "وصف المنتج مطلوب")]
        public string Desc { get; set; }
    }
}
