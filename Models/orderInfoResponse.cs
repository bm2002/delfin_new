using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace delfin.mvc.api.Models
{
    public class orderInfoResponse
    {

        public class orderResponse
        {
            public onedog[] Property1 { get; set; }
        }

        public class onedog
        {
            public string Code { get; set; }
            public string State { get; set; }
            public Cost Cost { get; set; }
            public Fee Fee { get; set; }
            public Request Request { get; set; }
            public Service[] Services { get; set; }
            public object[] AddableServices { get; set; }
            public int AvailableRooms { get; set; }
            public string CreateDate { get; set; }
            public object ExtendState { get; set; }
        }

        public class Cost
        {
            public decimal Netto { get; set; } 
            public decimal Brutto { get; set; }
            public decimal Fee { get; set; }
            public string Rate { get; set; }

        }

        public class Fee
        {
            public string Case { get; set; }
            public decimal[] Fields { get; set; }
        }

        public class Request
        {
            public object Access { get; set; }
            public Guest[] Guests { get; set; }
            public Pricekey PriceKey { get; set; }
            public int Adults { get; set; }
            public object[] ChildAges { get; set; }
            public object[] AddedServices { get; set; }
            public object[] RemovedServices { get; set; }
            public Customer Customer { get; set; }
            public int Provider { get; set; }
            public object DepartureVenue { get; set; }
            public object ArrivalVenue { get; set; }
            public object[] Related { get; set; }
        }

        public class Pricekey
        {
            public int Hotel { get; set; }
            public int Tour { get; set; }

            public int Pansion { get; set; }
            public int Room { get; set; }
            public int RoomCat { get; set; }
            public string Date { get; set; }
            public short Nights { get; set; }
            public int[] Beds { get; set; }
        }

        public class Customer
        {
            public string Case { get; set; }
            public Field[] Fields { get; set; }
        }

        public class Field
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool IsCoordinator { get; set; }
            public bool IsActive { get; set; }
            public bool IsSupervisor { get; set; }
            public Agency Agency { get; set; }
        }

        public class Agency
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Attr { get; set; }
            public int City { get; set; }
        }

        public class Guest
        {
            public bool IsMale { get; set; }
            public Latin Latin { get; set; }
            public Cyrilic Cyrilic { get; set; }
            public string Birthday { get; set; }
            public string DocumentNumber { get; set; }
            public string DocumentSerial { get; set; }
            public string Phone { get; set; }
            public string Citizenship { get; set; }
            public int Id { get; set; }
        }

        public class Latin
        {
            public string SurName { get; set; }
            public string Name { get; set; }
            public string Patronymic { get; set; }
        }

        public class Cyrilic
        {
            public string SurName { get; set; }
            public string Name { get; set; }
            public string Patronymic { get; set; }
        }

        public class Service
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string StartDate { get; set; }
            public int Persons { get; set; }
            public short? Duration { get; set; }
            public decimal? Brutto { get; set; }
            public decimal? Fee { get; set; }
            public bool IsInvisible { get; set; }
            public bool IsDeletable { get; set; }
        }

    }
}