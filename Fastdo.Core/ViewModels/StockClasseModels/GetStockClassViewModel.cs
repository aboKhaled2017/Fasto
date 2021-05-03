using System;
using System.Collections.Generic;
using System.Text;

namespace Fastdo.Core.ViewModels.StockClasseModels
{
    public class GetStockClassViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int? Discount { get; set; } = null;
    }
}
