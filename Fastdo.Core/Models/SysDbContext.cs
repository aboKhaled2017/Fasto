﻿using Fastdo.Core.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Fastdo.Core.Models
{
    public partial class SysDbContext : IdentityDbContext<AppUser>
    {
        public SysDbContext()
        {
        }

        public SysDbContext(DbContextOptions<SysDbContext> options)
            : base(options)
        {
        }
        //public virtual DbSet<BaseCustomer> Customers { get; set; }
        public virtual DbSet<Pharmacy> Pharmacies { get; set; }
     
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<LzDrug> LzDrugs { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<StkDrug> StkDrugs { get; set; }
        public virtual DbSet<PharmacyInStock> PharmaciesInStocks { get; set; }
        public virtual DbSet<Complain> Complains { get; set; }
        public virtual DbSet<LzDrugRequest> LzDrugRequests { get; set; }
        public virtual DbSet<StkDrugInStkDrgPackageReq> StkDrugInStkDrgPackagesRequests { get; set; }
        public virtual DbSet<StkDrugPackageRequest> StkDrugPackagesRequests { get; set; }
        public virtual DbSet<AdminHistory> AdminHistoryEntries { get; set; }

        //public virtual DbSet<PharmacyInStockClass> PharmaciesInStockClasses { get; set; }
        public virtual DbSet<BaseDrug> BaseDrugs { get; set; }
        public virtual DbSet<TechnicalSupportQuestion> TechnicalSupportQuestions { get; set; }
        public virtual DbSet<StockWithPharmaClass> StocksWithPharmaClasses { get; set; }
        public virtual DbSet<LzDrugExchangeRequest> LzDrugExchangeRequests { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            
            base.OnConfiguring(optionsBuilder);         
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            

            builder.Entity<Admin>()
                .HasOne(a => a.SuperAdmin)
                .WithMany(a=>a.SubAdmins)
                .HasForeignKey("SuperAdminId")
                .OnDelete(DeleteBehavior.Restrict);



            builder.Entity<PharmacyInStock>()
                .HasKey(t => new { t.PharmacyId, t.StockClassId });

            builder.Entity<PharmacyInStock>()
                .HasOne(t => t.Pharmacy)
                .WithMany(p => p.GoinedStocks)
                .HasForeignKey(t => t.PharmacyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PharmacyInStock>()
                .HasOne(t => t.StockClass)
                .WithMany(p => p.GoinedPharmacies)
                .HasForeignKey(t => t.StockClassId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<LzDrugRequest>()
                 .HasIndex(i => new { i.LzDrugId, i.PharmacyId })
                 .IsUnique();
            builder.Entity<StockWithPharmaClass>()
                .HasIndex(i => new { i.StockId, i.ClassName }).IsUnique();

            builder.Entity<StkDrugInStkDrgPackageReq>()
                 .HasIndex(i => new { i.StkDrugId, i.StkDrugPackageId })
                 .IsUnique();
 

            //builder.Entity<PharmacyInStockClass>()
            //   .HasIndex(t => new { t.PharmacyId, t.StockClassId }).IsUnique();
            builder.Entity<StkDrugPackageRequest>()
                .HasOne(e => e.Pharmacy)
                .WithMany(e => e.RequestedStkDrugsPackages)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<StkDrugInStkDrgPackageReq>()
                .HasOne(e => e.Package)
                .WithMany(e => e.PackageDrugs)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Admin>()
                .HasMany(e => e.TechQuestions)
                .WithOne(e => e.Admin)
                .HasForeignKey(e => e.AdminId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<TechnicalSupportQuestion>()
                .HasOne(e => e.Customer)
                .WithMany(e => e.TechQuestions)
                .HasForeignKey(e => e.CustomerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Pharmacy>()
                .HasOne(e => e.Customer)
                .WithOne(e => e.Pharmacy)
                .IsRequired(true);
            builder.Entity<Pharmacy>()
                .HasMany(e => e.LzDrugs)
                .WithOne(e => e.Pharmacy)
                .HasForeignKey(e => e.PharmacyId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Pharmacy>()
                .HasMany(e => e.RequestedLzDrugs)
                .WithOne(e => e.Pharmacy)
                .HasForeignKey(e => e.PharmacyId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Stock>()
                .HasOne(e => e.Customer)
                .WithOne(e => e.Stock)
                .IsRequired(true);

            builder.Entity<TechnicalSupportQuestion>()
                .HasOne(e => e.ResponseOn)
                .WithMany(e=>e.Responses)
                .IsRequired(false);

            builder.Entity<StockWithPharmaClass>()
                .Property(e => e.Discount)
                .HasDefaultValue(null)
                .IsRequired(false);

            base.OnModelCreating(builder);
        }
    //*844# *319#  0333  
    }   
}
