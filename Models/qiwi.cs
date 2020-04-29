using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qiwi.Models
{
    public partial class qiwi
    {
        public class qiwirequest
        {
            public Amount amount { get; set; }
            public DateTime expirationDateTime { get; set; }
        }

        public class Amount
        {
            public string currency { get; set; }
            public string value { get; set; }
        }


        public class qiwiresponse
        {
            public string siteId { get; set; }
            public string billId { get; set; }
            public Amount2 amount { get; set; }
            public Status status { get; set; }
            public DateTime creationDateTime { get; set; }
            public DateTime expirationDateTime { get; set; }
            public string payUrl { get; set; }
        }

        public class Amount2
        {
            public string currency { get; set; }
            public string value { get; set; }
        }

        public class Status
        {
            public string value { get; set; }
            public DateTime changedDateTime { get; set; }
        }


        public class qiwiPayment
        {
            public Payment payment { get; set; }
            public string version { get; set; }
        }

        public class Payment
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



        public class qiwiBill
        {
            public Bill bill { get; set; }
            public string version { get; set; }
        }

        public class Bill
        {
            public string siteId { get; set; }
            public string billId { get; set; }
            public Amount amount { get; set; }
            public Status status { get; set; }
            public Customer customer { get; set; }
            public Customfields customFields { get; set; }
            public DateTime creationDateTime { get; set; }
            public string comment { get; set; }
            public string expirationDateTime { get; set; }
        }



        public class Customfields
        {
        }
    }
}