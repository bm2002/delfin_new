using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace delfin.mvc.api.Models.gosha
{
    public partial class goshaContext : DbContext
    {
        public goshaContext()
        {
        }

        public goshaContext(DbContextOptions<goshaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TpLists> TpLists { get; set; }
        public virtual DbSet<TpPrices> TpPrices { get; set; }
        public virtual DbSet<TpTours> TpTours { get; set; }
        public virtual DbSet<TpTurDates> TpTurDates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //Scaffold-DbContext "Data Source=192.168.0.16;Initial Catalog=gosha;Integrated Security=False;User ID=sa;Password=23#swde23#;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models/gosha -tables tp_tours,TP_Prices,TP_Lists,TP_TurDates
                //optionsBuilder.UseSqlServer("Data Source=192.168.0.16;Initial Catalog=gosha;Integrated Security=False;User ID=sa;Password=23#swde23#;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;");
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["masterTour"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TpLists>(entity =>
            {
                entity.HasKey(e => e.TiKey);

                entity.ToTable("TP_Lists");

                entity.HasIndex(e => e.TiCalculatingKey)
                    .HasName("x_tp_lists_calc");

                entity.HasIndex(e => e.TiFirstCtKey)
                    .HasName("x_ti_firstctkey");

                entity.HasIndex(e => e.TiFirstHdstars)
                    .HasName("x_ti_firsthdstars");

                entity.HasIndex(e => e.TiFirsthdkey)
                    .HasName("x_ti_firsthdkey");

                entity.HasIndex(e => e.TiFirsthrkey)
                    .HasName("x_ti_firsthrkey");

                entity.HasIndex(e => e.TiFirstpnkey)
                    .HasName("x_ti_firstpnkey");

                entity.HasIndex(e => new { e.TiTokey, e.TiFirsthotelday })
                    .HasName("x_mwfill_chkey");

                entity.HasIndex(e => new { e.TiTokey, e.TiLasthotelday })
                    .HasName("x_mwfill_chbackkey");

                entity.HasIndex(e => new { e.TiTokey, e.TiUsedBySearch })
                    .HasName("x_usedBySearch");

                entity.HasIndex(e => new { e.TiCtkeyfrom, e.TiTotaldays, e.TiTokey })
                    .HasName("x_ti_tokey");

                entity.HasIndex(e => new { e.TiKey, e.TiDays, e.TiTokey })
                    .HasName("X_TP_LISTS_2");

                entity.HasIndex(e => new { e.TiTokey, e.TiChkey, e.TiFirsthotelday })
                    .HasName("x_mwfill_ch");

                entity.HasIndex(e => new { e.TiTokey, e.TiKey, e.TiFirsthdkey })
                    .HasName("X_TP_Lists_1");

                entity.HasIndex(e => new { e.TiKey, e.TiTokey, e.TiFirsthrkey, e.TiFirsthotelpartnerkey, e.TiFirsthdkey, e.TiFirstpnkey, e.TiTotaldays })
                    .HasName("x_hd_pn_totaldays_key_inc_to_hr_pr");

                entity.Property(e => e.TiKey)
                    .HasColumnName("TI_Key")
                    .ValueGeneratedNever();

                entity.Property(e => e.TiApkeyfrom).HasColumnName("ti_apkeyfrom");

                entity.Property(e => e.TiApkeyto).HasColumnName("ti_apkeyto");

                entity.Property(e => e.TiCalculatingKey).HasColumnName("TI_CalculatingKey");

                entity.Property(e => e.TiChbackday).HasColumnName("ti_chbackday");

                entity.Property(e => e.TiChbackkey).HasColumnName("ti_chbackkey");

                entity.Property(e => e.TiChbackpkkey).HasColumnName("ti_chbackpkkey");

                entity.Property(e => e.TiChbackprkey).HasColumnName("ti_chbackprkey");

                entity.Property(e => e.TiChday).HasColumnName("ti_chday");

                entity.Property(e => e.TiChkey).HasColumnName("ti_chkey");

                entity.Property(e => e.TiChpkkey).HasColumnName("ti_chpkkey");

                entity.Property(e => e.TiChprkey).HasColumnName("ti_chprkey");

                entity.Property(e => e.TiCtkeyfrom).HasColumnName("ti_ctkeyfrom");

                entity.Property(e => e.TiCtkeyto).HasColumnName("ti_ctkeyto");

                entity.Property(e => e.TiDays).HasColumnName("TI_DAYS");

                entity.Property(e => e.TiFirstCtKey).HasColumnName("TI_FirstCtKey");

                entity.Property(e => e.TiFirstHdstars)
                    .HasColumnName("TI_FirstHDStars")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.TiFirstRsKey).HasColumnName("TI_FirstRsKey");

                entity.Property(e => e.TiFirsthdkey).HasColumnName("TI_FIRSTHDKEY");

                entity.Property(e => e.TiFirsthotelday).HasColumnName("ti_firsthotelday");

                entity.Property(e => e.TiFirsthotelpartnerkey).HasColumnName("TI_FIRSTHOTELPARTNERKEY");

                entity.Property(e => e.TiFirsthrkey).HasColumnName("TI_FIRSTHRKEY");

                entity.Property(e => e.TiFirstpnkey).HasColumnName("TI_FIRSTPNKEY");

                entity.Property(e => e.TiHdday).HasColumnName("ti_hdday");

                entity.Property(e => e.TiHdnights).HasColumnName("ti_hdnights");

                entity.Property(e => e.TiHdpartnerkey).HasColumnName("ti_hdpartnerkey");

                entity.Property(e => e.TiHotelDays)
                    .HasColumnName("TI_HotelDays")
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.Property(e => e.TiHotelKeys)
                    .HasColumnName("TI_HotelKeys")
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.Property(e => e.TiHotelroomkeys)
                    .HasColumnName("ti_hotelroomkeys")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.TiHotelstars)
                    .HasColumnName("ti_hotelstars")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.TiLasthotelday).HasColumnName("ti_lasthotelday");

                entity.Property(e => e.TiName)
                    .HasColumnName("TI_Name")
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.Property(e => e.TiNights).HasColumnName("ti_nights");

                entity.Property(e => e.TiPansionKeys)
                    .HasColumnName("TI_PansionKeys")
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.Property(e => e.TiSecondCtKey).HasColumnName("TI_SecondCtKey");

                entity.Property(e => e.TiSecondHdkey).HasColumnName("TI_SecondHDKey");

                entity.Property(e => e.TiSecondHdstars)
                    .HasColumnName("TI_SecondHDStars")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.TiSecondHrkey).HasColumnName("TI_SecondHRKey");

                entity.Property(e => e.TiSecondPnkey).HasColumnName("TI_SecondPNKey");

                entity.Property(e => e.TiSecondRsKey).HasColumnName("TI_SecondRsKey");

                entity.Property(e => e.TiTokey).HasColumnName("TI_TOKey");

                entity.Property(e => e.TiTotaldays).HasColumnName("ti_totaldays");

                entity.Property(e => e.TiUpdate)
                    .HasColumnName("TI_UPDATE")
                    .HasDefaultValueSql("(0)");

                entity.Property(e => e.TiUsedBySearch).HasColumnName("ti_UsedBySearch");

                entity.HasOne(d => d.TiTokeyNavigation)
                    .WithMany(p => p.TpLists)
                    .HasForeignKey(d => d.TiTokey)
                    .HasConstraintName("FK_TI_TOKey");   
            });

            modelBuilder.Entity<TpPrices>(entity =>
            {
                entity.HasKey(e => new { e.TpDateBegin, e.TpTikey })
                    .HasName("PK_TP_DateBegin_TP_TIKey");

                entity.ToTable("TP_Prices");

                entity.HasIndex(e => new { e.TpTikey, e.TpKey })
                    .HasName("tp_prices_incl_tp_tikey");

                entity.HasIndex(e => new { e.TpKey, e.TpTikey, e.TpDateBegin })
                    .HasName("IX_TP_Prices_incl_TP_Key_forPriceList");

                entity.HasIndex(e => new { e.TpKey, e.TpDateBegin, e.TpTikey, e.TpTokey, e.TpDateEnd })
                    .HasName("IX_tp_prices__tokey_dateend");

                entity.Property(e => e.TpDateBegin)
                    .HasColumnName("TP_DateBegin")
                    .HasColumnType("datetime");

                entity.Property(e => e.TpTikey).HasColumnName("TP_TIKey");

                entity.Property(e => e.TpCalculatingKey).HasColumnName("TP_CalculatingKey");

                entity.Property(e => e.TpDateEnd)
                    .HasColumnName("TP_DateEnd")
                    .HasColumnType("datetime");

                entity.Property(e => e.TpGross).HasColumnName("TP_Gross");

                entity.Property(e => e.TpKey).HasColumnName("TP_Key");

                entity.Property(e => e.TpTokey).HasColumnName("TP_TOKey");

                entity.HasOne(d => d.TpTokeyNavigation)
                    .WithMany(p => p.TpPrices)
                    .HasForeignKey(d => d.TpTokey)
                    .HasConstraintName("FK_TP_TOKey");
            });

            modelBuilder.Entity<TpTours>(entity =>
            {
                entity.HasKey(e => e.ToKey);

                entity.ToTable("TP_Tours");

                entity.HasIndex(e => e.ToCnkey)
                    .HasName("x_to_cnkey");

                entity.HasIndex(e => e.ToDateBegin)
                    .HasName("x_to_datebegin");

                entity.HasIndex(e => e.ToDateEnd)
                    .HasName("x_to_dateend");

                entity.HasIndex(e => e.ToDateValid)
                    .HasName("x_to_datevalid");

                entity.HasIndex(e => e.ToTrkey)
                    .HasName("x_to_trkey");

                entity.HasIndex(e => new { e.ToCnkey, e.ToTrkey, e.ToIsEnabled })
                    .HasName("x_mwTourList");

                entity.Property(e => e.ToKey)
                    .HasColumnName("TO_Key")
                    .ValueGeneratedNever();

                entity.Property(e => e.ToAttribute)
                    .HasColumnName("to_attribute")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ToCalculateDateEnd)
                    .HasColumnName("TO_CalculateDateEnd")
                    .HasColumnType("datetime");

                entity.Property(e => e.ToCnkey).HasColumnName("TO_CNKey");

                entity.Property(e => e.ToDateBegin)
                    .HasColumnName("TO_DateBegin")
                    .HasColumnType("datetime");

                entity.Property(e => e.ToDateCreated)
                    .HasColumnName("TO_DateCreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.ToDateEnd)
                    .HasColumnName("TO_DateEnd")
                    .HasColumnType("datetime");

                entity.Property(e => e.ToDateValid)
                    .HasColumnName("TO_DateValid")
                    .HasColumnType("datetime");

                entity.Property(e => e.ToDateValidBegin)
                    .HasColumnName("TO_DateValidBegin")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ToHotelNights)
                    .HasColumnName("TO_HotelNights")
                    .HasMaxLength(256);

                entity.Property(e => e.ToIsEnabled).HasColumnName("TO_IsEnabled");

                entity.Property(e => e.ToMinPrice).HasColumnName("TO_MinPrice");

                entity.Property(e => e.ToName)
                    .HasColumnName("TO_Name")
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.ToOpKey)
                    .HasColumnName("TO_OpKey")
                    .HasDefaultValueSql("(0)");

                entity.Property(e => e.ToPriceCount).HasColumnName("TO_PriceCount");

                entity.Property(e => e.ToPriceFor)
                    .HasColumnName("TO_PriceFor")
                    .HasDefaultValueSql("(0)");

                entity.Property(e => e.ToPrkey).HasColumnName("TO_PRKey");

                entity.Property(e => e.ToProgress).HasColumnName("TO_PROGRESS");

                entity.Property(e => e.ToRate)
                    .HasColumnName("TO_Rate")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.ToTrkey).HasColumnName("TO_TRKey");

                entity.Property(e => e.ToUpdate).HasColumnName("TO_UPDATE");

                entity.Property(e => e.ToUpdatetime)
                    .HasColumnName("TO_UPDATETIME")
                    .HasColumnType("datetime");

                entity.Property(e => e.ToXml)
                    .HasColumnName("TO_XML")
                    .HasColumnType("image");
            });

            modelBuilder.Entity<TpTurDates>(entity =>
            {
                entity.HasKey(e => e.TdKey);

                entity.ToTable("TP_TurDates");

                entity.HasIndex(e => e.TdCalculatingKey)
                    .HasName("x_tp_turdates_calc");

                entity.HasIndex(e => new { e.TdTokey, e.TdDate })
                    .HasName("x_main");

                entity.Property(e => e.TdKey)
                    .HasColumnName("TD_Key")
                    .ValueGeneratedNever();

                entity.Property(e => e.TdAutodisabled).HasColumnName("td_autodisabled");

                entity.Property(e => e.TdCalculatingKey).HasColumnName("TD_CalculatingKey");

                entity.Property(e => e.TdCheckmargin)
                    .HasColumnName("TD_CHECKMARGIN")
                    .HasDefaultValueSql("(0)");

                entity.Property(e => e.TdDate)
                    .HasColumnName("TD_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.TdTokey).HasColumnName("TD_TOKey");

                entity.Property(e => e.TdUpdate)
                    .HasColumnName("TD_UPDATE")
                    .HasDefaultValueSql("(0)");

                entity.HasOne(d => d.TdTokeyNavigation)
                    .WithMany(p => p.TpTurDates)
                    .HasForeignKey(d => d.TdTokey)
                    .HasConstraintName("FK_TP_TurDates_TP_Tours");
            });

            modelBuilder.Entity<TpLists>()
                .HasMany(c => c.TpPrices)
                .WithOne(e => e.TpList)
                .HasForeignKey(p => p.TpTikey);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
