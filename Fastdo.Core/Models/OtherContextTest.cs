using Fastdo.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fastdo.Core.Models
{
    public  class Stock2
    {
        
        public StockRequestStatus Status { get; set; }
     
        public string Id { get; set; }

       
        public string Name { get; set; }

        public string MgrName { get; set; }

        public string OwnerName { get; set; }

    
        public string LicenseImgSrc { get; set; }


        public string CommercialRegImgSrc { get; set; }

        public string PersPhone { get; set; }

        public string LandlinePhone { get; set; }

        public string Address { get; set; }

   
        public byte AreaId { get; set; }

       
    }
    public class LzDrug2
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
    
        public string Type { get; set; }
        
        public int Quantity { get; set; }
        
        public double Price { get; set; }
    
        public LzDrugConsumeType ConsumeType { get; set; }
    
        
        public double Discount { get; set; }
        
        public DateTime ValideDate { get; set; }
    
        public LzDrugPriceState PriceType { get; set; }
    
        public LzDrugUnitType UnitType { get; set; }
        public string Desc { get; set; }
        
        public string PharmacyId { get; set; }
    }
    public class OtherContextTest:DbContext
    {
        public OtherContextTest()
            
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=SQL5053.site4now.net;Initial Catalog=DB_A4FB0D_fastdo2020;User Id=DB_A4FB0D_fastdo2020_admin;Password=AAaa@123");
            base.OnConfiguring(optionsBuilder);
        }
        public virtual DbSet<Stock2> Stocks { get; set; }
        public virtual DbSet<LzDrug2> LzDrugs { get; set; }
    }
}
