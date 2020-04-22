﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using qiwi.Models;

namespace qiwi.Migrations
{
    [DbContext(typeof(dbContext))]
    [Migration("20200421165936_21042020-4")]
    partial class _210420204
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("qiwi.Models.Payment", b =>
                {
                    b.Property<int>("PmId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PM_Id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("PmCreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PM_CreateDate")
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("PmCreatorKey")
                        .HasColumnName("PM_CreatorKey")
                        .HasColumnType("int");

                    b.Property<DateTime>("PmDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PM_Date")
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("PmDepartmentKey")
                        .HasColumnName("PM_DepartmentKey")
                        .HasColumnType("int");

                    b.Property<string>("PmDocumentNumber")
                        .IsRequired()
                        .HasColumnName("PM_DocumentNumber")
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.Property<int>("PmExport")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PM_Export")
                        .HasColumnType("int")
                        .HasDefaultValueSql("((1))");

                    b.Property<int>("PmFilialKey")
                        .HasColumnName("PM_FilialKey")
                        .HasColumnType("int");

                    b.Property<short?>("PmIsDeleted")
                        .HasColumnName("PM_IsDeleted")
                        .HasColumnType("smallint");

                    b.Property<int>("PmNumber")
                        .HasColumnName("PM_Number")
                        .HasColumnType("int");

                    b.Property<int?>("PmOrkey")
                        .HasColumnName("PM_ORKey")
                        .HasColumnType("int");

                    b.Property<int>("PmPoid")
                        .HasColumnName("PM_POId")
                        .HasColumnType("int");

                    b.Property<int>("PmPrkey")
                        .HasColumnName("PM_PRKey")
                        .HasColumnType("int");

                    b.Property<int>("PmRakey")
                        .HasColumnName("PM_RAKey")
                        .HasColumnType("int");

                    b.Property<string>("PmReason")
                        .HasColumnName("PM_Reason")
                        .HasColumnType("varchar(4000)")
                        .HasMaxLength(4000)
                        .IsUnicode(false);

                    b.Property<string>("PmRepresentInfo")
                        .HasColumnName("PM_RepresentInfo")
                        .HasColumnType("varchar(120)")
                        .HasMaxLength(120)
                        .IsUnicode(false);

                    b.Property<string>("PmRepresentName")
                        .HasColumnName("PM_RepresentName")
                        .HasColumnType("varchar(120)")
                        .HasMaxLength(120)
                        .IsUnicode(false);

                    b.Property<decimal>("PmSum")
                        .HasColumnName("PM_Sum")
                        .HasColumnType("money");

                    b.Property<decimal>("PmSumNational")
                        .HasColumnName("PM_SumNational")
                        .HasColumnType("money");

                    b.Property<decimal>("PmUsed")
                        .HasColumnName("PM_Used")
                        .HasColumnType("money");

                    b.Property<string>("PmVdata")
                        .HasColumnName("PM_VData")
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.HasKey("PmId");

                    b.HasIndex("PmIsDeleted", "PmPoid", "PmFilialKey", "PmCreatorKey", "PmDate")
                        .HasName("IX_PM_DEL_PO_FI_CR_DATE");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("qiwi.Models.PaymentDetail", b =>
                {
                    b.Property<int>("PdId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PD_Id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("PdCourse")
                        .HasColumnName("PD_Course")
                        .HasColumnType("decimal(19, 6)");

                    b.Property<DateTime>("PdCreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PD_CreateDate")
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("PdCreatorKey")
                        .HasColumnName("PD_CreatorKey")
                        .HasColumnType("int");

                    b.Property<DateTime>("PdDate")
                        .HasColumnName("PD_Date")
                        .HasColumnType("datetime");

                    b.Property<int?>("PdDgkey")
                        .HasColumnName("PD_DGKey")
                        .HasColumnType("int");

                    b.Property<decimal>("PdPercent")
                        .HasColumnName("PD_Percent")
                        .HasColumnType("money");

                    b.Property<int>("PdPmid")
                        .HasColumnName("PD_PMId")
                        .HasColumnType("int");

                    b.Property<string>("PdReason")
                        .HasColumnName("PD_Reason")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<decimal>("PdSum")
                        .HasColumnName("PD_Sum")
                        .HasColumnType("money");

                    b.Property<decimal?>("PdSumInDogovorRate")
                        .HasColumnName("PD_SumInDogovorRate")
                        .HasColumnType("money");

                    b.Property<decimal>("PdSumNational")
                        .HasColumnName("PD_SumNational")
                        .HasColumnType("money");

                    b.Property<string>("PdSumNationalWords")
                        .HasColumnName("PD_SumNationalWords")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<decimal?>("PdSumTax1")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PD_SumTax1")
                        .HasColumnType("money")
                        .HasDefaultValueSql("((0))");

                    b.Property<decimal?>("PdSumTax2")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PD_SumTax2")
                        .HasColumnType("money")
                        .HasDefaultValueSql("((0))");

                    b.Property<decimal?>("PdSumTaxPercent1")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PD_SumTaxPercent1")
                        .HasColumnType("money")
                        .HasDefaultValueSql("((0))");

                    b.Property<decimal?>("PdSumTaxPercent2")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("PD_SumTaxPercent2")
                        .HasColumnType("money")
                        .HasDefaultValueSql("((1))");

                    b.HasKey("PdId");

                    b.HasIndex("PdDgkey")
                        .HasName("IX_PD_DGKey");

                    b.HasIndex("PdPmid")
                        .HasName("IX_PD_PMId");

                    b.ToTable("PaymentDetails");
                });

            modelBuilder.Entity("qiwi.Models.PaymentsQiwi", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal?>("Amount")
                        .HasColumnType("money");

                    b.Property<DateTime?>("DateCreate")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime");

                    b.Property<string>("DgCode")
                        .HasColumnName("DG_Code")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<int?>("PaymentKey")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("_PaymentsUniteller");
                });

            modelBuilder.Entity("qiwi.Models.PaymentDetail", b =>
                {
                    b.HasOne("qiwi.Models.Payment", "PdPm")
                        .WithMany("PaymentDetails")
                        .HasForeignKey("PdPmid")
                        .HasConstraintName("FK_PaymentDetails_Payments")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
