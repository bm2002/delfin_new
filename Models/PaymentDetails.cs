using System;
using System.Collections.Generic;

namespace qiwi.Models
{
    public partial class PaymentDetail
    {
        public int Pd_Id { get; set; }
        public DateTime Pd_CreateDate { get; set; }
        public int Pd_CreatorKey { get; set; }
        public DateTime Pd_Date { get; set; }
        public decimal Pd_Course { get; set; }
        public decimal Pd_Percent { get; set; }
        public decimal Pd_Sum { get; set; }
        public decimal Pd_SumNational { get; set; }
        public decimal? Pd_SumInDogovorRate { get; set; }
        public decimal? Pd_SumTax1 { get; set; }
        public decimal? Pd_SumTaxPercent1 { get; set; }
        public decimal? Pd_SumTax2 { get; set; }
        public decimal? Pd_SumTaxPercent2 { get; set; }
        public string Pd_SumNationalWords { get; set; }
        public string Pd_Reason { get; set; }
        public int? Pd_Dgkey { get; set; }
        public int Pd_Pmid { get; set; }

        public Payment Pd_Pm { get; set; }
    }
}
