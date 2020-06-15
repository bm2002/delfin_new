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

    public class hotelquota
    {
        public int? Hotel { get; set; }
        public int? Room { get; set; }
        public int? RoomCat { get; set; }
        public int? Quota { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int? ReleasePeriod { get; set; }
        public bool ByRoom { get; set; }
        public int? Duration { get; set; }
    }

    public class hotelstop
    {
        public int Hotel { get; set; }
        public int Room { get; set; }
        public int RoomCat { get; set; }
        public string StartStop { get; set; }
        public string EndStop { get; set; }
        //public Period period { get; set; }
    }

    public class UserInfo
    {
        public string fullname { get; set; }
        public string legaladress { get; set; }
    }


    public class ExternHotelItem
    {
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public int PPTAreaId { get; set; }
        public bool HasFreeSale { get; set; }
    }

    public class AcademKurortCity
    {
        public int Item1 { get; set; }
        public int Item2 { get; set; }
        public int Item3 { get; set; }
    }

    public class Managerlogin
    {

        public class Rootobject
        {
            public string _case { get; set; }
            public Field[] fields { get; set; }
        }

        public class Field
        {
            public int id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public int legalEntity { get; set; }
            public int attr { get; set; }
        }
    }

    public class agencylogin
    {

        public class Rootobject
        {
            public string _case { get; set; }
            public Field[] fields { get; set; }
        }

        public class Field
        {
            public int id { get; set; }
            public string name { get; set; }
            public bool isCoordinator { get; set; }
            public bool isActive { get; set; }
            public bool isSupervisor { get; set; }
            public Agency agency { get; set; }
        }

        public class Agency
        {
            public int id { get; set; }
            public string name { get; set; }
            public int attr { get; set; }
            public int city { get; set; }
        }
    }

    public class dogGOI
    {
        public string error { get; set; }
        public string DG_CODE { get; set; }
        public string DG_MAINMENEMAIL { get; set; }
        public string DG_TURDATE { get; set; }
        public string DG_ENDDATE { get; set; }
        public int DG_NMEN { get; set; }
        public string DG_MAINMENPASPORT { get; set; }
        public string DG_MAINMENPHONE { get; set; }
        public int? DG_TRKEY { get; set; }
        public int DG_CNKEY { get; set; }
        public int DG_CTKEY { get; set; }
        public string TU_NAMERUS { get; set; }
        public string TU_FNAMERUS { get; set; }
        public string TU_SNAMERUS { get; set; }
        public string TU_NAMELAT { get; set; }
        public string TU_FNAMELAT { get; set; }
        public string TU_SNAMELAT { get; set; }
        public int TU_SEX { get; set; }
        public int TU_RealSex { get; set; }
        public int TU_ISMAIN { get; set; }
        public string TU_BIRTHDAY { get; set; }
        public string TU_EMAIL { get; set; }
        public string TU_PHONE { get; set; }
        public string TU_PHONECODE { get; set; }
        public string TU_PASPRUSER { get; set; }
        public string TU_PASPRUNUM { get; set; }
        public DateTime? TU_PASPRUDATE { get; set; }
        public string TU_PASPORTNUM { get; set; }
        public string TU_PASPORTTYPE { get; set; }
        public DateTime? TU_PASPORTDATE { get; set; }
        public DateTime? TU_PASPORTDATEEND { get; set; }
        public int? PR_CTKEY { get; set; }
        public string PR_CITY { get; set; }
        public string PR_LEGALADDRESS { get; set; }
        public int[] HDKEYS { get; set; }
    }

    public class tourhotelrequest
    {
        public int T { get; set; }
        public int H { get; set; }
    }

    public class Tours
    {
        public int? Tour { get; set; }
        public int? Hotel { get; set; }
        public int? Min01 { get; set; }
        public int? Min02 { get; set; }
        public int? Min03 { get; set; }
        public int? Min04 { get; set; }
        public int? Min05 { get; set; }
        public int? Min06 { get; set; }
        public int? Min07 { get; set; }
        public int? Min08 { get; set; }
        public int? Min09 { get; set; }
        public int? Min10 { get; set; }
        public int? Min11 { get; set; }
        public int? Min12 { get; set; }
        public int? Max01 { get; set; }
        public int? Max02 { get; set; }
        public int? Max03 { get; set; }
        public int? Max04 { get; set; }
        public int? Max05 { get; set; }
        public int? Max06 { get; set; }
        public int? Max07 { get; set; }
        public int? Max08 { get; set; }
        public int? Max09 { get; set; }
        public int? Max10 { get; set; }
        public int? Max11 { get; set; }
        public int? Max12 { get; set; }
        public int? TI_hdnights { get; set; }
    }
}