using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace qiwi.Models
{
    public partial class dbContext : DbContext
    {
        public dbContext()
        {
        }

        public class avia
        {
            public int Id { get; set; }

            public string name { get; set;  }
        }

        public dbContext(DbContextOptions<dbContext> options)
            : base(options)
        {
        }

        //public DbSet<avia> avias { get; set; }

        public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentsQiwi> _PaymentsQiwies { get; set; }
        public DbSet<PaymentsQiwiResponse> _PaymentsQiwiResponses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["masterTour"].ConnectionString);
                //optionsBuilder.UseSqlServer("Data Source=192.168.0.16;Initial Catalog=goshaTT;Integrated Security=False;User ID=sa;Password=23#swde23#;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>()
               .HasKey(c => new { c.Pm_Id })
               .HasName("PK_Payments");

            modelBuilder.Entity<PaymentDetail>()
               .HasKey(c => new { c.Pd_Id })
               .HasName("PK_PaymentDetails");

            modelBuilder.Entity<PaymentDetail>()
                .HasOne(d => d.Pd_Pm)
                .WithMany(p => p.PaymentDetails)
                .HasForeignKey(d => d.Pd_Pmid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PaymentDetails_Payments");

            //modelBuilder.Entity<PaymentsQiwi>()
            //    .HasIndex(x => x.DgCode).IsUnique();

            //modelBuilder.Entity<PaymentsQiwi>()
            //    .HasAlternateKey(x => x.DgCode)
            //    .HasName("constrait");

            //modelBuilder.Entity<PaymentsQiwi>()
            //    .HasCheckConstraint(x => x.)


            //modelBuilder.Entity<PaymentDetail>(entity =>
            //{
            //    entity.HasOne(d => d.Pd_Pm)
            //        .WithMany(p => p.PaymentDetails)
            //        .HasForeignKey(d => d.PdPmid)
            //        .HasConstraintName("FK_PaymentDetails_Payments");
            //};

            //modelBuilder.Entity<Payment>()
            //        //.HasOne(d => d.Pd_Pm)
            //        //.WithMany(p => p.PaymentDetails)
            //        .HasForeignKey(d => d.Pd_Pmid)
            //        .HasConstraintName("FK_PaymentDetails_Payments");

            //modelBuilder.Entity<PaymentDetail>(entity =>
            //{
            //    entity.HasKey(e => e.PdId);

            //    entity.HasIndex(e => e.PdDgkey)
            //        .HasName("IX_PD_DGKey");

            //    entity.HasIndex(e => e.PdPmid)
            //        .HasName("IX_PD_PMId");

            //    entity.Property(e => e.PdId).HasColumnName("PD_Id");

            //    entity.Property(e => e.PdCourse)
            //        .HasColumnName("PD_Course")
            //        .HasColumnType("decimal(19, 6)");

            //    entity.Property(e => e.PdCreateDate)
            //        .HasColumnName("PD_CreateDate")
            //        .HasColumnType("datetime")
            //        .HasDefaultValueSql("(getdate())");

            //    entity.Property(e => e.PdCreatorKey).HasColumnName("PD_CreatorKey");

            //    entity.Property(e => e.PdDate)
            //        .HasColumnName("PD_Date")
            //        .HasColumnType("datetime");

            //    entity.Property(e => e.PdDgkey).HasColumnName("PD_DGKey");

            //    entity.Property(e => e.PdPercent)
            //        .HasColumnName("PD_Percent")
            //        .HasColumnType("money");

            //    entity.Property(e => e.PdPmid).HasColumnName("PD_PMId");

            //    entity.Property(e => e.PdReason)
            //        .HasColumnName("PD_Reason")
            //        .HasMaxLength(255)
            //        .IsUnicode(false);

            //    entity.Property(e => e.PdSum)
            //        .HasColumnName("PD_Sum")
            //        .HasColumnType("money");

            //    entity.Property(e => e.PdSumInDogovorRate)
            //        .HasColumnName("PD_SumInDogovorRate")
            //        .HasColumnType("money");

            //    entity.Property(e => e.PdSumNational)
            //        .HasColumnName("PD_SumNational")
            //        .HasColumnType("money");

            //    entity.Property(e => e.PdSumNationalWords)
            //        .HasColumnName("PD_SumNationalWords")
            //        .HasMaxLength(255)
            //        .IsUnicode(false);

            //    entity.Property(e => e.PdSumTax1)
            //        .HasColumnName("PD_SumTax1")
            //        .HasColumnType("money")
            //        .HasDefaultValueSql("((0))");

            //    entity.Property(e => e.PdSumTax2)
            //        .HasColumnName("PD_SumTax2")
            //        .HasColumnType("money")
            //        .HasDefaultValueSql("((0))");

            //    entity.Property(e => e.PdSumTaxPercent1)
            //        .HasColumnName("PD_SumTaxPercent1")
            //        .HasColumnType("money")
            //        .HasDefaultValueSql("((0))");

            //    entity.Property(e => e.PdSumTaxPercent2)
            //        .HasColumnName("PD_SumTaxPercent2")
            //        .HasColumnType("money")
            //        .HasDefaultValueSql("((1))");

            //    entity.HasOne(d => d.PdPm)
            //        .WithMany(p => p.PaymentDetails)
            //        .HasForeignKey(d => d.PdPmid)
            //        .HasConstraintName("FK_PaymentDetails_Payments");
            //});

            //modelBuilder.Entity<Payment>(entity =>
            //{
            //    entity.HasKey(e => e.PmId);

            //    entity.HasIndex(e => new { e.PmIsDeleted, e.PmPoid, e.PmFilialKey, e.PmCreatorKey, e.PmDate })
            //        .HasName("IX_PM_DEL_PO_FI_CR_DATE");

            //    entity.Property(e => e.PmId).HasColumnName("PM_Id");

            //    entity.Property(e => e.PmCreateDate)
            //        .HasColumnName("PM_CreateDate")
            //        .HasColumnType("datetime")
            //        .HasDefaultValueSql("(getdate())");

            //    entity.Property(e => e.PmCreatorKey).HasColumnName("PM_CreatorKey");

            //    entity.Property(e => e.PmDate)
            //        .HasColumnName("PM_Date")
            //        .HasColumnType("datetime")
            //        .HasDefaultValueSql("(getdate())");

            //    entity.Property(e => e.PmDepartmentKey).HasColumnName("PM_DepartmentKey");

            //    entity.Property(e => e.PmDocumentNumber)
            //        .IsRequired()
            //        .HasColumnName("PM_DocumentNumber")
            //        .HasMaxLength(20)
            //        .IsUnicode(false);

            //    entity.Property(e => e.PmExport)
            //        .HasColumnName("PM_Export")
            //        .HasDefaultValueSql("((1))");

            //    entity.Property(e => e.PmFilialKey).HasColumnName("PM_FilialKey");

            //    entity.Property(e => e.PmIsDeleted).HasColumnName("PM_IsDeleted");

            //    entity.Property(e => e.PmNumber).HasColumnName("PM_Number");

            //    entity.Property(e => e.PmOrkey).HasColumnName("PM_ORKey");

            //    entity.Property(e => e.PmPoid).HasColumnName("PM_POId");

            //    entity.Property(e => e.PmPrkey).HasColumnName("PM_PRKey");

            //    entity.Property(e => e.PmRakey).HasColumnName("PM_RAKey");

            //    entity.Property(e => e.PmReason)
            //        .HasColumnName("PM_Reason")
            //        .HasMaxLength(4000)
            //        .IsUnicode(false);

            //    entity.Property(e => e.PmRepresentInfo)
            //        .HasColumnName("PM_RepresentInfo")
            //        .HasMaxLength(120)
            //        .IsUnicode(false);

            //    entity.Property(e => e.PmRepresentName)
            //        .HasColumnName("PM_RepresentName")
            //        .HasMaxLength(120)
            //        .IsUnicode(false);

            //    entity.Property(e => e.PmSum)
            //        .HasColumnName("PM_Sum")
            //        .HasColumnType("money");

            //    entity.Property(e => e.PmSumNational)
            //        .HasColumnName("PM_SumNational")
            //        .HasColumnType("money");

            //    entity.Property(e => e.PmUsed)
            //        .HasColumnName("PM_Used")
            //        .HasColumnType("money");

            //    entity.Property(e => e.PmVdata)
            //        .HasColumnName("PM_VData")
            //        .HasMaxLength(100)
            //        .IsUnicode(false);
            //});

            //modelBuilder.Entity<PaymentsQiwi>(entity =>
            //{
            //    entity.ToTable("_PaymentsUniteller");

            //    entity.Property(e => e.Amount).HasColumnType("money");

            //    entity.Property(e => e.DateCreate).HasColumnType("datetime");

            //    entity.Property(e => e.DateModified).HasColumnType("datetime");

            //    entity.Property(e => e.DgCode)
            //        .HasColumnName("DG_Code")
            //        .HasMaxLength(50)
            //        .IsUnicode(false);

            //    entity.Property(e => e.Status)
            //        .HasMaxLength(50)
            //        .IsUnicode(false);
            //});

            //OnModelCreatingPartial(modelBuilder);
        }

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
