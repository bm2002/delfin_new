﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using qiwi.Models;

namespace qiwi.Migrations
{
    [DbContext(typeof(dbContext))]
    partial class dbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("qiwi.Models.Payment", b =>
                {
                    b.Property<int>("Pm_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Pm_CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Pm_CreatorKey")
                        .HasColumnType("int");

                    b.Property<DateTime>("Pm_Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Pm_DepartmentKey")
                        .HasColumnType("int");

                    b.Property<string>("Pm_DocumentNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Pm_Export")
                        .HasColumnType("int");

                    b.Property<int>("Pm_FilialKey")
                        .HasColumnType("int");

                    b.Property<short?>("Pm_IsDeleted")
                        .HasColumnType("smallint");

                    b.Property<int>("Pm_Number")
                        .HasColumnType("int");

                    b.Property<int?>("Pm_Orkey")
                        .HasColumnType("int");

                    b.Property<int>("Pm_Poid")
                        .HasColumnType("int");

                    b.Property<int>("Pm_Prkey")
                        .HasColumnType("int");

                    b.Property<int>("Pm_Rakey")
                        .HasColumnType("int");

                    b.Property<string>("Pm_Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pm_RepresentInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pm_RepresentName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Pm_Sum")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Pm_SumNational")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Pm_Used")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Pm_Vdata")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Pm_Id")
                        .HasName("PK_Payments");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("qiwi.Models.PaymentDetail", b =>
                {
                    b.Property<int>("Pd_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Pd_Course")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Pd_CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Pd_CreatorKey")
                        .HasColumnType("int");

                    b.Property<DateTime>("Pd_Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Pd_Dgkey")
                        .HasColumnType("int");

                    b.Property<decimal>("Pd_Percent")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Pd_Pmid")
                        .HasColumnType("int");

                    b.Property<string>("Pd_Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Pd_Sum")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Pd_SumInDogovorRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Pd_SumNational")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Pd_SumNationalWords")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Pd_SumTax1")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Pd_SumTax2")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Pd_SumTaxPercent1")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("Pd_SumTaxPercent2")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Pd_Id")
                        .HasName("PK_PaymentDetails");

                    b.HasIndex("Pd_Pmid");

                    b.ToTable("PaymentDetails");
                });

            modelBuilder.Entity("qiwi.Models.PaymentsQiwi", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal?>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("DateCreate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("DgCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PaymentKey")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("_PaymentsQiwies");
                });

            modelBuilder.Entity("qiwi.Models.PaymentDetail", b =>
                {
                    b.HasOne("qiwi.Models.Payment", "Pd_Pm")
                        .WithMany("PaymentDetails")
                        .HasForeignKey("Pd_Pmid")
                        .HasConstraintName("FK_PaymentDetails_Payments")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
