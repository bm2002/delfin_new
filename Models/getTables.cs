using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace qiwi.Models
{

    public class turlistsshort
    {
        public int TL_KEY { get; set; }
        public string TL_NAME { get; set; }
        public string TL_NAMEWEB { get; set; }
        public short? TL_NDAY { get; set; }
        public int? TL_WEB { get; set; }
        public short? TL_IsDisabled { get; set; }
        public short? TL_Deleted { get; set; }
        public string TL_EMAIL { get; set; }
        public int TL_TIP { get; set; }
        public int? TL_CNKEY { get; set; }
        public int? TL_OpKey { get; set; }
        public int? TL_LEADDEPARTMENT { get; set; }
        public string TL_WEBHTTP { get; set; }
        public long? ROWID { get; set; }
    }

    public class hotelshort
    {
        public int HD_KEY { get; set; }
        public string HD_NAME { get; set; }
        public int HD_CNKEY { get; set; }
        public int HD_CTKEY { get; set; }
        public int? HD_RSKEY { get; set; }
        public int? HD_COHID { get; set; }
        public string HD_ADRESS { get; set; }
        public string HD_PHONE { get; set; }
        public string HD_FAX { get; set; }
        public string HD_EMAIL { get; set; }
        public string HD_HTTP { get; set; }
        public long? ROWID { get; set; }
    }

    public class gdth
    {
        public int tour_id { get; set; }
        public int hotel_id { get; set; }
    }

    public class nodejson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
    }

    public class dictionaryjson
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class hotelcontent2
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Stars { get; set; }
        public string Type { get; set; }
        public string Contacts { get; set; }
        public string Adress { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int Country { get; set; }
        public int Region { get; set; }
        public int City { get; set; }
        public int Distinct { get; set; }
        public string CheckoutTime { get; set; }
        public string CheckinTime { get; set; }
        public string CheckinServiceMode { get; set; }
        public string CheckinServiceFrom { get; set; }
        public string CheckinServiceTill { get; set; }
        public string[] Photos200x150 { get; set; }
        public string[] Photos800x600 { get; set; }
        public string[] Photos1020x700 { get; set; }
        public string Info { get; set; }
        public string ConstructionInfo { get; set; }
        public string TransportAccessability { get; set; }
        public string[] Services { get; set; }
        public dictionaryjson[] Tours { get; set; }
    }


    public class gdthPan
    {
        public int master_tour_id { get; set; }
        public int master_hotel_id { get; set; }
        public int master_room_id { get; set; }
        public int master_room_category_id { get; set; }
        public int master_pansion_id { get; set; }
    }

    public class Parts
    {
        public string Case { get; set; }
        public Field[][] Fields { get; set; }
    }

    public class Field
    {
        public int DayOfWeek { get; set; }
        public Info Info { get; set; }
    }

    public class Info
    {
        public string Title { get; set; }
        public string Detail { get; set; }
        public string[] Landmarks { get; set; }
        public string[] Areas { get; set; }
    }

    public class tourcontent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImportantInfo { get; set; }
        public string WarnInfo { get; set; }
        public string PaidFor { get; set; }
        public string RequiredCharges { get; set; }
        public string CancellationInfo { get; set; }
        public string Venue { get; set; }
        public int[] Hotels { get; set; }
        public Parts Parts { get; set; }
        public attr[] TourAttributes { get; set; }
        public string Rate { get; set; }
    }

    public class attr
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}