using System;
using System.Collections.Generic;

namespace delfin.mvc.api.Models.gosha
{
    public partial class TpTours
    {
        public TpTours()
        {
            TpLists = new HashSet<TpLists>();
            TpPrices = new HashSet<TpPrices>();
            TpTurDates = new HashSet<TpTurDates>();
        }

        public int ToKey { get; set; }
        public int ToTrkey { get; set; }
        public string ToName { get; set; }
        public int ToPrkey { get; set; }
        public int ToCnkey { get; set; }
        public string ToRate { get; set; }
        public DateTime? ToDateCreated { get; set; }
        public DateTime? ToDateValid { get; set; }
        public short ToPriceFor { get; set; }
        public int ToOpKey { get; set; }
        public byte[] ToXml { get; set; }
        public DateTime? ToDateBegin { get; set; }
        public DateTime? ToDateEnd { get; set; }
        public short? ToIsEnabled { get; set; }
        public int? ToProgress { get; set; }
        public int? ToUpdate { get; set; }
        public DateTime? ToUpdatetime { get; set; }
        public DateTime? ToDateValidBegin { get; set; }
        public DateTime? ToCalculateDateEnd { get; set; }
        public int? ToPriceCount { get; set; }
        public int? ToAttribute { get; set; }
        public double? ToMinPrice { get; set; }
        public string ToHotelNights { get; set; }

        public virtual ICollection<TpLists> TpLists { get; set; }
        public virtual ICollection<TpPrices> TpPrices { get; set; }
        public virtual ICollection<TpTurDates> TpTurDates { get; set; }
    }
}
