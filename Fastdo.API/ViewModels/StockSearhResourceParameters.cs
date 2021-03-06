using Fastdo.Core;
using Fastdo.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.API
{
    public class StockSearchResourceParameters : IStockSearchResourceParameters
    {
        private const int maxPageSize=10;
        private int _pageSize =10;
        public  int PageNumber { get;  set;} = 1;       
        public  int PageSize 
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
        public string S { get; set; }
        //cityIds=1,2,3&otherParama=...
        [ModelBinder(BinderType = typeof(ArrayModelBinder))] 
        public IEnumerable<byte> CityIds { get; set; }
        //areaIds=1,2,3&otherParama=...
        [ModelBinder(BinderType = typeof(ArrayModelBinder))]
        public IEnumerable<byte> AreaIds { get; set; }
    }
}
