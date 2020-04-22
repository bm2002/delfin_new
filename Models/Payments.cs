using System;
using System.Collections.Generic;

namespace qiwi.Models
{
    public partial class Payment
    {
        

        public int Pm_Id { get; set; }
        public DateTime Pm_CreateDate { get; set; }
        public int Pm_CreatorKey { get; set; }
        public int Pm_FilialKey { get; set; }
        public int Pm_DepartmentKey { get; set; }
        public string Pm_DocumentNumber { get; set; }
        public int Pm_Number { get; set; }
        public int Pm_Prkey { get; set; }
        public int Pm_Poid { get; set; }
        public decimal Pm_Sum { get; set; }
        public int Pm_Rakey { get; set; }
        public decimal Pm_SumNational { get; set; }
        public decimal Pm_Used { get; set; }
        public string Pm_RepresentName { get; set; }
        public string Pm_RepresentInfo { get; set; }
        public string Pm_Reason { get; set; }
        public int Pm_Export { get; set; }
        public int? Pm_Orkey { get; set; }
        public short? Pm_IsDeleted { get; set; }
        public DateTime Pm_Date { get; set; }
        public string Pm_Vdata { get; set; }

        public ICollection<PaymentDetail> PaymentDetails { get; set; }
    }
}
