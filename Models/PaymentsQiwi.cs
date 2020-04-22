using System;
using System.Collections.Generic;

namespace qiwi.Models
{
    public partial class PaymentsQiwi
    {
        public int Id { get; set; }
        public string DgCode { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? DateModified { get; set; }
        public string Status { get; set; }
        public int? PaymentKey { get; set; }
    }
}
