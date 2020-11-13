using System;
using System.Collections.Generic;

namespace delfin.mvc.api.Models.gosha
{
    public partial class TpTurDates
    {
        public int TdKey { get; set; }
        public int TdTokey { get; set; }
        public DateTime TdDate { get; set; }
        public short? TdUpdate { get; set; }
        public short? TdCheckmargin { get; set; }
        public short? TdAutodisabled { get; set; }
        public int? TdCalculatingKey { get; set; }

        public virtual TpTours TdTokeyNavigation { get; set; }
    }
}
