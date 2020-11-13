using System;
using System.Collections.Generic;

namespace delfin.mvc.api.Models.gosha
{
    public partial class TpPrices
    {
        public int TpKey { get; set; }
        public int TpTokey { get; set; }
        public DateTime TpDateBegin { get; set; }
        public DateTime? TpDateEnd { get; set; }
        public double? TpGross { get; set; }
        public int TpTikey { get; set; }
        public int? TpCalculatingKey { get; set; }

        public virtual TpTours TpTokeyNavigation { get; set; }

        public virtual TpLists TpList { get; set; }
}
}
