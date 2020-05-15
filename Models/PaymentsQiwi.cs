using System;
using System.Collections.Generic;

namespace qiwi.Models
{
    public class PaymentsQiwi
    {
        public int Id { get; set; }
        public string DgCode { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? DateModified { get; set; }
        public string Status { get; set; }
        public string payUrl { get; set; }
        public string billId { get; set; }
        public string response { get; set; }
        public string paymentIdqiwi{ get; set; }
        public int paymentId{ get; set; }
    }

    public class PaymentsQiwiResponse
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string billIdqiwi { get; set; }
        public string paymentIdqiwi { get; set; }
        public string response { get; set; }
        public DateTime date { get; set; }
    }

    public class RequestQiwi
    {
        public int Id { get; set; }
        public string DgCode { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? DateModified { get; set; }
        public string Status { get; set; }
        public string payUrl { get; set; }
        public string billId { get; set; }
        public string request { get; set; }
    }

    public class qiwiPayment
    {
        public Paymentqiwi payment { get; set; }
        public string version { get; set; }
    }

    public class Paymentqiwi
    {
        public string paymentId { get; set; }
        public string type { get; set; }
        public DateTime createdDateTime { get; set; }
        public Status status { get; set; }
        public Amount amount { get; set; }
        public Paymentmethod paymentMethod { get; set; }
        public Gatewaydata gatewayData { get; set; }
        public Customer customer { get; set; }
        public string billId { get; set; }
        public string[] flags { get; set; }
    }

    public class Paymentmethod
    {
        public string type { get; set; }
        public string cardHolder { get; set; }
        public string cardExpireDate { get; set; }
        public string maskedPan { get; set; }
    }

    public class Gatewaydata
    {
        public string type { get; set; }
        public string authCode { get; set; }
        public string rrn { get; set; }
    }


    public class Customer
    {
        public string ip { get; set; }
        public string account { get; set; }
        public string phone { get; set; }
    }

    public class Amount
    {
        public string currency { get; set; }
        public string value { get; set; }
    }

    public class Status
    {
        public string value { get; set; }
        public DateTime changedDateTime { get; set; }
    }
}
