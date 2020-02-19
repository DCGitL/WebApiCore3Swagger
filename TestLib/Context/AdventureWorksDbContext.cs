using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TestLib.Models;

namespace TestLib.Context
{
    public partial class AdventureWorksDbContext : DbContext
    {
        public AdventureWorksDbContext()
        {
        }

        public AdventureWorksDbContext(DbContextOptions<AdventureWorksDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DimEmployee> DimEmployee { get; set; }
        public virtual DbSet<DimSalesTerritory> DimSalesTerritory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=dacpc\\msqlserver;Initial Catalog=AdventureWorksDW2012;User ID=sa;Password=1qaz@WSX");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DimEmployee>(entity =>
            {
                entity.HasKey(e => e.EmployeeKey)
                    .HasName("PK_DimEmployee_EmployeeKey");

                entity.HasIndex(e => e.ParentEmployeeKey);

                entity.HasIndex(e => e.SalesTerritoryKey);

                entity.Property(e => e.BaseRate).HasColumnType("money");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.DepartmentName).HasMaxLength(50);

                entity.Property(e => e.EmailAddress).HasMaxLength(50);

                entity.Property(e => e.EmergencyContactName).HasMaxLength(50);

                entity.Property(e => e.EmergencyContactPhone).HasMaxLength(25);

                entity.Property(e => e.EmployeeNationalIdalternateKey)
                    .HasColumnName("EmployeeNationalIDAlternateKey")
                    .HasMaxLength(15);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.Property(e => e.HireDate).HasColumnType("date");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LoginId)
                    .HasColumnName("LoginID")
                    .HasMaxLength(256);

                entity.Property(e => e.MaritalStatus)
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.Property(e => e.ParentEmployeeNationalIdalternateKey)
                    .HasColumnName("ParentEmployeeNationalIDAlternateKey")
                    .HasMaxLength(15);

                entity.Property(e => e.Phone).HasMaxLength(25);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.ParentEmployeeKeyNavigation)
                    .WithMany(p => p.InverseParentEmployeeKeyNavigation)
                    .HasForeignKey(d => d.ParentEmployeeKey)
                    .HasConstraintName("FK_DimEmployee_DimEmployee");

                entity.HasOne(d => d.SalesTerritoryKeyNavigation)
                    .WithMany(p => p.DimEmployee)
                    .HasForeignKey(d => d.SalesTerritoryKey)
                    .HasConstraintName("FK_DimEmployee_DimSalesTerritory");
            });

            modelBuilder.Entity<DimSalesTerritory>(entity =>
            {
                entity.HasKey(e => e.SalesTerritoryKey)
                    .HasName("PK_DimSalesTerritory_SalesTerritoryKey");

                entity.HasIndex(e => e.SalesTerritoryAlternateKey)
                    .HasName("AK_DimSalesTerritory_SalesTerritoryAlternateKey")
                    .IsUnique();

                entity.Property(e => e.SalesTerritoryCountry)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SalesTerritoryGroup).HasMaxLength(50);

                entity.Property(e => e.SalesTerritoryRegion)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
