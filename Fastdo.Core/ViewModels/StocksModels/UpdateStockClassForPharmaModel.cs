using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.Core.ViewModels.Stocks
{
    public class UpdateStockClassForPharmaModel
    {
        [Required(ErrorMessage ="التصنيف لايمكن ان يكون فارغ")]
        [DataType(DataType.Text)]
        public string NewClass { get; set; }
        [Required(ErrorMessage = "التصنيف لايمكن ان يكون فارغ")]
        [DataType(DataType.Text)]
        public string OldClass { get; set; }
    }
    public class UpdateStockClassDiscountModel
    {
        [Required(ErrorMessage = "stock class id is required")]
        public Guid ClassId { get; set; }
        [Required(ErrorMessage ="discount is required")]
        [Range(minimum:1,100,ErrorMessage ="نسبة الخصم من 1 الى 100")]
        public int Discount { get; set; }
    }
}
