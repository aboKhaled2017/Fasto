﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.Core.ViewModels
{
    public class StockClassWithPharmaCountsModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
