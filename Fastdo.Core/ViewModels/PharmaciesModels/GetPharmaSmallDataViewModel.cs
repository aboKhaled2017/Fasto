using System;
using System.Collections.Generic;
using System.Text;

namespace Fastdo.Core.ViewModels.PharmaciesModels
{
    public class GetPharmaSmallDataViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int DrugsCount { get; set; }
        public string Address { get; set; }
    }
}
