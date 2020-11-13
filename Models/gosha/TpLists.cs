using System;
using System.Collections.Generic;

namespace delfin.mvc.api.Models.gosha
{
    public partial class TpLists
    {
        public int TiKey { get; set; }
        public int TiTokey { get; set; }
        public string TiName { get; set; }
        public int? TiDays { get; set; }
        public int? TiFirsthdkey { get; set; }
        public int? TiFirsthrkey { get; set; }
        public int? TiFirstpnkey { get; set; }
        public string TiHotelKeys { get; set; }
        public string TiPansionKeys { get; set; }
        public string TiHotelDays { get; set; }
        public string TiFirstHdstars { get; set; }
        public int? TiFirstCtKey { get; set; }
        public int? TiFirstRsKey { get; set; }
        public int? TiSecondHdkey { get; set; }
        public int? TiSecondHrkey { get; set; }
        public int? TiSecondPnkey { get; set; }
        public string TiSecondHdstars { get; set; }
        public int? TiSecondCtKey { get; set; }
        public int? TiSecondRsKey { get; set; }
        public short? TiUpdate { get; set; }
        public int? TiFirsthotelpartnerkey { get; set; }
        public int? TiCtkeyfrom { get; set; }
        public int? TiCtkeyto { get; set; }
        public int? TiApkeyfrom { get; set; }
        public int? TiApkeyto { get; set; }
        public int? TiFirsthotelday { get; set; }
        public int? TiHdpartnerkey { get; set; }
        public int? TiTotaldays { get; set; }
        public int? TiNights { get; set; }
        public int? TiLasthotelday { get; set; }
        public int? TiChkey { get; set; }
        public int? TiChbackkey { get; set; }
        public int? TiHdday { get; set; }
        public int? TiHdnights { get; set; }
        public int? TiChday { get; set; }
        public int? TiChbackday { get; set; }
        public int? TiChpkkey { get; set; }
        public int? TiChprkey { get; set; }
        public int? TiChbackpkkey { get; set; }
        public int? TiChbackprkey { get; set; }
        public string TiHotelroomkeys { get; set; }
        public string TiHotelstars { get; set; }
        public short? TiUsedBySearch { get; set; }
        public int? TiCalculatingKey { get; set; }

        public virtual TpTours TiTokeyNavigation { get; set; }

        public virtual List<TpPrices> TpPrices { get; set; }
    }
}
