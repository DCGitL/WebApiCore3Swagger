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

        public virtual DbSet<DimAccount> DimAccount { get; set; }
        public virtual DbSet<DimCurrency> DimCurrency { get; set; }
        public virtual DbSet<DimDate> DimDate { get; set; }
        public virtual DbSet<DimDepartmentGroup> DimDepartmentGroup { get; set; }
        public virtual DbSet<DimEmployee> DimEmployee { get; set; }
        public virtual DbSet<DimOrganization> DimOrganization { get; set; }
        public virtual DbSet<DimSalesTerritory> DimSalesTerritory { get; set; }
        public virtual DbSet<DimScenario> DimScenario { get; set; }
        public virtual DbSet<FactFinance> FactFinance { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=dacpc\\msqlserver;Initial Catalog=AdventureWorksDW2012;User ID=sa;Password=1qaz@WSX");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DimAccount>(entity =>
            {
                entity.HasKey(e => e.AccountKey);

                entity.HasIndex(e => e.AccountCodeAlternateKey)
                    .HasName("AK_DimAccount_AccountCodeAlternateKey")
                    .IsUnique();

                entity.Property(e => e.AccountDescription).HasMaxLength(50);

                entity.Property(e => e.AccountType).HasMaxLength(50);

                entity.Property(e => e.CustomMemberOptions).HasMaxLength(200);

                entity.Property(e => e.CustomMembers).HasMaxLength(300);

                entity.Property(e => e.Operator).HasMaxLength(50);

                entity.Property(e => e.ValueType).HasMaxLength(50);

                entity.HasOne(d => d.ParentAccountKeyNavigation)
                    .WithMany(p => p.InverseParentAccountKeyNavigation)
                    .HasForeignKey(d => d.ParentAccountKey)
                    .HasConstraintName("FK_DimAccount_DimAccount");
            });

            modelBuilder.Entity<DimCurrency>(entity =>
            {
                entity.HasKey(e => e.CurrencyKey)
                    .HasName("PK_DimCurrency_CurrencyKey");

                entity.HasIndex(e => e.CurrencyAlternateKey)
                    .HasName("AK_DimCurrency_CurrencyAlternateKey")
                    .IsUnique();

                entity.Property(e => e.CurrencyAlternateKey)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.Property(e => e.CurrencyName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<DimDate>(entity =>
            {
                entity.HasKey(e => e.DateKey)
                    .HasName("PK_DimDate_DateKey");

                entity.HasIndex(e => e.FullDateAlternateKey)
                    .HasName("AK_DimDate_FullDateAlternateKey")
                    .IsUnique();

                entity.Property(e => e.DateKey).ValueGeneratedNever();

                entity.Property(e => e.EnglishDayNameOfWeek)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.EnglishMonthName)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.FrenchDayNameOfWeek)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.FrenchMonthName)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.FullDateAlternateKey).HasColumnType("date");

                entity.Property(e => e.SpanishDayNameOfWeek)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.SpanishMonthName)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<DimDepartmentGroup>(entity =>
            {
                entity.HasKey(e => e.DepartmentGroupKey);

                entity.Property(e => e.DepartmentGroupName).HasMaxLength(50);

                entity.HasOne(d => d.ParentDepartmentGroupKeyNavigation)
                    .WithMany(p => p.InverseParentDepartmentGroupKeyNavigation)
                    .HasForeignKey(d => d.ParentDepartmentGroupKey)
                    .HasConstraintName("FK_DimDepartmentGroup_DimDepartmentGroup");
            });

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

            modelBuilder.Entity<DimOrganization>(entity =>
            {
                entity.HasKey(e => e.OrganizationKey);

                entity.Property(e => e.OrganizationName).HasMaxLength(50);

                entity.Property(e => e.PercentageOfOwnership).HasMaxLength(16);

                entity.HasOne(d => d.CurrencyKeyNavigation)
                    .WithMany(p => p.DimOrganization)
                    .HasForeignKey(d => d.CurrencyKey)
                    .HasConstraintName("FK_DimOrganization_DimCurrency");

                entity.HasOne(d => d.ParentOrganizationKeyNavigation)
                    .WithMany(p => p.InverseParentOrganizationKeyNavigation)
                    .HasForeignKey(d => d.ParentOrganizationKey)
                    .HasConstraintName("FK_DimOrganization_DimOrganization");
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

            modelBuilder.Entity<DimScenario>(entity =>
            {
                entity.HasKey(e => e.ScenarioKey);

                entity.Property(e => e.ScenarioName).HasMaxLength(50);
            });

            modelBuilder.Entity<FactFinance>(entity =>
            {
                entity.HasKey(e => e.FinanceKey);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.AccountKeyNavigation)
                    .WithMany(p => p.FactFinance)
                    .HasForeignKey(d => d.AccountKey)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FactFinance_DimAccount");

                entity.HasOne(d => d.DateKeyNavigation)
                    .WithMany(p => p.FactFinance)
                    .HasForeignKey(d => d.DateKey)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FactFinance_DimDate");

                entity.HasOne(d => d.DepartmentGroupKeyNavigation)
                    .WithMany(p => p.FactFinance)
                    .HasForeignKey(d => d.DepartmentGroupKey)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FactFinance_DimDepartmentGroup");

                entity.HasOne(d => d.OrganizationKeyNavigation)
                    .WithMany(p => p.FactFinance)
                    .HasForeignKey(d => d.OrganizationKey)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FactFinance_DimOrganization");

                entity.HasOne(d => d.ScenarioKeyNavigation)
                    .WithMany(p => p.FactFinance)
                    .HasForeignKey(d => d.ScenarioKey)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FactFinance_DimScenario");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
